using AutoMapper;
using Celebpretty.Application.Main.Models;

namespace Celebpretty.Application.Main.Extensions;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Core.Domain.Celebrity, Celebrity>().ReverseMap();
    }
}
