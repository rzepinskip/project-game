namespace Common
{
    public class BoardInfo
    {
        public int Width { get; set; }
        public int TasksHeight { get; set; }
        public int GoalsHeight { get; set; }

        public BoardInfo()
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
