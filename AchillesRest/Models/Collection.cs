﻿using System.Collections.Generic;
using System.Linq;
using AchillesRest.Models.Authentications;
using AchillesRest.Models.Enums;
using AchillesRest.ViewModels;
using LiteDB;

namespace AchillesRest.Models;

public class Collection
{
    public ObjectId? Id { get; set; }

    public string? Name { get; set; }
    public List<Request>? Requests { get; set; }

    public IAuthentication? Authentication { get; set; }
    public EnumAuthTypes? SelectedAuthType { get; set; } = EnumAuthTypes.None;

    public string? Description { get; set; }

    public Collection()
    {
    }

    public Collection(CollectionViewModel collectionViewModel)
    {
        Id = collectionViewModel.Id;
        Name = collectionViewModel.Name;
        Requests = collectionViewModel.Requests?.Select(r => new Request(r)).ToList();
        Authentication = collectionViewModel.Authentication;
        SelectedAuthType = collectionViewModel.SelectedAuthType;
        Description = collectionViewModel.Description;
    }
}