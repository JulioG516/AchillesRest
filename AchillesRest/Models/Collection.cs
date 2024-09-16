using System.Collections.Generic;

namespace AchillesRest.Models;

public class Collection
{
    public string? Name { get; set; }
    public List<Request>? Requests { get; set; }
}