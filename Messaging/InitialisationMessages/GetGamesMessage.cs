using System;
using System.Xml.Serialization;
using Common.Interfaces;

namespace Messaging.InitialisationMessages
{
    [XmlType(XmlRootName)]
    public class GetGamesMessage : IMessage
    {
        public const string XmlRootName = "GetGames";

        public GetGamesMessage() { }

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
            cs.Send(new RegisteredGamesMessage(cs.GetGames()), id);
        }
    }
}
