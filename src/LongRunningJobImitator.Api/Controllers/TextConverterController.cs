using LongRunningJobImitator.Api.Mediator.Requests;
using LongRunningJobImitator.Api.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LongRunningJobImitator.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TextConverterController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TextConverterController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("start")]
        public async Task<ActionResult<TextConverterResponse>> StartProcessing([FromBody] TextConverterRequest request, CancellationToken cancellation)
        {
            var jobId = Guid.NewGuid();
            await _mediator.Send(new ConversionRequestedEvent(jobId, request.Text), cancellation);

            return new TextConverterResponse(jobId);
        }

        [HttpPost("cancel")]
        public async Task<ActionResult> CancelProcessing([FromBody] CancelConversionRequest request, CancellationToken cancellation)
        {
            await _mediator.Send(new ConversionCanceledEvent(request.JobId), cancellation);

            return Ok();
        }
    }
}