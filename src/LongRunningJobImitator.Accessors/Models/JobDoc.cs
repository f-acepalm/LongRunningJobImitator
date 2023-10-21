using MongoDB.Bson.Serialization.Attributes;

namespace LongRunningJobImitator.Accessors.Models;

public record JobDoc(
    [property:BsonId]
    Guid Id,
    JobStatus Status,
    string Text,
    string Result,
    int ProcessingPosition
    );