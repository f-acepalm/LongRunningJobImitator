namespace LongRunningJobImitator.Services.Interfaces
{
    public interface ITextConverter
    {
        Task ConvertAsync(Guid jobId, string text, CancellationToken cancellation);
    }
}
