using Celebpretty.Application.Main.Models.Error;

namespace Celebpretty.Application.Main.Models;

public class Celebrity
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Gender { get; set; }
    public string Role { get; set; }
    public DateTime BirthDate { get; set; }
    public string Image { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
}

public class CreateCelebrityRes : BaseResult
{
    public Celebrity Celebrity { get; init; }
}

public class UpdateCelebrityRes : BaseResult
{
    public Celebrity Celebrity { get; init; }
}

