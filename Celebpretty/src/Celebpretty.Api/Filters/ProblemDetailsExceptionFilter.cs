using Microsoft.AspNetCore.Mvc.Filters;

namespace Celebpretty.Api.Filters;

public class ProblemDetailsExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        throw new NotImplementedException();
    }
}
