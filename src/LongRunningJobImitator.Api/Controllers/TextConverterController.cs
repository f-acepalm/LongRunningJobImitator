using LongRunningJobImitator.Api.Models;
using LongRunningJobImitator.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LongRunningJobImitator.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class TextConverterController : ControllerBase
{
    private readonly IJobManager _jobManager;

    public TextConverterController(IJobManager jobManager)
    {
        _jobManager = jobManager;
    }

    [HttpPost("start")]
    public async Task<ActionResult<TextConverterResponse>> StartProcessing([FromBody] TextConverterRequest request, CancellationToken cancellation)
    {
        var jobId = await _jobManager.StartJobAsync(new(request.Text), cancellation);

        return new TextConverterResponse(jobId);
    }

    [HttpPost("cancel")]
    public async Task<ActionResult> CancelProcessing([FromBody] CancelConversionRequest request, CancellationToken cancellation)
    {
        await _jobManager.CancelJobAsync(new(request.JobId), cancellation);

        return Ok();
    }
}