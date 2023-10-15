using LongRunningJobImitator.Services;
using LongRunningJobImitator.Services.Interfaces;

namespace LongRunningJobImitator.Api
{
    public static class ContainerExtensions
    {
        public static IServiceCollection AddLongRunningJobImitatorServices(this IServiceCollection services)
        {
            services.AddSingleton<ITextConverter, TextConverter>()
                .AddSingleton<BackgroundPrintingService>()
                .AddSingleton<ITextConversionBackgroundService>(
                    provider => provider.GetRequiredService<BackgroundPrintingService>())
                .AddHostedService(
                    provider => provider.GetRequiredService<BackgroundPrintingService>());

            return services;
        }
    }
}
