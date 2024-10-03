using System.Reactive;
using ReactiveUI;

namespace AchillesRest.Helpers;

public static class Interactions
{
    // Used in ResponseViewModel to Copy the result response.
    
    
    public static readonly Interaction<string, Unit>
        SetClipboard = new Interaction<string, Unit>();
}