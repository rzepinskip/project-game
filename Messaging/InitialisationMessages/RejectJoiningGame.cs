using System;
using System.Xml.Serialization;
using Common.Interfaces;

namespace Messaging.InitialisationMessages
{
    [XmlType(XmlRootName)]
    public class RejectJoiningGame : IResponse
    {
        public const string XmlRootName = "RejectJoiningGame";

        public RejectJoiningGame() { }
        public RejectJoiningGame(string gameName, int playerId)
        {
            GameName = gameName;
            PlayerId = playerId;
        }
        public string GameName { get; set; }
        public int PlayerId { get; set; }
        public IMessage Process(IGameMaster gameMaster)
        {
            throw new NotImplementedException();
        }

        public bool Process(IPlayer player)
        {
            //handle reject join
            throw new Exception();
        }

        public void Process(ICommunicationServer cs, int id)
        {
            cs.Send(this, PlayerId);
        }
    }
}
