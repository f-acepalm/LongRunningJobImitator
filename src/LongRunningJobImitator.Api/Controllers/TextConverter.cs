using LongRunningJobImitator.Api.Models;
using LongRunningJobImitator.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LongRunningJobImitator.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TextConverter : ControllerBase
    {
        private readonly ITextConverter _service;
        private readonly ILogger<TextConverter> _logger;

        public TextConverter(
            ITextConverter service,
            ILogger<TextConverter> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost("start")]
        public async Task<ActionResult<TextConverterResponse>> StartProcessing([FromBody] TextConverterRequest request)
        {
            _logger.LogInformation($"Accepted text: {request.Text}");
            
            var result = await _service.RunConversionAsync(request.Text);

            return new TextConverterResponse(result.JobId, request.Text);
        }

        [HttpPost("cancel")]
        public async Task<ActionResult> CancelProcessing([FromBody] CancelConversionRequest request)
        {
            _logger.LogInformation($"Cancelling job with id {request.JobId}");

            await _service.CancelConversionAsync(request.JobId);

            return Ok();
        }
    }
}