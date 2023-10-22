namespace LongRunningJobImitator.ClientContracts.Requests;
public record ResultNotificationRequest(Guid JobId, string Result);
