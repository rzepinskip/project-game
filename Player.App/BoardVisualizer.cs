using System;
using System.Collections.Generic;
using Common;
using Common.BoardObjects;

namespace Player.App
{
    public class BoardVisualizer
    {
        public void VisualizeBoard(PlayerBoard board, int? playerId)
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

                            if (playerId != null && field.PlayerId == playerId)
                            {
                                if (player.Team == TeamColor.Red)
                                    symbol = new ColoredString("R", ConsoleColor.Green);
                                else
                                    symbol = new ColoredString("B", ConsoleColor.Green);
                            }
                            else
                            {
                                if (player.Team == TeamColor.Red)
                                    symbol = new ColoredString("r", ConsoleColor.DarkRed);
                                else
                                    symbol = new ColoredString("b", ConsoleColor.DarkCyan);
                            }

                            if (player.Piece != null)
                                symbol.Data += "p";
                            else
                                symbol.Data += " ";
                        }
                        else if (field is GoalField goalField)
                        {
                            switch (goalField.Type)
                            {
                                case GoalFieldType.NonGoal:
                                    symbol.Data = "N ";
                                    break;
                                case GoalFieldType.Goal:
                                    symbol = new ColoredString("G ", ConsoleColor.DarkGray);
                                    break;
                                case GoalFieldType.Unknown:
                                    symbol.Data = ". ";
                                    break;
                            }
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