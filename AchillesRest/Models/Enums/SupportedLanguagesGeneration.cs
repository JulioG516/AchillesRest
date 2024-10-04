using System.ComponentModel;

namespace AchillesRest.Models.Enums;

public enum SupportedLanguagesGeneration
{
    [Description("C#")] CSharp,
    [Description("Java")] Java,
    [Description("C# Rest Sharp")] CSharpRestSharp
}