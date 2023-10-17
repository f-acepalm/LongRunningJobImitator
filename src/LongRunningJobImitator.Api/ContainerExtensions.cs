using LongRunningJobImitator.Api.Interfaces;
using LongRunningJobImitator.Api.Services;
using LongRunningJobImitator.Services;
using LongRunningJobImitator.Services.Interfaces;

namespace LongRunningJobImitator.Api
{
    public static class ContainerExtensions
    {
        public static IServiceCollection AddLongRunningJobImitatorServices(this IServiceCollection services)
        {
            services.AddTransient<IJobManager, TextConverterJobManager>()
                .AddTransient<ITextConverter, Base64TextConverter>()
                .AddTransient<ITextConversionResultSender, SignalRResultSender>()
                .AddSingleton<BackgroundPrintingService>()
                .AddSingleton<ITextConversionBackgroundService>(
                    provider => provider.GetRequiredService<BackgroundPrintingService>())
                .AddHostedService(
                    provider => provider.GetRequiredService<BackgroundPrintingService>());

            return services;
        }
    }
}
