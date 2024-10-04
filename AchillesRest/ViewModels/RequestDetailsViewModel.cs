using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AchillesRest.Helpers;
using AchillesRest.Models.Enums;
using AchillesRest.Services;
using ReactiveUI;
using Splat;

namespace AchillesRest.ViewModels;

public class RequestDetailsViewModel : ViewModelBase
{
    private readonly Dictionary<RequestViewModel, int?> _requests;

    public RequestDetailsViewModel()
    {
        SupportedLanguagesGenerations =
            new ObservableCollection<SupportedLanguagesGeneration>(Enum.GetValues(typeof(SupportedLanguagesGeneration))
                .Cast<SupportedLanguagesGeneration>());

        RequestService = Locator.Current.GetService<RequestService>()!;
        _requests = new Dictionary<RequestViewModel, int?>();

        AddHeaderCommand = ReactiveCommand.Create(AddHeader);
        DeleteHeaderCommand = ReactiveCommand.Create<KeyValueParamViewModel>(DeleteHeader);

        AddQueryParamCommand = ReactiveCommand.Create(AddQueryParam);
        DeleteQueryParamCommand = ReactiveCommand.Create<KeyValueParamViewModel>(DeleteQueryParam);

        CopyGeneratedCodeCommand = ReactiveCommand.CreateFromTask(CopyGeneratedCode);

        Authentications =
            new ObservableCollection<EnumAuthTypes>(Enum.GetValues(typeof(EnumAuthTypes)).Cast<EnumAuthTypes>());

        // this.WhenAnyValue(x => x.RequestService.SelectedRequest)
        //     .WhereNotNull()
        //     .DistinctUntilChanged()
        //     .Subscribe(_ => { SelectedTab = _requests!.GetValueOrDefault(RequestService.SelectedRequest, 0); });


        this.WhenAnyValue(x => x.RequestService.SelectedRequest)
            .WhereNotNull()
            .DistinctUntilChanged()
            .Subscribe(request =>
            {
                SelectedTab = _requests!.GetValueOrDefault(request, 0);

                // Subscribe to Action and Endpoint changes
                request.WhenAnyValue(r => r.Action, r => r.Endpoint, r => r.SelectedLanguageGeneration)
                    .CombineLatest(this.WhenAnyValue(x => x.SelectedTab))
                    .Where(tuple => tuple.Second == 4) // Only when SelectedTab is 4
                    .Subscribe(_ => request.UpdateGeneratedCode());
            });


        // TODO: Combobox to choose in body tab between Text, JavaScript, Json, HTML, XML
        // TODO: After change the Content-Type. to Application/{type}

        

        // this.WhenAnyValue(x => x.SelectedTab)
        //     .DistinctUntilChanged()
        //     .Where(index => index == 4)
        //     .Subscribe(_ => RequestService.SelectedRequest?.UpdateGeneratedCode());
    }

    public ObservableCollection<SupportedLanguagesGeneration> SupportedLanguagesGenerations { get; set; }


    public RequestService RequestService { get; }

    public ICommand AddHeaderCommand { get; }

    private void AddHeader()
    {
        RequestService.AddHeader();
    }

    public ICommand DeleteHeaderCommand { get; }

    private void DeleteHeader(KeyValueParamViewModel header)
    {
        RequestService.DeleteHeader(header);
    }

    public ICommand AddQueryParamCommand { get; }

    private void AddQueryParam()
    {
        RequestService.AddQueryParam();
    }

    public ICommand DeleteQueryParamCommand { get; }

    private void DeleteQueryParam(KeyValueParamViewModel queryParam)
    {
        RequestService.DeleteQueryParam(queryParam);
    }

    public ICommand CopyGeneratedCodeCommand { get; }

    private async Task CopyGeneratedCode()
    {
        if (RequestService.SelectedRequest != null && RequestService.SelectedRequest.GeneratedCode != null)
        {
            await Interactions.SetClipboard.Handle(RequestService.SelectedRequest.GeneratedCode);
        }
    }

    public ObservableCollection<EnumAuthTypes> Authentications { get; }

    private int? _selectedTab;

    public int? SelectedTab
    {
        get => _selectedTab;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedTab, value);

            _requests[RequestService.SelectedRequest!] = SelectedTab;
        }
    }
}