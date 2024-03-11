using AutoMapper;
using Celebpretty.Application.Persistence;
using Celebpretty.Infrastructure.Common;
using Celebpretty.Infrastructure.Scraper.Models;
using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Celebpretty.Infrastructure.Scraper;

public class Scraper : IScraper
{
    private readonly IMapper _mapper;
    private readonly ILogger _logger;
    private const string baseUrl = "https://www.imdb.com";

    public Scraper(IMapper mapper, ILogger<Scraper> logger)
    {
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<Core.Domain.Celebrity>> ScrapCelebrities(CancellationToken cancellationToken)
    {
        var web = new HtmlWeb();
        var document = await web.LoadFromWebAsync($"{baseUrl}/list/ls052283250/", cancellationToken);
        var celebritiesList = new List<Celebrity>();
        var celebritiesHtml = document.DocumentNode.QuerySelectorAll(".lister-item");

        int processed = 0;
        int batchSize = 6;
        while (processed < celebritiesHtml.Count)
        {
            if (celebritiesHtml.Count - processed < batchSize)
                batchSize = celebritiesHtml.Count - processed;

            var countdownEvent = new CountdownEvent(batchSize);
            foreach (var celebrityHtml in celebritiesHtml.Take(new Range(processed, processed + batchSize)))
            {
                ThreadPool.QueueUserWorkItem(async state =>
                {
                    var htmlNode = state;

                    var page = HtmlEntity.DeEntitize(htmlNode.QuerySelector(".lister-item-content h3 a").Attributes["href"].Value).Trim();
                    var name = HtmlEntity.DeEntitize(htmlNode.QuerySelector(".lister-item-content h3 a").GetDirectInnerText()).Trim();
                    _logger.LogDebug(name);
                    var personalPage = await web.LoadFromWebAsync($"{baseUrl}{page}", cancellationToken);
                    var birthDate = HtmlEntity.DeEntitize(personalPage.QuerySelector(".sc-dec7a8b-1 span:last-child").GetDirectInnerText()).Trim();
                    var image = htmlNode.QuerySelector(".lister-item-image a img").Attributes["src"].Value;

                    var role = HtmlEntity.DeEntitize(htmlNode.QuerySelector(".lister-item-content p").GetDirectInnerText()).Trim();
                    string gender;
                    switch (role)
                    {
                        case "Actor":
                            gender = "male";
                            break;
                        case "Actress":
                            gender = "female";
                            break;
                        default:
                            gender = GetGender(name, personalPage);
                            break;
                    }

                    lock (celebritiesList)
                    {
                        celebritiesList.Add(new Celebrity
                        {
                            Name = name,
                            Gender = gender,
                            Role = role,
                            Image = image,
                            BirthDate = DateTime.Parse(birthDate)
                        });
                    }

                    await Task.Delay(300);
                    countdownEvent.Signal();
                }, celebrityHtml, true);
            }

            countdownEvent.Wait();
            processed += batchSize;
        }
        return _mapper.Map<IEnumerable<Core.Domain.Celebrity>>(celebritiesList);
    }

    private string GetGender(string name, HtmlDocument page)
    {
        var roles = page.QuerySelectorAll(".sc-16a236a8-0 ul li");
        foreach(var role in roles)
        {
            if (role.GetDirectInnerText().Trim() == "Actor")
                return "male";

            if (role.GetDirectInnerText().Trim() == "Actress")
                return "female";
        }

        var firstName = name.Split(' ').FirstOrDefault();
        return firstName?.Last() != 'a' ? "male" : "female";
    }
}
