using System.Collections.ObjectModel;
using AchillesRest.Models.Enums;

namespace AchillesRest.Models;

public class Request
{
    public string? Name { get; set; }
    public EnumActions? Action { get; set; }
    public string? Endpoint { get; set; }
}