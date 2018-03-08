using System;
using System.Collections.Generic;
using System.Text;
using Shared;
using Shared.Board;
namespace Player
{
    class Player : PlayerBase
    {
        private string PlayerGuid { get; set; }
        private Board Board { get; set; }
        private List<PlayerBase> Players { get; set; }
        private Field OccupiedField { get; set; }
    }
}
