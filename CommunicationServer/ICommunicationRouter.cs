using System.Collections.Generic;
using Common;
using Common.Interfaces;

namespace CommunicationServer
{
    public interface ICommunicationRouter : IGamesManager, IClientTypeManager
    {
        IEnumerable<int> GetAllClientsConnectedWithGame(int gameId);
        ClientType GetClientTypeFrom(int connectionId);
    }
}