using Shared.BoardObjects;
using System;
using System.Text;

namespace GameSimulation
{
    class BoardVisualizer
    {
        public void VisualizeBoard(Board board)
        {
            var output = new StringBuilder();
            for (int i = board.Height - 1; i >= 0; i--)
            {
                for (int j = 0; j < board.Width; j++)
                {
                    var objectSymbol = new StringBuilder();

                    var field = board.Content[j, i];
                    if (field.PlayerId != null)
                    {
                        var player = board.Players[field.PlayerId.Value];
                        if (player.Team == Shared.CommonResources.TeamColour.Red)
                            objectSymbol.Append("R");
                        else
                            objectSymbol.Append("B");

                        if (player.Piece != null)
                            objectSymbol.Append("p");
                        else
                            objectSymbol.Append(" ");
                    }
                    else if (field is GoalField goalField)
                    {
                        if (goalField.Type == Shared.CommonResources.GoalFieldType.Goal)
                            objectSymbol.Append("G ");
                        else
                            objectSymbol.Append("+ ");
                    }
                    else if (field is TaskField taskField)
                    {
                        if (taskField.PieceId != null)
                            objectSymbol.Append("p ");
                        else
                            objectSymbol.Append("- ");
                    }
                    output.Append(objectSymbol);
                }
                output.AppendLine();
            }

            Console.CursorVisible = false;
            Console.SetCursorPosition(0, 0);

            Console.WriteLine(output.ToString());
        }
    }
}
