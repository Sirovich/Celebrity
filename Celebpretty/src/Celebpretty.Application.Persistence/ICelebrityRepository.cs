namespace Celebpretty.Application.Persistence;

public interface ICelebrityRepository
{
    Task<Core.Domain.Celebrity> CreateCelebrity(Core.Domain.Celebrity celebrity, CancellationToken cancellationToken);
    Task<Core.Domain.Celebrity> UpdateCelebrity(int id, Core.Domain.Celebrity celebrity, CancellationToken cancellationToken);
    Task DeleteCelebrity(int id, CancellationToken cancellationToken);
    Task ClearCelebrities(CancellationToken cancellationToken);
    Task<IEnumerable<Core.Domain.Celebrity>> CreateCelebrities(IEnumerable<Core.Domain.Celebrity> celebrities, CancellationToken cancellationToken);
    Task<Core.Domain.Celebrity> GetCelebrity(int id, CancellationToken cancellationToken);
    Task<IEnumerable<Core.Domain.Celebrity>> GetCelebrities(CancellationToken cancellationToken);
}
