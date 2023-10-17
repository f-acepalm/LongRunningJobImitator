using LongRunningJobImitator.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Text;

namespace LongRunningJobImitator.Services
{
    public class Base64TextConverter : ITextConverter
    {
        private readonly ITextConversionResultSender _resultSender;
        private readonly ILogger<Base64TextConverter> _logger;

        public Base64TextConverter(ITextConversionResultSender resultSender, ILogger<Base64TextConverter> logger)
        {
            _resultSender = resultSender;
            _logger = logger;
        }

        public async Task ConvertAsync(Guid jobId, string text, CancellationToken cancellation)
        {
            _logger.LogInformation($"Starting conversion. JobId : {jobId}");

            ValidateInput(jobId, text);
            var convertedText = Base64Encode(text);

            foreach (var currentSymbol in convertedText)
            {
                await RandomDelay();
                await _resultSender.SendResultAsync(jobId, currentSymbol.ToString());

                if (cancellation.IsCancellationRequested)
                {
                    _logger.LogInformation($"Conversion was canceled. JobId : {jobId}");

                    return;
                }
            }

            _logger.LogInformation($"Conversion is done. JobId : {jobId}");
            await _resultSender.SendDoneAsync(jobId);
        }

        internal string Base64Encode(string text)
        {
            ValidateInput(text);
            var textBytes = Encoding.UTF8.GetBytes(text);

            return Convert.ToBase64String(textBytes);
        }

        internal string Base64Decode(string base64String)
        {
            ValidateInput(base64String);
            var base64EncodedBytes = Convert.FromBase64String(base64String);
            
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        private static void ValidateInput(Guid jobId, string text)
        {
            ValidateInput(text);

            if (jobId == default)
            {
                throw new ArgumentException("jobId can not be empty");
            }
        }

        private static void ValidateInput(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentException("Text can not be empty");
            }
        }

        private static async Task RandomDelay()
        {
            var random = new Random();
            var delay = random.Next(1000, 1500);
            await Task.Delay(delay);
        }
    }
}
