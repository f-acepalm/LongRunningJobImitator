using LongRunningJobImitator.Services.Models;

namespace LongRunningJobImitator.Services.Interfaces
{
    public interface ITextConversionWorker
    {
        Task StartJobAsync(StartWorkerModel model, CancellationToken cancellation);
    }
}
