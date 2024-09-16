using System;
using ReactiveUI;
using Splat;

namespace AchillesRest;

public class ReactiveViewLocator : IViewLocator
{
    public IViewFor? ResolveView<T>(T? viewModel, string? contract = null)
    {
        if (viewModel is null)
            return null;

        var viewModelName = viewModel.GetType().FullName;
        var viewTypeName = viewModel.GetType().FullName!.Replace("ViewModel", "View", StringComparison.Ordinal);
        var type = Type.GetType(viewTypeName);


        try
        {
            if (type == null)
            {
                this.Log().Error($"Could not find the view {viewTypeName} for view model {viewModelName}.");
                return null;
            }

            return Activator.CreateInstance(type)! as IViewFor;
        }
        catch (Exception)
        {
            this.Log().Error($"Could not instatiate {viewTypeName}");
            throw;
        }
    }
}