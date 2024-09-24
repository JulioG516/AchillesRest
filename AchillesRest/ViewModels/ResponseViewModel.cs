using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AchillesRest.Helpers;
using AchillesRest.Services;
using ReactiveUI;
using Splat;

namespace AchillesRest.ViewModels;

public class ResponseViewModel : ViewModelBase
{
    public ResponseViewModel()
    {
        RequestService = Locator.Current.GetService<RequestService>()!;
        ClipboardCopyCommand = ReactiveCommand.CreateFromTask(ClipboardCopy);
    }

    public ICommand ClipboardCopyCommand { get; }

    private async Task ClipboardCopy()
    {
        if (RequestService.Response?.Content == null)
            return;

        await Interactions.SetClipboard.Handle(RequestService.Response.Content);
    }

    public RequestService RequestService { get; }
}