namespace LongRunningJobImitator.Api.Interfaces
{
    public interface ITextConversionBackgroundService
    {
        void StartProcessing(Guid jobId, string text);

        void CancelProcessing(Guid jobId);
    }
}
