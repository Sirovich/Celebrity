using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celebpretty.Application.Persistence;
public interface IIdGenerator
{
    Task<int> GenerateCelebrityId(CancellationToken cancellationToken);
}
