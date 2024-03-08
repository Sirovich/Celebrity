using Celebpretty.Infrastructure.Common;
using Extensions.Hosting.AsyncInitialization;

namespace Celebpretty.Api.Extensions;

public class DataAsyncInitializer : IAsyncInitializer
{
    private readonly IEnumerable<IDataInitializer> _dataInitializers;
    public DataAsyncInitializer(IEnumerable<IDataInitializer> dataInitializers)
    {
        _dataInitializers = dataInitializers;
    }

    public Task InitializeAsync(CancellationToken cancellationToken)
    {
        return Task.WhenAll(_dataInitializers.Select(d => d.Init(cancellationToken)));
    }
}
