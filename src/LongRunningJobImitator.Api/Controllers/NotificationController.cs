using LongRunningJobImitator.Api.Interfaces;
using LongRunningJobImitator.ClientContracts.Requests;
using Microsoft.AspNetCore.Mvc;

namespace LongRunningJobImitator.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class NotificationController : ControllerBase
{
    private readonly ISignalRSender _signalRSender;

    public NotificationController(ISignalRSender signalRSender)
    {
        _signalRSender = signalRSender;
    }

    [HttpPost("Result")]
    public async Task<ActionResult> SendResultAsync([FromBody] ResultNotificationRequest request, CancellationToken cancellation)
    {
        await _signalRSender.SendResultAsync(request.JobId, request.Result, cancellation);

        return Ok();
    }


    [HttpPost("Done")]
    public async Task<ActionResult> SendDoneAsync([FromBody] DoneNotificationRequest request, CancellationToken cancellation)
    {
        await _signalRSender.SendDoneAsync(request.JobId, cancellation);

        return Ok();
    }
}
