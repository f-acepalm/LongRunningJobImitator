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
            services.AddTransient<ITextConverter, Base64TextConverter>()
                .AddTransient<ITextConversionResultSender, SignalRResultSender>()
                .AddTransient<ILongProcessImitator, LongProcessImitator>();

            return services;
        }

        public static IServiceCollection AddBackgroundServices(this IServiceCollection services)
        {
            services.AddSingleton<TextConversionBackgroundService>()
                .AddSingleton<ITextConversionBackgroundService>(
                    provider => provider.GetRequiredService<TextConversionBackgroundService>())
                .AddHostedService(
                    provider => provider.GetRequiredService<TextConversionBackgroundService>());

            return services;
        }
    }
}
