using Asp.Versioning;
using AutoMapper;
using Celebpretty.Api.Extensions;
using Celebpretty.Api.Models.V1;
using Celebpretty.Api.Models.V1.CreateCelebrity;
using Celebpretty.Api.Models.V1.UpdateCelebrity;
using Celebpretty.Application.Main;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Celebpretty.Api.Controllers.V1;

[Route("api/v1/celebrities")]
[ApiVersion("1.0")]
[ApiController]
[AllowAnonymous]
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

        return _mapper.Map<Celebrity>(result);
    }

    [HttpPut]
    [Route("{id:int}")]
    public async Task<ActionResult<Celebrity>> UpdateCelebrity([FromRoute] int id, UpdateCelebrityReq request, CancellationToken cancellationToken)
    {
        var celebrity = _mapper.Map<Application.Main.Models.Celebrity>(request);
        var result = await _celebrityService.UpdateCelebrity(id, celebrity, cancellationToken);
        if (!result.IsSuccess)
        {
            return result.ToProblemDetails();
        }

        return _mapper.Map<Celebrity>(result);
    }
}
