using System;
using System.Text;
using PlayerStateCoordinator.Info;
using PlayerStateCoordinator.States;

namespace PlayerStateCoordinator
{
    public class StrategyException : Exception
    {
        private readonly State _state;

        public StrategyException(State state, string message) : base(message)
        {
            _state = state;
        }

        public StrategyException(State state, string message, Exception innerException) : base(message, innerException)
        {
            _state = state;
        }

        public string PrintFull()
        {
            var strategyErrorBuilder = new StringBuilder();

            strategyErrorBuilder.AppendLine($"Strategy error occurred in {_state}: {Message}");
            strategyErrorBuilder.Append("\t");

            if (_state.Info is GameInitializationInfo initInfo)
                strategyErrorBuilder.AppendLine(
                    $"Join: {initInfo.JoiningSuccessful}, GameRunning: {initInfo.IsGameRunning}");

            return strategyErrorBuilder.ToString();
        }
    }
}