using System;
using System.Collections.Generic;
using System.Text;
using Common;
using Common.Interfaces;

namespace Messaging.InitialisationMessages
{
    public class RegisteredGamesMessage : IMessage
    {
        public RegisteredGamesMessage(IEnumerable<GameInfo> games)
        {
            this.Games = games;
        }

        public IEnumerable<GameInfo> Games { get; set; }

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
