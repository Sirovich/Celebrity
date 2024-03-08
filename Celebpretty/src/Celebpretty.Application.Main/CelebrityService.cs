using AutoMapper;
using Celebpretty.Application.Main.Models;
using Celebpretty.Application.Main.Models.Error;
using Celebpretty.Application.Persistence;

namespace Celebpretty.Application.Main;

public class CelebrityService : ICelebrityService
{
    private readonly ICelebrityRepository _repository;
    private readonly IMapper _mapper;
    private readonly IIdGenerator _idGenerator;

    public CelebrityService(ICelebrityRepository repository, IMapper mapper, IIdGenerator idGenerator)
    {
        _repository = repository;
        _mapper = mapper;
        _idGenerator = idGenerator;
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
        var result = await _repository.GetCelebrities(cancellationToken);
        return _mapper.Map<IEnumerable<Celebrity>>(result);
    }
}
