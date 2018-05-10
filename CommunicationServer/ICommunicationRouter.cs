using System.Collections.Generic;
using Common.Interfaces;

namespace CommunicationServer
{
    public interface ICommunicationRouter : IGamesManager
    {
        IEnumerable<int> GetAllPlayersInGame(int gameId);
    }
}