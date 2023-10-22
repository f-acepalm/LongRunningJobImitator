using LongRunningJobImitator.BackgroundServices.Interfaces;
using LongRunningJobImitator.ClientContracts.Requests;
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
    public async Task<ActionResult> StartAsync(StartJobRequest request)
    {
        await _backgroundService.StartProcessingAsync(request.JobId);

        return Ok();
    }
}
