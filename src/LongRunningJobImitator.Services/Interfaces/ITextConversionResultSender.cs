namespace LongRunningJobImitator.Services.Interfaces
{
    public interface ITextConversionResultSender
    {
        Task SendResultAsync(Guid jobId, string result);

        Task SendDoneAsync(Guid jobId);
    }
}
