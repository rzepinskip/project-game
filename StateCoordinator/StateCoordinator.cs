using System;
using System.Collections.Generic;
using Common;
using Common.Interfaces;
using Messaging.InitializationMessages;
using PlayerStateCoordinator.Common;
using PlayerStateCoordinator.Common.States;
using PlayerStateCoordinator.GameInitialization;
using PlayerStateCoordinator.GameInitialization.States;
using PlayerStateCoordinator.Info;

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

        public State CurrentState { get; set; }

        public IMessage Start()
        {
            return new GetGamesMessage();
        }

        public IEnumerable<IMessage> Process(IMessage message)
        {
            var messagesToSend = new List<IMessage>();

            try
            {
                do
                {
                    var transition = CurrentState.Process(message);
                    //Console.WriteLine($"{transition.GetType().Name} for {message.GetType().Name}\n\t{transition.NextState.GetType().Name}\n");
                    CurrentState = transition.NextState;
                    //Console.WriteLine($"After transition state {CurrentState.GetType().Name}");
                    messagesToSend.AddRange(transition.Message);
                } while (CurrentState.TransitionType == StateTransitionType.Immediate);
            }
            catch (StrategyException strategyException)
            {
                Console.WriteLine(strategyException.PrintFull());
                CurrentState = _gameInitializationInfo.PlayerGameStrategyBeginningState;
                messagesToSend = new List<IMessage>();
            }

            return messagesToSend;
        }

        public void UpdatePlayerStrategyBeginningState(State state)
        {
            _gameInitializationInfo.PlayerGameStrategyBeginningState = state;
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