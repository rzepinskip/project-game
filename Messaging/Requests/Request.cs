using System.Xml.Serialization;
using Common;
using Common.Interfaces;
using Messaging.ActionHelpers;
using Messaging.Responses;

namespace Messaging.Requests
{
    [XmlRoot(Namespace = "https://se2.mini.pw.edu.pl/17-results/")]
    public abstract class Request : ILoggable, IDelayed
    {
        // TODO Guid and GameId handling
        protected Request(int playerId)
        {
            PlayerId = playerId;
        }

        [XmlAttribute]
        public int PlayerId { get; }

        public string PlayerGuid { get; }

        [XmlAttribute]
        public int GameId { get; }

        public abstract double GetDelay(ActionCosts actionCosts);


        public abstract Response Execute(IGameMasterBoard board);

        public virtual string ToLog()
        {
            return string.Join(',', PlayerId, PlayerGuid, GameId);
        }
    }
}