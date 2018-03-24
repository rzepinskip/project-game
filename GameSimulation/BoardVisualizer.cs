using System;
using System.Collections.Generic;
using Common;
using Common.BoardObjects;
using Common.Interfaces;

namespace GameSimulation
{
    internal class BoardVisualizer
    {
        public void VisualizeBoard(IBoard board)
        {
            var output = new List<ColoredString>(100);
            for (var i = board.Height - 1; i >= 0; i--)
            {
                for (var j = 0; j < board.Width; j++)
                {
                    var symbol = new ColoredString("");

                    var field = board[new Location(j, i)];
                    if (field.PlayerId != null)
                    {
                        var player = board.Players[field.PlayerId.Value];
                        if (player.Team == TeamColor.Red)
                            symbol = new ColoredString("R", ConsoleColor.DarkRed);
                        else
                            symbol = new ColoredString("B", ConsoleColor.DarkCyan);

                        if (player.Piece != null)
                            symbol.Data += "p";
                        else
                            symbol.Data += " ";
                    }
                    else if (field is GoalField goalField)
                    {
                        if (goalField.Type == GoalFieldType.Goal)
                            symbol.Data = "G ";
                        else
                            symbol.Data = "+ ";
                    }
                    else if (field is TaskField taskField)
                    {
                        if (taskField.PieceId != null)
                            symbol.Data = "p ";
                        else
                            symbol.Data = "- ";
                    }

                    output.Add(symbol);
                }

                output.Add(new ColoredString("\n"));
            }

            Console.CursorVisible = false;
            Console.SetCursorPosition(0, 0);

            foreach (var coloredString in output)
            {
                Console.ForegroundColor = coloredString.Color;
                Console.Write(coloredString.Data);
            }
        }

        private class ColoredString
        {
            public ColoredString(string data, ConsoleColor color = ConsoleColor.White)
            {
                Data = data;
                Color = color;
            }

            public string Data { get; set; }
            public ConsoleColor Color { get; }
        }
    }
}