using System;
using System.Collections.Generic;
using Common;
using Common.BoardObjects;

namespace GameMaster.App
{
    internal class BoardVisualizer
    {
        public void VisualizeBoard(GameMasterBoard board)
        {
            lock (board.Lock)
            {
                var output = new List<ColoredString>(100);
                for (var y = board.Height - 1; y >= 0; y--)
                {
                    for (var x = 0; x < board.Width; x++)
                    {
                        var symbol = new ColoredString("");

                        var field = board[new Location(x, y)];
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
                            {
                                if (board.UncompletedRedGoalsLocations.Contains(goalField) ||
                                    board.UncompletedBlueGoalsLocations.Contains(goalField))
                                {
                                    symbol.Data = "G ";
                                }
                                else
                                {
                                    symbol = new ColoredString("G ", ConsoleColor.DarkGray);
                                }


                            }
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