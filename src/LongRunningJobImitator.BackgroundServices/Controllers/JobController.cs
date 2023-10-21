using LongRunningJobImitator.BackgroundServices.Models;
using LongRunningJobImitator.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LongRunningJobImitator.BackgroundServices.Controllers;
[ApiController]
[Route("[controller]")]
public class JobController : ControllerBase
{
    private readonly ITextConversionBackgroundService _backgroundService;

    public JobController(ITextConversionBackgroundService backgroundService)
    {
        _backgroundService = backgroundService;
    }

    [HttpPost("start")]
    public async Task<ActionResult> StartAsync(StartJobRequest request, CancellationToken cancellation)
    {
        await _backgroundService.StartProcessingAsync(request.JobId, request.text);

        return Ok();
    }
}
