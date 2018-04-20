using System;
using System.Xml.Serialization;
using Common.Interfaces;

namespace Messaging.InitialisationMessages
{
    [XmlType(XmlRootName)]
    public class GetGamesMessage : Message
    {
        public const string XmlRootName = "GetGames";

        public GetGamesMessage() { }

        public override IMessage Process(IGameMaster gameMaster)
        {
            throw new NotImplementedException();
        }

        public override bool Process(IPlayer player)
        {
            throw new NotImplementedException();
        }

        public override void Process(ICommunicationServer cs, int id)
        {
            cs.Send(new RegisteredGamesMessage(cs.GetGames()), id);
        }
    }
}
