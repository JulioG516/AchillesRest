using System;
using System.Collections.Generic;
using System.Diagnostics;
using AchillesRest.Models;
using LiteDB;

namespace AchillesRest.Services;

public class LiteDbService : IDbService
{
    private readonly Lazy<LiteDatabase> _lazyDb;
    private LiteDatabase Database => _lazyDb.Value;

    public LiteDbService()
    {
        var dbPath = AppContext.BaseDirectory + @"\achillesrest.db";

        var connectionString = $"Filename={dbPath}; Connection=Shared;";


        _lazyDb = new Lazy<LiteDatabase>(() => new LiteDatabase(connectionString));
    }


    public bool SaveCollection(Collection collection)
    {
        var col = Database.GetCollection<Collection>("Collections");
        var id = col.Insert(collection);

        return id != null;
    }

    public bool SaveCollection(List<Collection> collections)
    {
        var col = Database.GetCollection<Collection>("Collections");
        var id = col.Upsert(collections); // InsertBulk

        return id > 0;
    }

    public bool DeleteCollection(Collection collection)
    {
        var col = Database.GetCollection<Collection>("Collections");
        var result = col.Delete(collection.Id);
        return result;
    }

    public bool SaveRequest(Request request)
    {
        throw new System.NotImplementedException();
    }

    public bool SaveRequest(List<Request> requests)
    {
        throw new System.NotImplementedException();
    }

    public List<Collection> RetrieveCollections()
    {
        var col = Database.GetCollection<Collection>("Collections");
        return col.Query().ToList();
    }

    public List<Request> RetrieveRequests()
    {
        throw new System.NotImplementedException();
    }
}