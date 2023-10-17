using LongRunningJobImitator.Api.Interfaces;
using System.Collections.Concurrent;

namespace LongRunningJobImitator.Api.Services
{
    public class TextConversionBackgroundService : BackgroundService, ITextConversionBackgroundService
    {
        private readonly ConcurrentDictionary<Guid, CancellationTokenSource> _tokens = new ConcurrentDictionary<Guid, CancellationTokenSource>();
        private readonly ITextConversionResultSender _resultSender;
        private readonly ILogger<TextConversionBackgroundService> _logger;

        public TextConversionBackgroundService(
            ITextConversionResultSender resultSender,
            ILogger<TextConversionBackgroundService> logger)
        {
            _resultSender = resultSender;
            _logger = logger;
        }

        //TODO: Not void
        public void StartProcessing(Guid jobId, string text)
        {
            var tokenSource = new CancellationTokenSource();
            if (_tokens.TryAdd(jobId, tokenSource))
            {
                //TODO: Check, exceptions
                Task.Run(async () => await ProcessText(jobId, text, tokenSource.Token));
            }
        }

        public void CancelProcessing(Guid jobId)
        {
            _logger.LogInformation($"Cancelation was requested for job: {jobId}");

            if (_tokens.TryRemove(jobId, out var tokenSource))
            {
                tokenSource.Cancel();
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }

        private async Task ProcessText(Guid jobId, string text, CancellationToken cancellation)
        {
            _logger.LogInformation($"Starting job: {jobId}");

            foreach (var currentSymbol in text)
            {
                await RandomDelay();
                await _resultSender.SendResultAsync(jobId, currentSymbol.ToString());

                if (cancellation.IsCancellationRequested)
                {
                    _logger.LogInformation($"Job was canceled: {jobId}");
                    _tokens.Remove(jobId, out _);

                    return;
                }
            }

            _logger.LogInformation($"Job is done: {jobId}");
            _tokens.Remove(jobId, out _);
            await _resultSender.SendDoneAsync(jobId);
        }

        private static async Task RandomDelay()
        {
            var random = new Random();
            var delay = random.Next(1000, 1500);
            await Task.Delay(delay);
        }
    }
}
