namespace LongRunningJobImitator.Api;

public static class ContainerExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        return services;
    }

    public static IApplicationBuilder UseGlobalExceptionHandling(this IApplicationBuilder applicationBuilder)
    {
        applicationBuilder.UseMiddleware<ExceptionHandlerMiddleware>();

        return applicationBuilder;
    }
}
