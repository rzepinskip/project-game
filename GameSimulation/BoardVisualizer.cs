using Shared.BoardObjects;
using System;
using System.Text;

namespace GameSimulation
{
    class BoardVisualizer
    {
        public void VisualizeBoard(Board board)
        {
            Console.Clear();
            for (int i = board.Height - 1; i >= 0; i--)
            {
                for (int j = 0; j < board.Width; j++)
                {
                    var output = new StringBuilder();

                    var field = board.Content[j, i];
                    if (field.PlayerId != null)
                    {
                        var player = board.Players[field.PlayerId.Value];
                        if (player.Team == Shared.CommonResources.TeamColour.Red)
                            output.Append("R");
                        else
                            output.Append("B");

                        if (player.Piece != null)
                            output.Append("p");
                        else
                            output.Append(" ");
                    }
                    else if (field is GoalField goalField)
                    {
                        if (goalField.Type == Shared.CommonResources.GoalFieldType.Goal)
                            output.Append("G ");
                        else
                            output.Append("+ ");
                    }
                    else if (field is TaskField taskField)
                    {
                        if (taskField.PieceId != null)
                            output.Append("p ");
                        else
                            output.Append("- ");
                    }
                    Console.Write(output.ToString());
                }
                Console.WriteLine();
            }

        }
    }
}
