using System;
using System.Xml.Serialization;
using Common.Interfaces;

namespace Messaging.InitialisationMessages
{
    /// <summary>
    ///     GM's response to Player about join game denial
    /// </summary>
    [XmlType(XmlRootName)]
    public class RejectJoiningGame : MessageToPlayer
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

        [XmlAttribute("gameName")] public string GameName { get; set; }

        public override IMessage Process(IGameMaster gameMaster)
        {
            throw new NotImplementedException();
        }

        public override void Process(IPlayer player)
        {
            player.UpdateJoiningInfo(false);
        }

        public override void Process(ICommunicationServer cs, int id)
        {
            cs.Send(this, PlayerId);
        }

        public override string ToLog()
        {
            return string.Join(',', XmlRootName, base.ToLog());
        }
    }
}