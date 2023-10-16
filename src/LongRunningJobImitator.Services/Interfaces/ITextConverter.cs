using LongRunningJobImitator.Services.Models;

namespace LongRunningJobImitator.Services.Interfaces
{
    public interface ITextConverter
    {
        Task CancelConversionAsync(Guid jobId);
        Task<ConversionResponse> RunConversionAsync(string text);
    }
}