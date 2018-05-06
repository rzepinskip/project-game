using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Common;
using Common.ActionInfo;

namespace Messaging.Requests
{
    [XmlType(XmlRootName)]
    public class DestroyPieceRequest : Request
    {
        public const string XmlRootName = "DestroyPiece";

        protected DestroyPieceRequest() { }

        public DestroyPieceRequest(Guid playerGuid, int gameId) : base(playerGuid, gameId)
        {
        }

        public override ActionInfo GetActionInfo()
        {
            return new DestroyActionInfo(PlayerGuid);
        }

        public override string ToLog()
        {
            return String.Join(',', ActionType.Destroy, base.ToLog());
        }
    }
}
