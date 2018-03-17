﻿using Shared.BoardObjects;
using Shared.ResponseMessages;
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
        public int PlayerId { get; set; }
        public string PlayerGuid { get; set; }

        [XmlAttribute()]
        public int GameId { get; set; }

        public abstract ResponseMessage Execute(BoardObjects.Board board);

        public abstract ActionLog ToLog(int playerId, PlayerInfo playeInfo);
    }
}