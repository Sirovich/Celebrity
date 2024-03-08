using Asp.Versioning;
using Celebpretty.Api.Models.V1.CreateCelebrity;
using Celebpretty.Api.Models.V1.UpdateCelebrity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Celebpretty.Api.Controllers;

[Route("api/v1/celebrities")]
[ApiVersion("1.0")]
[ApiController]
public class CelebritiesController : ControllerBase
{
    public CelebritiesController()
    {
        
    }

    [HttpPost]
    public Task CreateCelebrity(CreateCelebrityReq request)
    {
        return Task.CompletedTask;
    }

    [HttpPut]
    [Route("{id:int}")]
    public Task UpdateCelebrity(UpdateCelebrityReq request, [FromRoute] int id)
    {
        return Task.CompletedTask;
    }
}
