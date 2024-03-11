namespace Celebpretty.Application.Persistence;

public interface IScraper
{
    Task<IEnumerable<Core.Domain.Celebrity>> ScrapCelebrities(CancellationToken cancellationToken);
}
