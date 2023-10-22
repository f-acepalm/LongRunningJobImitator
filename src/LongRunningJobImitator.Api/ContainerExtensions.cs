using LongRunningJobImitator.Api.SignalR;

namespace LongRunningJobImitator.Api;

public static class ContainerExtensions
{
    public static IServiceCollection AddLongRunningJobImitatorApiServices(this IServiceCollection services)
    {
        return services.AddTransient<ISignalRSender, SignalRSender>();
    }

    public static IApplicationBuilder UseGlobalExceptionHandling(this IApplicationBuilder applicationBuilder)
    {
        applicationBuilder.UseMiddleware<ExceptionHandlerMiddleware>();

        return applicationBuilder;
    }
}
