using System.Collections.Generic;
using AchillesRest.Models.Authentications;
using AchillesRest.Models.Enums;

namespace AchillesRest.Models;

public class Collection
{
    public string? Name { get; set; }
    public List<Request>? Requests { get; set; }

    public IAuthentication? Authentication { get; set; }
    public EnumAuthTypes? SelectedAuthType { get; set; } = EnumAuthTypes.None;

    public string? Description { get; set; }
}