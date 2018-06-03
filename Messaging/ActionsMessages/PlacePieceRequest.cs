using System;
using System.Xml.Serialization;
using Common;
using Common.ActionInfo;

namespace Messaging.Requests
{
    [XmlType(XmlRootName)]
    public class PlacePieceRequest : RequestMessage
    {
        public const string XmlRootName = "PlacePiece";

        protected PlacePieceRequest()
        {
        }

        public PlacePieceRequest(Guid playerGuid, int gameId) : base(playerGuid, gameId)
        {
        }

        public override ActionInfo GetActionInfo()
        {
            return new PlaceActionInfo(PlayerGuid);
        }

        public override string ToLog()
        {
            return string.Join(',', ActionType.Place, base.ToLog());
        }
    }
}