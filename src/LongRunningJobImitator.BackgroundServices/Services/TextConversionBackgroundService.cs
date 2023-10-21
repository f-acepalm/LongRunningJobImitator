using LongRunningJobImitator.Services.Interfaces;
using System.Collections.Concurrent;

namespace LongRunningJobImitator.BackgroundServices.Services;

public class TextConversionBackgroundService : BackgroundService, ITextConversionBackgroundService
{
    private readonly ConcurrentDictionary<Guid, CancellationTokenSource> _tokens = new ConcurrentDictionary<Guid, CancellationTokenSource>();
    private readonly ITextConverter _textConverter;
    private readonly ILogger<TextConversionBackgroundService> _logger;

    public TextConversionBackgroundService(
        ITextConverter textConverter,
        ILogger<TextConversionBackgroundService> logger)
    {
        _textConverter = textConverter;
        _logger = logger;
    }

    public async Task StartProcessingAsync(Guid jobId, string text)
    {
        _logger.LogInformation($"Starting job: {jobId}");

        var tokenSource = new CancellationTokenSource();
        if (_tokens.TryAdd(jobId, tokenSource))
        {
            //TODO: Check, exceptions
            Task.Run(async () => await ProcessText(jobId, text, tokenSource.Token));
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

    private async Task ProcessText(Guid jobId, string text, CancellationToken cancellation)
    {
        try
        {
            await _textConverter.ConvertAsync(jobId, text, cancellation);
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
