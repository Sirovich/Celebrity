﻿namespace Celebpretty.Infrastructure.Mongo.Models;

public class Celebrity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Gender { get; set; }
    public string Role { get; set; }
    public DateTime BirthDate { get; set; }
    public string Image { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
    public bool Deleted { get; set; }
}
