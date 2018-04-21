using System;
using System.Xml.Serialization;
using Common.Interfaces;
using Messaging.Responses;

namespace Messaging.InitialisationMessages
{
    /// <summary>
    /// GM's response to Player about join game denial
    /// </summary>
    [XmlType(XmlRootName)]
    public class RejectJoiningGame : Response
    {
        public const string XmlRootName = "RejectJoiningGame";

        protected RejectJoiningGame()
        {
        }

        public RejectJoiningGame(string gameName, int playerId)
        {
            GameName = gameName;
            PlayerId = playerId;
        }

        public string GameName { get; set; }

        public override IMessage Process(IGameMaster gameMaster)
        {
            throw new NotImplementedException();
        }

        public override bool Process(IPlayer player)
        {
            player.UpdateJoiningInfo(false);
            player.ChangePlayerCoordinatorState();
            return false;
        }

        public override void Process(ICommunicationServer cs, int id)
        {
            cs.Send(this, PlayerId);
        }
    }
}