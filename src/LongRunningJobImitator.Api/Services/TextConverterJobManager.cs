using LongRunningJobImitator.Api.Interfaces;

namespace LongRunningJobImitator.Api.Services
{
    public class TextConverterJobManager : IJobManager // TODO: Mediator
    {
        private readonly ITextConversionBackgroundService _backgroundService;

        public TextConverterJobManager(ITextConversionBackgroundService backgroundService)
        {
            _backgroundService = backgroundService;
        }

        public async Task<Guid> RunTextConversionAsync(string text)
        {
            var jobId = Guid.NewGuid();
            _backgroundService.StartProcessing(jobId, text);

            return await Task.FromResult(jobId);
        }

        public async Task CancelTextConversionAsync(Guid jobId)
        {
            _backgroundService.CancelProcessing(jobId);

            await Task.CompletedTask;
        }
    }
}