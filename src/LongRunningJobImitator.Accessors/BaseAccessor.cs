using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace LongRunningJobImitator.Accessors;
public abstract class BaseAccessor<T>
{
    protected readonly IMongoCollection<T> Collection;

    static BaseAccessor()
    {
        // TODO: check
        BsonDefaults.GuidRepresentationMode = GuidRepresentationMode.V3;
        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
    }

    public BaseAccessor(IOptions<DbSettings> settings)
    {
        var mongoClient = new MongoClient(settings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(settings.Value.DatabaseName);
        Collection = mongoDatabase.GetCollection<T>(settings.Value.CollectionName);
    }
}
