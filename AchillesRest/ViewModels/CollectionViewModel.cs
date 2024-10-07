using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using AchillesRest.Models;
using AchillesRest.Models.Authentications;
using AchillesRest.Models.Enums;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using DynamicData;
using DynamicData.Binding;
using LiteDB;
using ReactiveUI;

namespace AchillesRest.ViewModels;

// MVVM
public class CollectionViewModel : ViewModelBase
{
    private string? _name;

    public string? Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }

    private ObservableCollection<RequestViewModel>? _requests;

    public ObservableCollection<RequestViewModel>? Requests
    {
        get => _requests;
        set => this.RaiseAndSetIfChanged(ref _requests, value);
    }

    private IAuthentication? _authentication;

    public IAuthentication? Authentication
    {
        get => _authentication;
        set => this.RaiseAndSetIfChanged(ref _authentication, value);
    }

    private EnumAuthTypes? _selectedAuthType;

    public EnumAuthTypes? SelectedAuthType
    {
        get => _selectedAuthType;
        set => this.RaiseAndSetIfChanged(ref _selectedAuthType, value);
    }


    private string? _description;

    public string? Description
    {
        get => _description;
        set => this.RaiseAndSetIfChanged(ref _description, value);
    }

    private ObjectId? _id;

    public ObjectId? Id
    {
        get => _id;
        set => this.RaiseAndSetIfChanged(ref _id, value);
    }

    private bool _hasModifications = false;

    public bool HasModifications
    {
        get => _hasModifications;
        set => this.RaiseAndSetIfChanged(ref _hasModifications, value);
    }

    public CollectionViewModel(Collection collection)
    {
        Id = collection.Id ?? ObjectId.NewObjectId();

        Name = collection.Name;
        Requests = new ObservableCollection<RequestViewModel>(
            collection.Requests?.Select(r => new RequestViewModel(r)) ?? new List<RequestViewModel>()
        );
        SelectedAuthType = collection.SelectedAuthType;
        Authentication = collection.Authentication;
        Description = collection.Description;

        Requests
            .ToObservableChangeSet()
            .AutoRefreshOnObservable(request =>
                request.WhenAnyValue(x => x.Name,
                    x => x.Action,
                    x => x.Authentication,
                    x => x.Body,
                    x => x.Endpoint
                ))
            .Subscribe(_ => HasModifications = true);

        HasModifications = false;
    }

    private void SaveChanges()
    {
        HasModifications = false;
    }

    public override string ToString()
    {
        return
            $"Name: {Name}\nRequests Count: {Requests?.Count}\nAuthentication: {Authentication}\nDescription: {Description}";
    }
}