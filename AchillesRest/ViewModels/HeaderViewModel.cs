using AchillesRest.Models;
using ReactiveUI;

namespace AchillesRest.ViewModels;

public class HeaderViewModel : ViewModelBase
{
    private string? _key;

    public string? Key
    {
        get => _key;
        set => this.RaiseAndSetIfChanged(ref _key, value);
    }

    private string? _value;

    public string? Value
    {
        get => _value;
        set => this.RaiseAndSetIfChanged(ref _value, value);
    }

    private bool _enabled;

    public bool Enabled
    {
        get => _enabled;
        set => this.RaiseAndSetIfChanged(ref _enabled, value);
    }

    public HeaderViewModel()
    {
    }

    public HeaderViewModel(Header header)
    {
        Key = header.Key;
        Value = header.Value;
        Enabled = header.Enabled;
    }
}