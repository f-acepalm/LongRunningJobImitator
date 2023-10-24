using LongRunningJobImitator.ClientContracts.Requests;
using LongRunningJobImitator.Notification.Api.SignalR;
using Microsoft.AspNetCore.Mvc;

namespace LongRunningJobImitator.Notification.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationController : ControllerBase
{
    private readonly INotifier _notifier;

    public NotificationController(INotifier notifier)
    {
        _notifier = notifier;
    }

    [HttpPost("Result")]
    public async Task<ActionResult> SendResultAsync([FromBody] ResultNotificationRequest request, CancellationToken cancellation)
    {
        await _notifier.SendResultAsync(request.JobId, request.Result, cancellation);

        return Ok();
    }


    [HttpPost("Done")]
    public async Task<ActionResult> SendDoneAsync([FromBody] DoneNotificationRequest request, CancellationToken cancellation)
    {
        await _notifier.SendDoneAsync(request.JobId, cancellation);

        return Ok();
    }
}
