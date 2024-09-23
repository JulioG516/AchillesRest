using System.Net.Http;

namespace AchillesRest.Models.Authentications;

public interface IAuthentication
{
    void ApplyAuthentication(HttpRequestMessage requestMessage);
}

public class BasicAuthentication : IAuthentication
{
    public string? Username { get; set; }
    public string? Password { get; set; }

    public void ApplyAuthentication(HttpRequestMessage requestMessage)
    {
        if (Username == null || Password == null)
            return;

        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes($"{Username}:{Password}");
        var base64 = System.Convert.ToBase64String(plainTextBytes);
        requestMessage.Headers.Add("Authorization", $"Basic {base64}");
    }

    public override string ToString()
    {
        return $"Basic Authentication - Username: {Username}";
    }
}

public class BearerAuthentication : IAuthentication
{
    public string? Token { get; set; }

    public void ApplyAuthentication(HttpRequestMessage requestMessage)
    {
        if (Token == null)
            return;

        requestMessage.Headers.Add("Authorization", $"Bearer {Token}");
    }

    public override string ToString()
    {
        return $"Bearer Authentication - Token: {Token}";
    }
}

public class DigestAuthentication : IAuthentication
{
    public string? Username { get; set; }
    public string? Password { get; set; }

    public void ApplyAuthentication(HttpRequestMessage requestMessage)
    {
        throw new System.NotImplementedException();
    }

    public override string ToString()
    {
        return $"Digest Authentication - Username: {Username}";
    }
}