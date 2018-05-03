using System.Xml.Serialization;

namespace Common
{
    public class BoardInfo
    {
        [XmlAttribute("width")] public int Width { get; set; }
        [XmlAttribute("tasksHeight")] public int TasksHeight { get; set; }
        [XmlAttribute("goalsHeight")] public int GoalsHeight { get; set; }

        protected BoardInfo()
        {
        }

        public BoardInfo(int width, int tasksHeight, int goalsHeight)
        {
            Width = width;
            TasksHeight = tasksHeight;
            GoalsHeight = goalsHeight;
        }
    }
}
