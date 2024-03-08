using Celebpretty.Application.Persistence;
using Celebpretty.Infrastructure.Mongo.Models;
using MongoDB.Driver;

namespace Celebpretty.Infrastructure.Mongo.Repositories;

public class SequencesRepository : ISequencesRepository
{
    private readonly IMongoCollection<SequenceDoc> _collection;

    public SequencesRepository(IMongoCollection<SequenceDoc> collection)
    {
        _collection = collection;
    }

    public async Task<int> SequenceInc(string seqName)
    {
        var index = await _collection.FindOneAndUpdateAsync(
           Builders<SequenceDoc>.Filter.Eq(x => x.SequenceName, seqName),
           Builders<SequenceDoc>.Update.Inc(x => x.Index, 1),
           new FindOneAndUpdateOptions<SequenceDoc>
           {
               IsUpsert = true,
               ReturnDocument = ReturnDocument.After
           });

        return index.Index;
    }
}
