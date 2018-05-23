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
        private readonly List<State> _lastStates = new List<State>();

        public StateCoordinator(string gameName, TeamColor preferredTeam, PlayerType preferredRole)
        {
            _gameInitializationInfo = new GameInitializationInfo(gameName, preferredTeam, preferredRole);
            CurrentState = new GetGamesState(_gameInitializationInfo);
        }

        private State _currentState;
        public State CurrentState
        {
            get => _currentState;
            set
            {
                _lastStates.Add(value);
                _currentState = value;
            }
        }

        public IMessage Start()
        {
            _stateTimeoutTimer = new Timer(CheckForInactivity, null, TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(30));
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
                ResetToInitState();
                return new List<IMessage>();
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
            _stateTimeoutTimer.Dispose();
        }

        private void CheckForInactivity(object state)
        {
            const int checkedItemsCount = Constants.DefaultLastStatesChecked;
            var lastXStates = _lastStates.TakeLast(checkedItemsCount).ToList();

            if (_lastStates.Count >= checkedItemsCount && lastXStates.TrueForAll(i => i.Equals(lastXStates.FirstOrDefault())) ||
                DateTime.Now - CurrentState.EnteredTimestamp > Constants.DefaultStateTimeout)
            {
                ResetToInitState();
            }
        }

        private void ResetToInitState()
        {
            if (CurrentState is GameInitializationState)
                CurrentState = _gameInitializationInfo.PlayerGameInitializationBeginningState;
            else if (CurrentState is GamePlayStrategyState)
                CurrentState = _gameInitializationInfo.PlayerGameStrategyBeginningState;

            _lastStates.Clear();

            Console.WriteLine($"Strategy error - resetting back to state {CurrentState.GetType().Name}");
        }
    }
}