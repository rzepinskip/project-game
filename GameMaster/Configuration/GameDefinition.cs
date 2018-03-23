using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Common.BoardObjects;

namespace GameMaster.Configuration
{
    public class GameDefinition : GameDefinitionBase, IEquatable<GameDefinition>
    {

        public double ShamProbability { get; set; }

        public double PlacingNewPiecesFrequency { get; set; }

        public string GameName { get; set; }

        public int InitialNumberOfPieces { get; set; }

        public int NumberOfPlayersPerTeam { get; set; }

        public bool Equals(GameDefinition other)
        {
            if (Goals.Count != other.Goals.Count)
                return false;

            for (var i = 0; i < Goals.Count; ++i)
                if (!Goals[i].Equals(other.Goals[i]))
                    return false;

            return other != null &&
                   ShamProbability == other.ShamProbability &&
                   PlacingNewPiecesFrequency == other.PlacingNewPiecesFrequency &&
                   InitialNumberOfPieces == other.InitialNumberOfPieces &&
                   BoardWidth == other.BoardWidth &&
                   TaskAreaLength == other.TaskAreaLength &&
                   GoalAreaLength == other.GoalAreaLength &&
                   NumberOfPlayersPerTeam == other.NumberOfPlayersPerTeam &&
                   GameName == other.GameName;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as GameDefinition);
        }

        public override int GetHashCode()
        {
            var hashCode = 1843543038;
            hashCode = hashCode * -1521134295 + EqualityComparer<List<GoalField>>.Default.GetHashCode(Goals);
            hashCode = hashCode * -1521134295 + ShamProbability.GetHashCode();
            hashCode = hashCode * -1521134295 + PlacingNewPiecesFrequency.GetHashCode();
            hashCode = hashCode * -1521134295 + InitialNumberOfPieces.GetHashCode();
            hashCode = hashCode * -1521134295 + BoardWidth.GetHashCode();
            hashCode = hashCode * -1521134295 + TaskAreaLength.GetHashCode();
            hashCode = hashCode * -1521134295 + GoalAreaLength.GetHashCode();
            hashCode = hashCode * -1521134295 + NumberOfPlayersPerTeam.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(GameName);
            return hashCode;
        }

        public static bool operator ==(GameDefinition definition1, GameDefinition definition2)
        {
            return EqualityComparer<GameDefinition>.Default.Equals(definition1, definition2);
        }

        public static bool operator !=(GameDefinition definition1, GameDefinition definition2)
        {
            return !(definition1 == definition2);
        }
    }
}