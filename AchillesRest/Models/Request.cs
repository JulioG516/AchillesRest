using AchillesRest.Models.Authentications;
using AchillesRest.Models.Enums;

namespace AchillesRest.Models;

public class Request
{
    public string? Name { get; set; }
    public EnumActions? Action { get; set; }
    public string? Endpoint { get; set; }
    public EnumAuthTypes? SelectedAuthType { get; set; } = EnumAuthTypes.None;
    public IAuthentication? Authentication { get; set; }

    // TODO:  Auth, Headers, Params
}