using AutoMapper;
using Celebpretty.Infrastructure.Scraper.Models;

namespace Celebpretty.Infrastructure.Scraper.Configuration;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Core.Domain.Celebrity, Celebrity>().ReverseMap();
    }
}
