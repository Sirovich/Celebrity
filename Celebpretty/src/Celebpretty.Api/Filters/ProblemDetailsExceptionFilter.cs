using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Diagnostics;

namespace Celebpretty.Api.Filters;

public class ProblemDetailsExceptionFilter : IExceptionFilter
{
    private readonly ProblemDetailsFactory _problemDetailsFactory;
    private readonly ILogger<ProblemDetailsExceptionFilter> _logger;

    public ProblemDetailsExceptionFilter(ProblemDetailsFactory problemDetailsFactory, ILogger<ProblemDetailsExceptionFilter> logger)
    {
        _problemDetailsFactory = problemDetailsFactory;
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        _logger.LogError(context.Exception, "Unhandled exception");

        var problemDetails = _problemDetailsFactory.CreateProblemDetails(context.HttpContext, 500, "Internal error",
            detail: "");

        context.Result = new ObjectResult(problemDetails);
        context.ExceptionHandled = true;
    }
}
