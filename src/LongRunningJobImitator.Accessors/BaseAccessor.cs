using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace LongRunningJobImitator.Accessors;
public abstract class BaseAccessor<T>
{
    protected readonly IMongoCollection<T> Collection;

    public BaseAccessor(IOptions<DbSettings> settings)
    {
        var mongoClient = new MongoClient(settings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(settings.Value.DatabaseName);
        Collection = mongoDatabase.GetCollection<T>(settings.Value.CollectionName);
    }
}
