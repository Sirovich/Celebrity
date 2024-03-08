using AutoMapper;
using Celebpretty.Api.Extensions;
using Celebpretty.Api.Models.V1;
using Celebpretty.Api.Models.V1.CreateCelebrity;
using Celebpretty.Api.Models.V1.UpdateCelebrity;
using Celebpretty.Application.Main;
using Microsoft.AspNetCore.Mvc;

namespace Celebpretty.Api.Controllers;

[Route("api/celebrities")]
[ApiController]
public class CelebritiesController : ControllerBase
{
    private readonly ICelebrityService _celebrityService;
    private readonly IMapper _mapper;

    public CelebritiesController(ICelebrityService celebrityService, IMapper mapper)
    {
        _celebrityService = celebrityService;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult<Celebrity>> CreateCelebrity(CreateCelebrityReq request, CancellationToken cancellationToken)
    {
        var celebrity = _mapper.Map<Application.Main.Models.Celebrity>(request);
        var result = await _celebrityService.CreateCelebrity(celebrity, cancellationToken);
        if (!result.IsSuccess)
        {
            return result.ToProblemDetails();
        }

        return _mapper.Map<Celebrity>(result.Celebrity);
    }

    [HttpPut]
    public async Task<ActionResult<Celebrity>> UpdateCelebrity(UpdateCelebrityReq request, CancellationToken cancellationToken)
    {
        var celebrity = _mapper.Map<Application.Main.Models.Celebrity>(request);
        var result = await _celebrityService.UpdateCelebrity(celebrity, cancellationToken);
        if (!result.IsSuccess)
        {
            return result.ToProblemDetails();
        }

        return _mapper.Map<Celebrity>(result.Celebrity);
    }


    [HttpDelete]
    public async Task<ActionResult<Celebrity>> DeleteCelebrity(string id, CancellationToken cancellationToken)
    {
        var result = await _celebrityService.UpdateCelebrity(celebrity, cancellationToken);
        if (!result.IsSuccess)
        {
            return result.ToProblemDetails();
        }

        return _mapper.Map<Celebrity>(result.Celebrity);
    }
}
