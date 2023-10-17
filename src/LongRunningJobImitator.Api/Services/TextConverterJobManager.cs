using LongRunningJobImitator.Api.Interfaces;
using LongRunningJobImitator.Services.Interfaces;

namespace LongRunningJobImitator.Api.Services
{
    public class TextConverterJobManager : IJobManager
    {
        private readonly ITextConversionBackgroundService _backgroundService;
        private readonly ITextConverter _textConverter;

        public TextConverterJobManager(ITextConversionBackgroundService backgroundService, ITextConverter textConverter)
        {
            _backgroundService = backgroundService;
            _textConverter = textConverter;
        }

        public async Task<Guid> RunTextConversionAsync(string text)
        {
            var jobId = Guid.NewGuid();
            var convertedText = _textConverter.Convert(text);
            _backgroundService.StartProcessing(jobId, convertedText);

            return await Task.FromResult(jobId);
        }

        public async Task CancelTextConversionAsync(Guid jobId)
        {
            _backgroundService.CancelProcessing(jobId);

            await Task.CompletedTask;
        }
    }
}