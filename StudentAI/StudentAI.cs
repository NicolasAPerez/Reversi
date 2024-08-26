using GameAI.GamePlaying.Core;
using System;
using System.Collections.Generic;

namespace GameAI.GamePlaying
{
    public class StudentAI : Behavior
    {
        // TODO: Methods go here
        public ComputerMove Run(int _color, Board _board, int _lookAheadDepth)
        {
            return RecursiveLook(_board, _lookAheadDepth, _color);
        }

        private int EvalBoard(Board _board)
        {
            int totalScore = 0;
            //Calculate indivual tile scores
            for (int x = 0; x < Board.Width; x++)
            {
                for (int y = 0; y < Board.Height; y++)
                {
                    int tile = _board.GetTile(x, y);
                    if (x == 0 || x == Board.Width - 1)
                    {
                        tile *= 10;
                    }
                    if (y == 0 || y == Board.Width - 1)
                    {
                        tile *= 10;
                    }
                    totalScore += tile;

                }
            }

            //Calculate win condition
            if (_board.IsTerminalState())
            {
                if (_board.Score > 0)
                {
                    totalScore += 10000;
                }
                else if (_board.Score < 0)
                {
                    totalScore -= 10000;
                }
            }


            return totalScore;
        }

        private ComputerMove RecursiveLook(Board board, int lookAhead, int color)
        {
            List<ComputerMove> possibleStates = new List<ComputerMove>();

            ComputerMove finalMove = null;

            for (int x = 0; x < Board.Width; x++)
            {
                for (int y = 0; y < Board.Height; y++)
                {
                    if (board.IsValidMove(color, x, y))
                    {
                        ComputerMove temp = new ComputerMove(x, y);

                        Board testBoard = new Board();
                        testBoard.Copy(board);
                        testBoard.MakeMove(color, x, y);

                        if (lookAhead > 0)
                        {
                            ComputerMove tester = RecursiveLook(testBoard, lookAhead - 1, -color);
                            if (tester != null)
                            {
                                temp.rank = tester.rank;
                            }
                        }
                        else
                        {
                            temp.rank = EvalBoard(testBoard);
                        }
                        possibleStates.Add(temp);
                    }

                }
            }
            if (possibleStates.Count > 0) {
                finalMove = possibleStates[0];

                foreach (ComputerMove move in possibleStates)
                {
                    if (color == 1)
                    {
                        if (move.rank > finalMove.rank)
                        {
                            finalMove = move;
                        }
                    }
                    else
                    {
                        if (move.rank < finalMove.rank)
                        {
                            finalMove = move;
                        }
                    }
                }
            }

            return finalMove;


        }
    }
}
