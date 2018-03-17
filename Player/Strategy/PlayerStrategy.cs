using Shared.BoardObjects;
using Shared.GameMessages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Player.Strategy
{
    class PlayerStrategy
    {
        public enum PlayerState
        {
            InitState,
            //etc
        }
        private PlayerState currentState;

        
        public void ChangeState(Board currentBoard)
        {

        }
        public GameMessage NextMove()
        {
            /*
            switch (currentState)
            {
                case state:
                    init message
                    return message
            }
            */
            throw new NotImplementedException();

        }
    }
}
