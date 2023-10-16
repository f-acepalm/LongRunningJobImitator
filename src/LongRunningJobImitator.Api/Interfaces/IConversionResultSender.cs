namespace LongRunningJobImitator.Api.Interfaces
{
    public interface IConversionResultSender
    {
        Task SendAsync(Guid jobId, string result);
    }
}
