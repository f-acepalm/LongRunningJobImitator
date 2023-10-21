using LongRunningJobImitator.Accessors.Interfaces;
using LongRunningJobImitator.Accessors.Models;
using LongRunningJobImitator.Services.Interfaces;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Text;

namespace LongRunningJobImitator.Services.Services;

public class Base64TextConverter : ITextConverter
{
    private readonly ITextConversionResultSender _resultSender;
    private readonly ILogger<Base64TextConverter> _logger;
    private readonly ILongProcessImitator _longProcessImitator;
    private readonly IJobAccessor _jobAccessor;

    public Base64TextConverter(
        ITextConversionResultSender resultSender,
        ILogger<Base64TextConverter> logger,
        ILongProcessImitator longProcessImitator,
        IJobAccessor jobAccessor)
    {
        _resultSender = resultSender;
        _logger = logger;
        _longProcessImitator = longProcessImitator;
        _jobAccessor = jobAccessor;
    }

    public async Task ConvertAsync(Guid jobId, string text, CancellationToken cancellation)
    {
        _logger.LogInformation($"Starting conversion. JobId : {jobId}");
        ValidateInput(jobId, text);

        var convertedText = Base64Encode(text);
        await UpdateInProgressStatusAsync(jobId, convertedText, cancellation);

        for (var currentPosition = 0; currentPosition < convertedText.Length; currentPosition++)
        {
            await _longProcessImitator.DoSomething();
            var jobDoc = await _jobAccessor.GetAsync(jobId, cancellation);
            if (cancellation.IsCancellationRequested || jobDoc.Status == JobStatus.Canceled)
            {
                CancelJob(jobId);
            }

            _logger.LogInformation($"Processing '{convertedText[currentPosition]}' symbol. JobId : {jobId}");

            await _resultSender.SendResultAsync(jobId, convertedText[currentPosition].ToString(), cancellation);
            var updateResult = await _jobAccessor.UpdateProgressAsync(jobId, currentPosition, cancellation);
            if (updateResult.MatchedCount != 1)
            {
                CancelJob(jobId);
            }
        }

        await UpdateDoneStatusAsync(jobId, cancellation);
        await _resultSender.SendDoneAsync(jobId, cancellation);
        _logger.LogInformation($"Conversion is done. JobId : {jobId}");
    }

    internal string Base64Encode(string text)
    {
        ValidateInput(text);
        var textBytes = Encoding.UTF8.GetBytes(text);

        return Convert.ToBase64String(textBytes);
    }

    internal string Base64Decode(string base64String)
    {
        ValidateInput(base64String);
        var base64EncodedBytes = Convert.FromBase64String(base64String);

        return Encoding.UTF8.GetString(base64EncodedBytes);
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
            // TODO: Better handling, Canceled???
            throw new InvalidOperationException("Something wrong");
        }
    }

    private static void ValidateInput(Guid jobId, string text)
    {
        ValidateInput(text);

        if (jobId == default)
        {
            throw new ArgumentException("jobId can not be empty");
        }
    }

    private static void ValidateInput(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            throw new ArgumentException("Text can not be empty");
        }
    }
}
