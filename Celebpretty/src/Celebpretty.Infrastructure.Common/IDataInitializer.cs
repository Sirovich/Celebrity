namespace Celebpretty.Infrastructure.Common;

public interface IDataInitializer
{
    Task Init(CancellationToken cancellationToken);
}
