namespace LongRunningJobImitator.BackgroundServices.Models;

public record StartJobRequest(Guid JobId, string text);