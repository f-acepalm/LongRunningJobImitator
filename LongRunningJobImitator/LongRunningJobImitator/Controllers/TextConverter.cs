using LongRunningJobImitator.Models;
using Microsoft.AspNetCore.Mvc;

namespace LongRunningJobImitator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TextConverter : ControllerBase
    {
        private readonly ILogger<TextConverter> _logger;

        public TextConverter(ILogger<TextConverter> logger)
        {
            _logger = logger;
        }

        [HttpPost("start")]
        public ActionResult<TextConverterResponse> StartProcessing([FromBody] TextConverterRequest request)
        {
            _logger.LogInformation($"Accepted text: {request.Text}");

            return new TextConverterResponse { Result = request.Text, JobId = 1 };
        }
    }
}