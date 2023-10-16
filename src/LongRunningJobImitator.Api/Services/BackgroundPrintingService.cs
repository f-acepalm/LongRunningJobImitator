using LongRunningJobImitator.Api.Interfaces;
using System.Collections.Concurrent;

namespace LongRunningJobImitator.Api.Services
{
    public class BackgroundPrintingService : BackgroundService, ITextConversionBackgroundService
    {
        private readonly ConcurrentDictionary<Guid, CancellationTokenSource> _tokens = new ConcurrentDictionary<Guid, CancellationTokenSource>();
        private readonly IConversionResultSender _resultSender;
        private readonly ILogger<BackgroundPrintingService> _logger;

        public BackgroundPrintingService(
            IConversionResultSender resultSender,
            ILogger<BackgroundPrintingService> logger)
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
                //TODO: Check
                Task.Run(async () => await ProcessText(jobId, text, tokenSource.Token));
            }
        }

        public void CancelProcessing(Guid jobId)
        {
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
            foreach (var currentSymbol in text)
            {
                await _resultSender.SendAsync(jobId, currentSymbol.ToString());
                await Task.Delay(1000);

                if (cancellation.IsCancellationRequested)
                {
                    await _resultSender.SendAsync(jobId, "Canceled!");
                    break;
                }
            }

            _logger.LogInformation($"Done for: {jobId}");
            _tokens.Remove(jobId, out _);
        }
    }
}
