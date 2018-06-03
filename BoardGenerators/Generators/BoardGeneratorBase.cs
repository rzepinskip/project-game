using System.Collections.Generic;
using System.IO;
using Common;
using Common.BoardObjects;
using Common.Interfaces;

namespace BoardGenerators.Generators
{
    public abstract class BoardGeneratorBase<TBoard> where TBoard : IBoard
    {
        protected abstract TBoard Board { get; set; }


        protected virtual void PlaceGoals(IEnumerable<GoalField> goals)
        {
            foreach (var goal in goals)
            {
                if (!(Board[goal] is GoalField))
                    throw new InvalidDataException();
                goal.PlayerId = null;
                Board[goal] = goal;
            }
        }

        protected virtual void PlacePlayers(IEnumerable<(PlayerBase player, Location location)> playersWithLocations)
        {
            Board.Players.Clear();
            foreach (var (player, location) in playersWithLocations)
            {
                var playerInfo = new PlayerInfo(player, location);
                Board.Players.Add(playerInfo.Id, playerInfo);
                Board[playerInfo.Location].PlayerId = playerInfo.Id;
            }
        }
    }
}