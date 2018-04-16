using System;
using Common.Interfaces;

namespace Messaging.InitialisationMessages
{
    public class GetGamesMessage : IMessage
    {
        public IMessage Process(IGameMaster gameMaster)
        {
            throw new NotImplementedException();
        }

        public void Process(IPlayer player)
        {
            throw new NotImplementedException();
        }

        public void Process(ICommunicationServer cs, int id)
        {
            cs.Send(new RegisteredGamesMessage(cs.GetGames()), id);
        }
    }
}
