using LongRunningJobImitator.Accessors.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;

namespace LongRunningJobImitator.Accessors;
public static class ContainerExtensions
{
    public static IServiceCollection AddLongRunningJobImitatorAccessors(
            this IServiceCollection services,
            IConfiguration configuration)
    {
        BsonDefaults.GuidRepresentationMode = GuidRepresentationMode.V3;
        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

        // Per the official Mongo Client reuse guidelines, MongoClient should be registered in DI with a singleton service lifetime.
        // https://mongodb.github.io/mongo-csharp-driver/2.14/reference/driver/connecting/#re-use
        services.Configure<DbSettings>(configuration.GetSection("Database"))
            .AddSingleton<IJobAccessor, JobAccessor>();

        return services;
    }
}
