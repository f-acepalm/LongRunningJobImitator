using LongRunningJobImitator.Accessors;
using LongRunningJobImitator.Services.Interfaces;
using LongRunningJobImitator.Services.Services;
using LongRunningJobImitator.Services.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly.Extensions.Http;
using Polly;

namespace LongRunningJobImitator.Services;
public static class ContainerExtensions
{
    public static IServiceCollection AddJobServices(this IServiceCollection services, IConfiguration configuration)
    {
        var signalRSettings = configuration.GetRequiredSection(Constants.SignalRClientName).Get<SignalRHubSettings>();

        services
            .AddLongRunningJobImitatorAccessors(configuration)
            .AddTransient<ITextConversionWorker, LongRunningConversionWorker>()
            .AddTransient<ILongProcessImitator, LongProcessImitator>()
            .AddTransient<ITextConversionResultSender, HttpResultSender>()
            .AddTransient<ITextEncoder, Base64Encoder>()
            .AddTransient<IRetriableHttpClient, RetriableHttpClient>()
            .AddHttpClientWithRetry(Constants.SignalRClientName, signalRSettings.Url);

        return services;
    }

    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        var jobApiSettings = configuration.GetRequiredSection(Constants.JobClientName).Get<JobApiSettings>();

        services.AddTransient<IJobManager, JobManager>()
            .AddLongRunningJobImitatorAccessors(configuration)
            .AddTransient<IRetriableHttpClient, RetriableHttpClient>()
            .AddHttpClientWithRetry(Constants.JobClientName, jobApiSettings.Url);

        return services;
    }

    private static IServiceCollection AddHttpClientWithRetry(this IServiceCollection services, string clientName, string baseUrl)
    {
        services.AddHttpClient(clientName, httpClient =>
        {
            httpClient.BaseAddress = new Uri(baseUrl);
        })
            .SetHandlerLifetime(TimeSpan.FromMinutes(5))
            .AddPolicyHandler(GetRetryPolicy());

        return services;
    }

    static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
            .WaitAndRetryAsync(
                3, // TODO: configs
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }
}
