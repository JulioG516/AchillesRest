using AchillesRest.ViewModels;

namespace AchillesRest.Models;

public class KeyValueParam
{
    public string? Key { get; set; }
    public string? Value { get; set; }
    public bool Enabled { get; set; } = true;

    public KeyValueParam()
    {
    }

    public KeyValueParam(KeyValueParamViewModel keyValueParamViewModel)
    {
        Key = keyValueParamViewModel.Key;
        Value = keyValueParamViewModel.Value;
        Enabled = keyValueParamViewModel.Enabled;
    }
}