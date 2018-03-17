using Shared.BoardObjects;
using Shared.GameMessages;
using System;
using System.Collections.Generic;
using System.Text;
using static Player.Strategy.PlayerStrategy;
using static Shared.CommonResources;
using Shared;

namespace Player.Strategy.StateTransition
{
    abstract class BaseTransition
    {
        protected Location location;
        protected TeamColour team;
        protected int playerId;
        public PlayerState ChangeState { get; set; }

        public BaseTransition(Location location, TeamColour team, int playerId)
        {
            this.location = location;
            this.team = team;
            this.playerId = playerId;
        }

        public abstract GameMessage ExecuteStrategy(Board board);
    }
}
