using Celebpretty.Application.Main.Models;

namespace Celebpretty.Application.Main;

public interface ICelebrityService
{
    Task<CreateCelebrityRes> CreateCelebrity(Celebrity celebrity, CancellationToken cancellationToken);
    Task<UpdateCelebrityRes> UpdateCelebrity(int id, Celebrity celebrity, CancellationToken cancellationToken);
    Task DeleteCelebrity(int id, CancellationToken cancellationToken);
    Task<IEnumerable<Celebrity>> GetCelebrities(CancellationToken cancellationToken);
    Task<IEnumerable<Celebrity>> ResetCelebrities(CancellationToken cancellationToken);
}
