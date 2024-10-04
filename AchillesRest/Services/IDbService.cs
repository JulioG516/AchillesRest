using System.Collections.Generic;
using AchillesRest.Models;

namespace AchillesRest.Services;

public interface IDbService
{
    bool SaveCollection(Collection collection);
    bool SaveCollection(List<Collection> collections);

    bool SaveRequest(Request request);
    bool SaveRequest(List<Request> requests);

    List<Collection> RetrieveCollections();
    List<Request> RetrieveRequests();
}