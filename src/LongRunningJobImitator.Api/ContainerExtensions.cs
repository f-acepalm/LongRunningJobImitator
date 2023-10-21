using LongRunningJobImitator.Api.Interfaces;
using LongRunningJobImitator.Api.Services;

namespace LongRunningJobImitator.Api;

public static class ContainerExtensions
{
    public static IServiceCollection AddLongRunningJobImitatorApiServices(this IServiceCollection services)
    {
        services.AddTransient<ISignalRSender, SignalRSender>();

        return services;
    }
}
