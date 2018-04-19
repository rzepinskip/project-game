using System;
using Common;
using Common.Interfaces;

namespace Messaging.InitialisationMessages
{
    public class RegisterGameMessage : IMessage
    {
        public RegisterGameMessage(GameInfo newGameInfo)
        {
            NewGameInfo = newGameInfo;
        }

        public GameInfo NewGameInfo { get; set; }
        public IMessage Process(IGameMaster gameMaster)
        {
            throw new NotImplementedException();
        }

        public bool Process(IPlayer player)
        {
            throw new NotImplementedException();
        }

        public void Process(ICommunicationServer cs, int id)
        {
            cs.RegisterNewGame(NewGameInfo, id);
            cs.Send(new ConfirmGameRegistrationMessage(id), id);
        }
    }
}
