using LongRunningJobImitator.Notification.Api.SignalR;

namespace LongRunningJobImitator.Api;

public static class ContainerExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        return services.AddTransient<INotifier, SignalRNotifier>();
    }
}
