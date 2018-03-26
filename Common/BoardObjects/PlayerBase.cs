using System.Xml.Serialization;

namespace Common
{
    public class PlayerBase
    {
        public int Id { get; set; }
        public PlayerType Type { get; set; }
        public TeamColor Team { get; set; }
    }
}