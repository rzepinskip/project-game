using System.Collections.Generic;
using Common;
using Common.Interfaces;
using Messaging.InitializationMessages;
using PlayerStateCoordinator.Info;
using PlayerStateCoordinator.States;
using PlayerStateCoordinator.States.InitializationStates;

namespace PlayerStateCoordinator
{
    public class StateCoordinator
    {
        private readonly GameInitializationInfo _gameInitializationInfo;

        public StateCoordinator(string gameName, TeamColor preferredTeam, PlayerType preferredRole)
        {
            _gameInitializationInfo = new GameInitializationInfo(gameName, preferredTeam, preferredRole);
            CurrentState = new GetGamesState(_gameInitializationInfo);
        }

        public State CurrentState { get; private set; }

        public IMessage Start()
        {
            return new GetGamesMessage();
        }

        public IEnumerable<IMessage> Process(IMessage message)
        {
            var messagesToSend = new List<IMessage>();

            try
            {
                while (CurrentState.TransitionType == StateTransitionType.Triggered)
                {
                    var transition = CurrentState.Process(message);
                    CurrentState = transition.NextState;
                    messagesToSend.AddRange(transition.Message);
                }
            }
            catch (StrategyException strategyException)
            {
                CurrentState = _gameInitializationInfo.PlayerStrategyBeginningState;
                messagesToSend = new List<IMessage>();
            }

            return messagesToSend;
        }

        public void UpdatePlayerStrategyBeginningState(State state)
        {
            _gameInitializationInfo.PlayerStrategyBeginningState = state;
        }

        public void UpdateJoiningResult(bool joiningSuccessful)
        {
            _gameInitializationInfo.JoiningSuccessful = joiningSuccessful;
        }

        public void UpdateGamesInfo(IEnumerable<GameInfo> gamesInfo)
        {
            _gameInitializationInfo.GamesInfo = gamesInfo;
        }

        public void NotifyAboutGameEnd()
        {
            _gameInitializationInfo.IsGameRunning = false;
        }
    }
}