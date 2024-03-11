using AutoMapper;
using Celebpretty.Application.Main.Models;
using Celebpretty.Application.Main.Models.Error;
using Celebpretty.Application.Persistence;
using Microsoft.Extensions.Caching.Memory;
using System.Security.AccessControl;

namespace Celebpretty.Application.Main;

public class CelebrityService : ICelebrityService
{
    private readonly ICelebrityRepository _repository;
    private readonly IMapper _mapper;
    private readonly IIdGenerator _idGenerator;
    private readonly IMemoryCache _memoryCache;
    private readonly IScraper _scraper;
    private const string cacheKey = "celebrities";

    public CelebrityService(ICelebrityRepository repository, IMapper mapper, IIdGenerator idGenerator, IMemoryCache memoryCache, IScraper scraper)
    {
        _repository = repository;
        _mapper = mapper;
        _idGenerator = idGenerator;
        _memoryCache = memoryCache;
        _scraper = scraper;
    }

    public async Task<CreateCelebrityRes> CreateCelebrity(Celebrity celebrity, CancellationToken cancellationToken)
    {
        var coreCelebrity = _mapper.Map<Core.Domain.Celebrity>(celebrity);
        coreCelebrity.Id = await _idGenerator.GenerateCelebrityId(cancellationToken);
        coreCelebrity.Created = DateTime.UtcNow;
        coreCelebrity.Updated = DateTime.UtcNow;

        var resultCelebrity = await _repository.CreateCelebrity(coreCelebrity, cancellationToken);

        return new CreateCelebrityRes { Celebrity = _mapper.Map<Celebrity>(resultCelebrity) };
    }

    public async Task<UpdateCelebrityRes> UpdateCelebrity(int id, Celebrity celebrity, CancellationToken cancellationToken)
    {
        var existingCelebrity = await _repository.GetCelebrity(id, cancellationToken);
        if(existingCelebrity is null)
        {
            return new UpdateCelebrityRes { ErrorCode = ErrorCode.CELEBRITY_NOT_FOUND };
        }

        var coreCelebrity = _mapper.Map<Core.Domain.Celebrity>(celebrity);
        coreCelebrity.Updated = DateTime.UtcNow;

        var result = await _repository.UpdateCelebrity(id, coreCelebrity, cancellationToken);
        return new UpdateCelebrityRes { Celebrity = _mapper.Map<Celebrity>(result) };
    }

    public async Task<IEnumerable<Celebrity>> GetCelebrities(CancellationToken cancellationToken)
    {
        var cache = _memoryCache.Get(cacheKey);
        if(cache == null)
        {
            await ResetCelebrities(cancellationToken);
        }

        var result = await _repository.GetCelebrities(cancellationToken);
        return _mapper.Map<IEnumerable<Celebrity>>(result);
    }

    public async Task DeleteCelebrity(int id, CancellationToken cancellationToken)
    {
        await _repository.DeleteCelebrity(id, cancellationToken);
    }

    public async Task<IEnumerable<Celebrity>> ResetCelebrities(CancellationToken cancellationToken)
    {
        await _repository.ClearCelebrities(cancellationToken);
        var celebrities = await _memoryCache.GetOrCreateAsync(cacheKey, async (entry) =>
        {
            return await _scraper.ScrapCelebrities(cancellationToken);
        });

        foreach(var celebrity in celebrities)
        {
            celebrity.Id = await _idGenerator.GenerateCelebrityId(cancellationToken);
        }

        var dbCelebrities = await _repository.CreateCelebrities(celebrities, cancellationToken);

        return _mapper.Map<IEnumerable<Celebrity>>(dbCelebrities);
    }
}
