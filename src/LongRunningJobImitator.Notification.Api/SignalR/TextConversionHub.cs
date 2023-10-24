using Microsoft.AspNetCore.SignalR;

namespace LongRunningJobImitator.Notification.Api.SignalR;

public class TextConversionHub : Hub
{
    public async Task AddToGroupAsync(string groupName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }

    public async Task RemoveFromGroupAsync(string groupName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
    }
}
