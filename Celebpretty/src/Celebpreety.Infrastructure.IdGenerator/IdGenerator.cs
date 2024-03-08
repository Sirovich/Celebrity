using Celebpretty.Application.Persistence;

namespace Celebpreety.Infrastructure.IdGenerator;

public class IdGenerator : IIdGenerator
{
    private readonly ISequencesRepository _sequencesRepository;
    private const string celebritySequence = "celebrity";

    public IdGenerator(ISequencesRepository sequencesRepository)
    {
        _sequencesRepository = sequencesRepository;
    }

    public async Task<int> GenerateCelebrityId(CancellationToken cancellationToken)
    {
        return await _sequencesRepository.SequenceInc(celebritySequence);
    }
}
