using System.Collections.Generic;
using Common;
using Common.Interfaces;

namespace CommunicationServer
{
    public interface ICommunicationRouter : IGamesManager, IClientTypeManager
    {
        IEnumerable<int> GetAllPlayersInGame(int gameId);
        ClientType GetClientTypeFrom(int socketId);
    }
}