using AutoMapper;
using Celebpretty.Application.Persistence;
using Celebpretty.Infrastructure.Common;
using Celebpretty.Infrastructure.Mongo.Models;
using MongoDB.Driver;
using System.Reflection;

namespace Celebpretty.Infrastructure.Mongo.Repositories;

public class CelebrityRepository : ICelebrityRepository, IDataInitializer
{
    private readonly IMongoCollection<Celebrity> _celebrityCollection;
    private readonly IMapper _mapper;

    public CelebrityRepository(IMongoCollection<Celebrity> celebrityCollection, IMapper mapper)
    {
        _celebrityCollection = celebrityCollection;
        _mapper = mapper;
    }

    public async Task Init(CancellationToken cancellationToken)
    {
        await _celebrityCollection.Indexes.CreateOneAsync(new CreateIndexModel<Celebrity>(Builders<Celebrity>.IndexKeys.Descending(x => x.Id)));
    }

    public async Task<Core.Domain.Celebrity> CreateCelebrity(Core.Domain.Celebrity celebrity, CancellationToken cancellationToken)
    {
        var dbCelebrity = _mapper.Map<Celebrity>(celebrity);
        await _celebrityCollection.InsertOneAsync(dbCelebrity, cancellationToken: cancellationToken);

        return _mapper.Map<Core.Domain.Celebrity>(dbCelebrity);
    }

    public async Task<Core.Domain.Celebrity> UpdateCelebrity(int id, Core.Domain.Celebrity celebrity, CancellationToken cancellationToken)
    {
        var dbCelebrity = _mapper.Map<Celebrity>(celebrity);
        var updatedCelebrity = await _celebrityCollection.FindOneAndReplaceAsync(
            filter: Builders<Celebrity>.Filter.Eq(x => x.Id, id),
            dbCelebrity, 
            new FindOneAndReplaceOptions<Celebrity>
            {
                ReturnDocument = ReturnDocument.After
            },
            cancellationToken);

        return _mapper.Map<Core.Domain.Celebrity>(updatedCelebrity);
    }

    public async Task<IEnumerable<Core.Domain.Celebrity>> GetCelebrities(CancellationToken cancellationToken)
    {
        var celebrities = await _celebrityCollection.FindAsync(
            filter: Builders<Celebrity>.Filter.Ne(x => x.Deleted, true),
            cancellationToken: cancellationToken);

        return _mapper.Map<IEnumerable<Core.Domain.Celebrity>>(celebrities);
    }

    public async Task<Core.Domain.Celebrity> GetCelebrity(int id, CancellationToken cancellationToken)
    {
        var result = await _celebrityCollection.Find(filter: Builders<Celebrity>.Filter.Eq(x => x.Id, id))
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        return result is null ? null : _mapper.Map<Core.Domain.Celebrity>(result);
    }
}
