using LongRunningJobImitator.Services.Interfaces;
using Microsoft.Extensions.Hosting;
using System.Collections.Concurrent;

namespace LongRunningJobImitator.Services
{
    public class BackgroundPrintingService : BackgroundService, ITextConversionBackgroundService
    {
        private readonly ConcurrentDictionary<Guid, CancellationTokenSource> _tokens = new ConcurrentDictionary<Guid, CancellationTokenSource>();

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
                Console.WriteLine($"{jobId}: {currentSymbol}");
                await Task.Delay(1000);

                if (cancellation.IsCancellationRequested)
                {
                    Console.WriteLine("Canceled!");
                    break;
                }
            }

            Console.WriteLine($"Done for: {jobId}");
            _tokens.Remove(jobId, out _);
        }
    }
}
