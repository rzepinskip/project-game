using System;
using System.Collections.Generic;
using System.Text;
using Common.GameInfo;
using Common.Interfaces;

namespace Messaging.InitialisationMessages
{
    public class RegisterGameMessage : IMessage
    {
        public RegisterGameMessage(GameInfo newGameInfo)
        {
            this.NewGameInfo = newGameInfo;
        }

        public GameInfo NewGameInfo { get; set; }
        public IMessage Process(IGameMaster gameMaster)
        {
            throw new NotImplementedException();
        }

        public void Process(IPlayer player)
        {
            throw new NotImplementedException();
        }
    }
}
