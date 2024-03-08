namespace Celebpretty.Infrastructure.Mongo.Models;

public class Celebrity
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string Gender { get; init; }
    public string Role { get; init; }
    public DateTime BirthDate { get; init; }
    public string Image { get; init; }
    public DateTime Created { get; init; }
    public DateTime Updated { get; init; }
    public bool Deleted { get; init; }
}
