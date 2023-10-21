using LongRunningJobImitator.Accessors;
using LongRunningJobImitator.Services.Interfaces;
using LongRunningJobImitator.Services.Services;
using LongRunningJobImitator.Services.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LongRunningJobImitator.Services;
public static class ContainerExtensions
{
    public static IServiceCollection AddJobServices(this IServiceCollection services, IConfiguration configuration)
    {
        var signalRSettings = configuration.GetRequiredSection(Constants.SignalRClientName).Get<SignalRHubSettings>();
        
        services
            .AddLongRunningJobImitatorAccessors(configuration)
            .AddTransient<ITextConverter, Base64TextConverter>()
            .AddTransient<ILongProcessImitator, LongProcessImitator>()
            .AddTransient<ITextConversionResultSender, HttpResultSender>()
            .AddHttpClient(Constants.SignalRClientName, httpClient =>
            {
                httpClient.BaseAddress = new Uri(signalRSettings.Url);
            });

        return services;
    }

    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        var jobApiSettings = configuration.GetRequiredSection(Constants.JobClientName).Get<JobApiSettings>();

        services.AddTransient<IJobManager, JobManager>()
            .AddLongRunningJobImitatorAccessors(configuration)
            .AddHttpClient(Constants.JobClientName, httpClient =>
            {
                httpClient.BaseAddress = new Uri(jobApiSettings.Url);
            }); ;

        return services;
    }
}
