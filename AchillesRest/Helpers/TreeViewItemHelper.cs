using Avalonia;
using Avalonia.Controls;

namespace AchillesRest.Helpers;

public static class TreeViewItemHelper
{
    public static readonly AttachedProperty<bool> HasColNameProperty =
        AvaloniaProperty.RegisterAttached<TreeViewItem, TreeViewItem, bool>("HasColName");

    public static bool GetHasColName(TreeViewItem element) => element.GetValue(HasColNameProperty);
    public static void SetHasColName(TreeViewItem element, bool value) => element.SetValue(HasColNameProperty, value);
}