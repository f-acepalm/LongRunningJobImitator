using LongRunningJobImitator.Api.Interfaces;
using LongRunningJobImitator.Services.Models;

namespace LongRunningJobImitator.Api.Services
{
    public class TextConverter : ITextConverter
    {
        private readonly ITextConversionBackgroundService _backgroundService;

        public TextConverter(ITextConversionBackgroundService backgroundService)
        {
            _backgroundService = backgroundService;
        }

        public async Task<ConversionResponse> RunConversionAsync(string text)
        {
            var jobId = Guid.NewGuid();
            _backgroundService.StartProcessing(jobId, text);

            return await Task.FromResult(new ConversionResponse(jobId));
        }

        public async Task CancelConversionAsync(Guid jobId)
        {
            _backgroundService.CancelProcessing(jobId);

            await Task.CompletedTask;
        }
    }
}