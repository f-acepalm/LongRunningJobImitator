namespace LongRunningJobImitator.Api.Interfaces
{
    public interface ITextConversionResultSender
    {
        Task SendResultAsync(Guid jobId, string result);

        Task SendDoneAsync(Guid jobId);

        Task SendCanceledAsync(Guid jobId); // TODO: remove
    }
}
