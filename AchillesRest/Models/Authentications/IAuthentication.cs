using System.Net.Http;

namespace AchillesRest.Models.Authentications;

public interface IAuthentication
{
    void ApplyAuthentication(HttpClient client);
}

public class BasicAuthentication : IAuthentication
{
    public string? Username { get; set; }
    public string? Password { get; set; }

    public void ApplyAuthentication(HttpClient client)
    {
        throw new System.NotImplementedException();
    }
    
    public override string ToString()
    {
        return $"Basic Authentication - Username: {Username}";
    }
}

public class BearerAuthentication : IAuthentication
{
    public string? Token { get; set; }

    public void ApplyAuthentication(HttpClient client)
    {
        throw new System.NotImplementedException();
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

    public void ApplyAuthentication(HttpClient client)
    {
        throw new System.NotImplementedException();
    }
    
        public override string ToString()
        {
            return $"Digest Authentication - Username: {Username}";
        }
}