using AutoMapper;
using Celebpretty.Infrastructure.Mongo.Models;

namespace Celebpretty.Infrastructure.Mongo.Configuration;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Core.Domain.Celebrity, Celebrity>().ReverseMap();
    }
}
