using LongRunningJobImitator.Api.Interfaces;
using LongRunningJobImitator.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace LongRunningJobImitator.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TextConverterController : ControllerBase
    {
        private readonly IJobManager _service;
        private readonly ILogger<TextConverterController> _logger;

        public TextConverterController(
            IJobManager service,
            ILogger<TextConverterController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost("start")]
        public async Task<ActionResult<TextConverterResponse>> StartProcessing([FromBody] TextConverterRequest request)
        {
            _logger.LogInformation($"Accepted text: {request.Text}");
            
            var jobId = await _service.RunTextConversionAsync(request.Text);

            _logger.LogInformation($"Job {jobId} has started");

            return new TextConverterResponse(jobId);
        }

        [HttpPost("cancel")]
        public async Task<ActionResult> CancelProcessing([FromBody] CancelConversionRequest request)
        {
            _logger.LogInformation($"Cancelling job with id {request.JobId}");

            await _service.CancelTextConversionAsync(request.JobId);

            _logger.LogInformation($"Job with id {request.JobId} was cancelled");

            return Ok();
        }
    }
}