namespace LongRunningJobImitator.Api.Models;
public record ResultNotificationRequest(Guid JobId, string Result);
