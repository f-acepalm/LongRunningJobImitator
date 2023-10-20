using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace LongRunningJobImitator.Accessors.Models;

public record JobDoc(
    [property:BsonId]
    Guid Id,
    JobStatus Status,
    string Text,
    int ProcessingPosition
    );