using LongRunningJobImitator.Services.Interfaces;
using System.Collections.Concurrent;

namespace LongRunningJobImitator.BackgroundServices.Services;

public class TextConversionBackgroundService : BackgroundService, ITextConversionBackgroundService
{
    private readonly ConcurrentDictionary<Guid, CancellationTokenSource> _tokens = new ConcurrentDictionary<Guid, CancellationTokenSource>();
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<TextConversionBackgroundService> _logger;

    public TextConversionBackgroundService(
        IServiceProvider serviceProvider,
        ILogger<TextConversionBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task StartProcessingAsync(Guid jobId)
    {
        _logger.LogInformation($"Starting job: {jobId}");

        var tokenSource = new CancellationTokenSource();
        if (_tokens.TryAdd(jobId, tokenSource))
        {
            _ = Task.Run(async () => await ProcessText(jobId, tokenSource.Token));
        }

        await Task.CompletedTask;
    }

    public async Task CancelProcessingAsync(Guid jobId)
    {
        _logger.LogInformation($"Job canceled: {jobId}");

        if (_tokens.TryRemove(jobId, out var tokenSource))
        {
            tokenSource.Cancel();
        }

        await Task.CompletedTask;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }

        foreach (var token in _tokens.Values)
        {
            token.Cancel();
        }
    }

    private async Task ProcessText(Guid jobId, CancellationToken cancellation)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var worker = scope.ServiceProvider.GetRequiredService<ITextConversionWorker>();
            
            await worker.StartJobAsync(new(jobId), cancellation);

            if (_tokens.TryRemove(jobId, out _))
            {
                _logger.LogInformation($"Job is done: {jobId}");
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation($"Job was canceled. JobId : {jobId}");
        }
    }
}
