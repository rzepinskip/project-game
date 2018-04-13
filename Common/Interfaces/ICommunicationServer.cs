using System;
using System.Collections.Generic;

namespace Common.Interfaces
{
    public interface ICommunicationServer
    {
        void Send(IMessage message, int id);
        void SetupServer(Action<IMessage, int> messageHandler);
        IEnumerable<GameInfo> GetGames();
        int GetGameId(string gameName);
    }
}
