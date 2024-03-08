using AutoMapper;
using Celebpretty.Application.Main.Models;
using Celebpretty.Application.Persistence;

namespace Celebpretty.Application.Main;

public class CelebrityService : ICelebrityService
{
    private readonly ICelebrityRepository _repository;
    private readonly IMapper _mapper;

    public CelebrityService(ICelebrityRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CreateCelebrityRes> CreateCelebrity(Celebrity celebrity, CancellationToken cancellationToken)
    {
        var coreCelebrity = _mapper.Map<Core.Domain.Celebrity>(celebrity);
        coreCelebrity.Id = 
        coreCelebrity.Created = DateTime.UtcNow;
        coreCelebrity.Updated = DateTime.UtcNow;

        _repository
    }

    public async Task<UpdateCelebrityRes> UpdateCelebrity(int id, Celebrity celebrity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Celebrity>> GetCelebrities()
    {
        throw new NotImplementedException();
    }
}
