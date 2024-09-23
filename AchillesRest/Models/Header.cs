namespace AchillesRest.Models;

public class Header
{
    public string? Key { get; set; }
    public string? Value { get; set; }
    public bool Enabled { get; set; } = true;
}