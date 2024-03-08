using AutoMapper;
using Celebpretty.Api.Models.V1;
using Celebpretty.Api.Models.V1.CreateCelebrity;
using Celebpretty.Api.Models.V1.UpdateCelebrity;

namespace Celebpretty.Api.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Application.Main.Models.Celebrity, Celebrity>();
        CreateMap<CreateCelebrityReq, Application.Main.Models.Celebrity>();
        CreateMap<UpdateCelebrityReq, Application.Main.Models.Celebrity>();
    }
}
