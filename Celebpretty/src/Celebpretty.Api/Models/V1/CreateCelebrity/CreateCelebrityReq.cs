namespace Celebpretty.Api.Models.V1.CreateCelebrity;

public class CreateCelebrityReq
{
    public string Name { get; set; }
    public string Gender { get; set; }
    public string Role { get; set; }
    public DateTime BirthDate { get; set; }
    public string Image { get; set; }
}
