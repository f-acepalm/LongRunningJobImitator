using LongRunningJobImitator.Accessors;
using LongRunningJobImitator.Accessors.Interfaces;
using LongRunningJobImitator.Api.Interfaces;
using LongRunningJobImitator.Api.Services;
using LongRunningJobImitator.Services;
using LongRunningJobImitator.Services.Interfaces;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Driver;

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

        public static IServiceCollection AddLongRunningJobImitatorAccessors(
            this IServiceCollection services,
            IConfiguration configuration)
        {            
            // TODO: Check:
            // Per the official Mongo Client reuse guidelines, MongoClient should be registered in DI with a singleton service lifetime.
            // https://mongodb.github.io/mongo-csharp-driver/2.14/reference/driver/connecting/#re-use
            services.Configure<DbSettings>(configuration.GetSection("Database"))
                .AddSingleton<IJobAccessor, JobAccessor>();

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
