using LongRunningJobImitator.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace LongRunningJobImitator.Services;
public static class ContainerExtensions
{
    public static IServiceCollection AddLongRunningJobImitatorServices(this IServiceCollection services)
    {
        services.AddTransient<ITextConverter, Base64TextConverter>()
            .AddTransient<ILongProcessImitator, LongProcessImitator>()
            .AddTransient<IJobManager, JobManager>();

        return services;
    }
}
