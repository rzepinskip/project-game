using System;
using System.Xml.Serialization;
using Common;
using Common.Interfaces;

namespace Messaging.InitialisationMessages
{
    /// <summary>
    /// GM's request to register game
    /// </summary>
    [XmlType(XmlRootName)]
    public class RegisterGameMessage : Message
    {
        public const string XmlRootName = "RegisterGame";

        protected RegisterGameMessage()
        {
        }

        public RegisterGameMessage(GameInfo newGameInfo)
        {
            NewGameInfo = newGameInfo;
        }

        public GameInfo NewGameInfo { get; set; }

        public override IMessage Process(IGameMaster gameMaster)
        {
            throw new NotImplementedException();
        }

        public override void Process(IPlayer player)
        {
            throw new NotImplementedException();
        }

        public override void Process(ICommunicationServer cs, int id)
        {
            cs.MarkClientAsGameMaster(id);
            cs.RegisterNewGame(NewGameInfo, id);
            cs.Send(new ConfirmGameRegistrationMessage(id), id);
        }
    }
}