using Shared.BoardObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Shared.GameMessages
{
    [XmlRoot(Namespace = "https://se2.mini.pw.edu.pl/17-results/")]
    public abstract class GameMessage : ILoggable
    {

        [XmlAttribute()]
        public string PlayerGuid { get; set; }

        [XmlAttribute()]
        public int GameId { get; set; }

        public abstract void Execute(BoardObjects.Board board);
        public abstract void CanExecute(BoardObjects.Board board);

        public abstract ActionLog ToLog(int playerId, PlayerInfo playeInfo);
    }
}
