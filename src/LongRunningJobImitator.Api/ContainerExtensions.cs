﻿using LongRunningJobImitator.Api.Services;
using LongRunningJobImitator.Services.Interfaces;

namespace LongRunningJobImitator.Api;

public static class ContainerExtensions
{
    public static IServiceCollection AddLongRunningJobImitatorApiServices(this IServiceCollection services)
    {
        services.AddTransient<ITextConversionResultSender, SignalRResultSender>();

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
