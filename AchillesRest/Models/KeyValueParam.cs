namespace AchillesRest.Models;

public class KeyValueParam
{
    public string? Key { get; set; }
    public string? Value { get; set; }
    public bool Enabled { get; set; } = true;
}