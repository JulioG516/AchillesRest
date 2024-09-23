using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using AchillesRest.Models.Enums;
using AchillesRest.Services;
using ReactiveUI;
using Splat;

namespace AchillesRest.ViewModels;

public class CollectionManagerViewModel : ViewModelBase
{
    public CollectionManagerViewModel()
    {
        Authentications =
            new ObservableCollection<EnumAuthTypes>(Enum.GetValues(typeof(EnumAuthTypes))
                .Cast<EnumAuthTypes>());
        Authentications.Remove(EnumAuthTypes.InheritFromParent);

        
        
        EditDescriptionMdCommand = ReactiveCommand.Create(EditDescriptionMd);
        ExitEditDescriptionMdCommand = ReactiveCommand.Create(ExitEditDescriptionMd);

        RequestService = Locator.Current.GetService<RequestService>()!;

        // When switched collection if it was in Edit Mode change to view mode
        this.WhenAnyValue(x => x.RequestService.SelectedCollection)
            .Subscribe(_ =>
            {
                if (InEditDescriptionMode)
                    InEditDescriptionMode = false;
            });

        // For Debug purposes
        // RequestService.WhenAnyValue(x => x.SelectedCollection)
        //     .Subscribe(x => { Debug.WriteLine($"Valor do RequestService collection: {x}"); });
    }

    public RequestService RequestService { get; }

    public ObservableCollection<EnumAuthTypes> Authentications { get; }
    
    private bool _inEditDescriptionMode;
    public bool InEditDescriptionMode
    {
        get => _inEditDescriptionMode;
        set => this.RaiseAndSetIfChanged(ref _inEditDescriptionMode, value);
    }

    public ICommand EditDescriptionMdCommand { get; }

    private void EditDescriptionMd()
    {
        InEditDescriptionMode = true;
    }

    public ICommand ExitEditDescriptionMdCommand { get; }

    private void ExitEditDescriptionMd()
    {
        InEditDescriptionMode = false;
    }
}