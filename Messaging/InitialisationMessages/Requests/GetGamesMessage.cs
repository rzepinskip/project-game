using System;
using System.Xml.Serialization;
using Common.Interfaces;

namespace Messaging.InitialisationMessages
{
    /// <summary>
    ///     Player's request to list all games handled by CS that can be joined(not in progress)
    /// </summary>
    [XmlType(XmlRootName)]
    public class GetGamesMessage : Message
    {
        public const string XmlRootName = "GetGames";

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
            cs.Send(new RegisteredGamesMessage(cs.GetAllJoinableGames()), id);
        }
    }
}