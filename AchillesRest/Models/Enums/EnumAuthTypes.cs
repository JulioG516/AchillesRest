// ReSharper disable InconsistentNaming

using System.ComponentModel;

namespace AchillesRest.Models.Enums;

public enum EnumAuthTypes
{
    [Description("None")] None,
    [Description("Inherit from parent")] InheritFromParent,
    [Description("Basic")] Basic,
    [Description("Bearer")] Bearer,
    [Description("Digest")] Digest,
}