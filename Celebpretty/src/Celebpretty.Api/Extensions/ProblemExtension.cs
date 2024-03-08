using Celebpretty.Application.Main.Models.Error;
using Microsoft.AspNetCore.Mvc;

namespace Celebpretty.Api.Extensions;

public static class ProblemExtension
{
    public static ActionResult ToProblemDetails(this BaseResult result)
    {
        var problem = new ProblemDetails();
        switch (result.ErrorCode)
        {
            case ErrorCode.CELEBRITY_NOT_FOUND:
                problem.Status = 404;
                problem.Title = nameof(ErrorCode.CELEBRITY_NOT_FOUND);
                break;
        }

        return new ObjectResult(problem);
    }
}
