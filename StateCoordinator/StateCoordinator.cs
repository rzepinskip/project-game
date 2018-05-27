using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Common;
using Common.Interfaces;
using Messaging.InitializationMessages;
using PlayerStateCoordinator.Common;
using PlayerStateCoordinator.Common.States;
using PlayerStateCoordinator.GameInitialization;
using PlayerStateCoordinator.GameInitialization.States;
using PlayerStateCoordinator.GamePlay;

namespace PlayerStateCoordinator
{
    public class StateCoordinator
    {
        private readonly GameInitializationInfo _gameInitializationInfo;
        private Timer _stateTimeoutTimer;

        public StateCoordinator(string gameName, TeamColor preferredTeam, PlayerType preferredRole)
        {
            _gameInitializationInfo = new GameInitializationInfo(gameName, preferredTeam, preferredRole);
            CurrentState = new GetGamesState(_gameInitializationInfo);
        }

        public State CurrentState { get; set; }

        public IMessage Start()
        {
            _stateTimeoutTimer = new Timer(CheckForInactivity, null, TimeSpan.FromSeconds(0), Constants.DefaultStateTimeout / 2);
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
                    //Console.WriteLine($"{DateTime.Now} now but state's {CurrentState.EnteredTimestamp}");
                    //Console.WriteLine($"{CurrentState.GetType().Name} for {message.GetType().Name}\n\t{transition.NextState.GetType().Name} by {transition.GetType().Name}");
                    CurrentState = transition.NextState;
                    messagesToSend.AddRange(transition.Message);
                } while (CurrentState.TransitionType == StateTransitionType.Immediate);
            }
            catch (StrategyException strategyException)
            {
                Console.WriteLine(strategyException.PrintFull());
                ResetToInitState();
                return new List<IMessage>();
            }

            //foreach (var sendMessage in messagesToSend)
            //{
            //    Console.WriteLine(sendMessage.GetType().Name);
            //}
            //Console.WriteLine();
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
            StopTimers();
        }

        private void CheckForInactivity(object state)
        {
            if (DateTime.Now - CurrentState.EnteredTimestamp > Constants.DefaultStateTimeout)
            {
                Console.WriteLine($"Inactivity: {DateTime.Now} vs last state {CurrentState.EnteredTimestamp}");
                ResetToInitState();
            }
        }

        private void ResetToInitState()
        {
            if (CurrentState is GameInitializationState)
                CurrentState = _gameInitializationInfo.PlayerGameInitializationBeginningState;
            else if (CurrentState is GamePlayStrategyState)
                CurrentState = _gameInitializationInfo.PlayerGameStrategyBeginningState;

            Console.WriteLine($"Strategy error - resetting back to state {CurrentState.GetType().Name}");
        }

        public void StopTimers()
        {
            _stateTimeoutTimer.Dispose();
        }
    }
}