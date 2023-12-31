﻿using FluentValidation;
using LongRunningJobImitator.Accessors.Interfaces;
using LongRunningJobImitator.Accessors.Models;
using LongRunningJobImitator.Services.Interfaces;
using LongRunningJobImitator.Services.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace LongRunningJobImitator.Services.Services;

public class LongRunningConversionWorker : ITextConversionWorker
{
    private readonly ITextConversionResultSender _resultSender;
    private readonly ILogger<LongRunningConversionWorker> _logger;
    private readonly ILongProcessImitator _longProcessImitator;
    private readonly IJobAccessor _jobAccessor;
    private readonly ITextEncoder _textEncoder;
    private readonly IValidator<StartWorkerModel> _startWorkerValidator;

    public LongRunningConversionWorker(
        ITextConversionResultSender resultSender,
        ILogger<LongRunningConversionWorker> logger,
        ILongProcessImitator longProcessImitator,
        IJobAccessor jobAccessor,
        ITextEncoder textEncoder,
        IValidator<StartWorkerModel> startWorkerValidator)
    {
        _resultSender = resultSender;
        _logger = logger;
        _longProcessImitator = longProcessImitator;
        _jobAccessor = jobAccessor;
        _textEncoder = textEncoder;
        _startWorkerValidator = startWorkerValidator;
    }

    public async Task StartJobAsync(StartWorkerModel model, CancellationToken cancellation)
    {
        _logger.LogInformation($"Starting conversion. JobId : {model.JobId}");

        _startWorkerValidator.ValidateAndThrow(model);

        var jobId = model.JobId;
        var jobDoc = await _jobAccessor.GetAsync(jobId, cancellation);
        var convertedText = _textEncoder.Encode(new(jobDoc.Text));
        await UpdateInProgressStatusAsync(jobId, convertedText, cancellation);
        var currentPosition = jobDoc.ProcessingPosition;

        while (currentPosition < convertedText.Length)
        {
            await _longProcessImitator.DoSomething();
            await CheckCancelationAsync(jobId, cancellation);

            _logger.LogInformation($"Processing '{convertedText[currentPosition]}' symbol. JobId : {jobId}");

            await _resultSender.SendResultAsync(jobId, convertedText[currentPosition].ToString(), cancellation);
            await UpdateProgressAsync(jobId, currentPosition++, cancellation);
        }

        await UpdateDoneStatusAsync(jobId, cancellation);
        await _resultSender.SendDoneAsync(jobId, cancellation);
        _logger.LogInformation($"Conversion is done. JobId : {jobId}");
    }

    private async Task UpdateProgressAsync(Guid jobId, int currentPosition, CancellationToken cancellation)
    {
        var updateResult = await _jobAccessor.UpdateProgressAsync(jobId, currentPosition, cancellation);
        if (updateResult.MatchedCount != 1)
        {
            CancelJob(jobId);
        }
    }

    private async Task CheckCancelationAsync(Guid jobId, CancellationToken cancellation)
    {
        var jobDoc = await _jobAccessor.GetAsync(jobId, cancellation);
        if (cancellation.IsCancellationRequested || jobDoc.Status == JobStatus.Canceled)
        {
            CancelJob(jobId);
        }
    }

    private void CancelJob(Guid jobId)
    {
        var message = $"Conversion was canceled. JobId : {jobId}";
        _logger.LogInformation(message);
        throw new OperationCanceledException(message);
    }

    private async Task UpdateDoneStatusAsync(Guid jobId, CancellationToken cancellation)
    {
        var updateResult = await _jobAccessor.UpdateToDoneAsync(jobId, cancellation);
        HandleUpdateResult(updateResult);
    }

    private async Task UpdateInProgressStatusAsync(Guid jobId, string convertedText, CancellationToken cancellation)
    {
        var updateResult = await _jobAccessor.UpdateToInProgressAsync(jobId, convertedText, cancellation);
        HandleUpdateResult(updateResult);
    }

    private void HandleUpdateResult(UpdateResult startResult)
    {
        if (startResult.ModifiedCount != 1)
        {
            throw new InvalidOperationException("Job state is invalid");
        }
    }

    private static void ValidateInput(Guid jobId)
    {
        if (jobId == default)
        {
            throw new ArgumentException("jobId can not be empty");
        }
    }
}
