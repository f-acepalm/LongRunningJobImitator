using LongRunningJobImitator.BackgroundServices.Services;
using LongRunningJobImitator.Services.Interfaces;

namespace LongRunningJobImitator.BackgroundServices;

public static class ContainerExtensions
{
    public static IServiceCollection AddTextConversionBackgroundService(this IServiceCollection services)
    {
        services.AddSingleton<TextConversionBackgroundService>()
            .AddSingleton<ITextConversionBackgroundService>(
                provider => provider.GetRequiredService<TextConversionBackgroundService>())
            .AddHostedService(
                provider => provider.GetRequiredService<TextConversionBackgroundService>());

        return services;
    }
}
