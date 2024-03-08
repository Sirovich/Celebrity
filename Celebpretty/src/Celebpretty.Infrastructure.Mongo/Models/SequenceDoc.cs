using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Celebpretty.Infrastructure.Mongo.Models;

public class SequenceDoc
{
    public int Index { get; set; }

    [BsonId(IdGenerator = typeof(NullIdChecker))]
    public string SequenceName { get; set; }
}
