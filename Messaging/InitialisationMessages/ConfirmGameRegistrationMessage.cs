using System;
using Common.Interfaces;

namespace Messaging.InitialisationMessages
{
    public class ConfirmGameRegistrationMessage : IMessage
    {
        public ConfirmGameRegistrationMessage(int gameId)
        {
            GameId = gameId;
        }

        public int GameId { get; set; }
        public IMessage Process(IGameMaster gameMaster)
        {
            //add new handler for confirm game registration
            throw new NotImplementedException();
        }

        public void Process(IPlayer player)
        {
            throw new NotImplementedException();
        }

        public void Process(ICommunicationServer cs, int id)
        {
            throw new NotImplementedException();
        }
    }
}
