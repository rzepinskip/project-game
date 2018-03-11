using System;
using System.Collections.Generic;

namespace GameMaster.Configuration
{
    public class ActionCosts : IEquatable<ActionCosts>
    {
        public double MoveDelay { get; set; }
        public double DiscoverDelay { get; set; }
        public double TestDelay { get; set; }
        public double PickUpDelay { get; set; }
        public double PlacingDelay { get; set; }
        public double KnowledgeExchangeDelay { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as ActionCosts);
        }

        public bool Equals(ActionCosts other)
        {
            return other != null &&
                   MoveDelay == other.MoveDelay &&
                   DiscoverDelay == other.DiscoverDelay &&
                   TestDelay == other.TestDelay &&
                   PickUpDelay == other.PickUpDelay &&
                   PlacingDelay == other.PlacingDelay &&
                   KnowledgeExchangeDelay == other.KnowledgeExchangeDelay;
        }

        public override int GetHashCode()
        {
            var hashCode = -582915105;
            hashCode = hashCode * -1521134295 + MoveDelay.GetHashCode();
            hashCode = hashCode * -1521134295 + DiscoverDelay.GetHashCode();
            hashCode = hashCode * -1521134295 + TestDelay.GetHashCode();
            hashCode = hashCode * -1521134295 + PickUpDelay.GetHashCode();
            hashCode = hashCode * -1521134295 + PlacingDelay.GetHashCode();
            hashCode = hashCode * -1521134295 + KnowledgeExchangeDelay.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(ActionCosts costs1, ActionCosts costs2)
        {
            return EqualityComparer<ActionCosts>.Default.Equals(costs1, costs2);
        }

        public static bool operator !=(ActionCosts costs1, ActionCosts costs2)
        {
            return !(costs1 == costs2);
        }
    }
}
