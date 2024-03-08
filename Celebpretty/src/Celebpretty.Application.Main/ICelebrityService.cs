using Celebpretty.Application.Main.Models;

namespace Celebpretty.Application.Main;

public interface ICelebrityService
{
    Task<CreateCelebrityRes> CreateCelebrity(Celebrity celebrity, CancellationToken cancellationToken);
    Task<UpdateCelebrityRes> UpdateCelebrity(Celebrity celebrity, CancellationToken cancellationToken);
    Task<IEnumerable<Celebrity>> GetCelebrities(CancellationToken cancellationToken);
}
