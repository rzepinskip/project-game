using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Messaging.ActionHelpers;

namespace GameMaster.Configuration
{
    [XmlRoot(ElementName = "GameMasterSettings", Namespace = "https://se2.mini.pw.edu.pl/17-pl-19/17-pl-19/")]
    public class GameConfiguration : IEquatable<GameConfiguration>
    {
        public GameDefinition GameDefinition { get; set; }
        public ActionCosts ActionCosts { get; set; }

        [XmlAttribute]
        public double KeepAliveInterval { get; set; }

        [XmlAttribute]
        public double RetryRegisterGameInterval { get; set; }

        public bool Equals(GameConfiguration other)
        {
            return other != null &&
                   GameDefinition == other.GameDefinition &&
                   ActionCosts == other.ActionCosts &&
                   KeepAliveInterval == other.KeepAliveInterval &&
                   RetryRegisterGameInterval == other.RetryRegisterGameInterval;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as GameConfiguration);
        }

        public override int GetHashCode()
        {
            var hashCode = -1819436856;
            hashCode = hashCode * -1521134295 + EqualityComparer<GameDefinition>.Default.GetHashCode(GameDefinition);
            hashCode = hashCode * -1521134295 + EqualityComparer<ActionCosts>.Default.GetHashCode(ActionCosts);
            hashCode = hashCode * -1521134295 + KeepAliveInterval.GetHashCode();
            hashCode = hashCode * -1521134295 + RetryRegisterGameInterval.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(GameConfiguration configuration1, GameConfiguration configuration2)
        {
            return EqualityComparer<GameConfiguration>.Default.Equals(configuration1, configuration2);
        }

        public static bool operator !=(GameConfiguration configuration1, GameConfiguration configuration2)
        {
            return !(configuration1 == configuration2);
        }
    }
}