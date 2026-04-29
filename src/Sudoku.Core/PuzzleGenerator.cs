using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Core
{
    public enum DifficultyLevel
    {
        Easy,
        Medium,
        Hard
    }
    public class PuzzleGenerator
    {
        private readonly PuzzleSolver solver = new PuzzleSolver();
        Random rand = new Random();

        public Board GenerateNewPuzzle(DifficultyLevel level)
        {
            int givenNums = level switch
            {
                DifficultyLevel.Easy => 38,
                DifficultyLevel.Medium => 36,
                DifficultyLevel.Hard => 32,
                _ => 36
            };

            var board = GenerateSolvedPuzzle();
            RemoveValues(board, givenNums);
            return board;
        }
        private Board GenerateSolvedPuzzle()
        {
            var board = new Board();

            for (int i = 0; i < 10; i++)
            {
                int row = rand.Next(0, 9);
                int col = rand.Next(0, 9);
                int v = rand.Next(0, 10);

                board.board[row, col].value = v;

                if (!board.ValidRow(row) || 
                    !board.ValidCol(col) || 
                    !board.ValidBox(row/3, col/3))
                {
                    board.board[row, col].value = 0;
                }

            }

            solver.Solved(board);

            for (int r = 0; r < 9; r++)
            {
                for (int c = 0; c < 9; c++)
                {
                    if (board.board[r, c] == null)
                        board.board[r, c] = new Cell();
                }
            }

            return board;
        }

        private void RemoveValues(Board board, int numGiven)
        {
            var boxPositions = Enumerable.Range(0, 81).ToList();
            Shuffle(boxPositions);

            int removed = 0;
            int removing = 81 - numGiven;

            foreach (int position in boxPositions)
            {
                if (removed >= removing)
                    break;

                int r = position / 9;
                int c = position % 9;

                int cellValue = board.board[r, c].value;
                board.board[r, c].value = 0;

                if (solver.Solutions(board) != 1)
                {
                    board.board[r, c].value = cellValue;
                    board.board[r, c].given = true;
                }
                else
                {
                    board.board[r, c].given = false;
                    removed++;
                }
            }

            for (int r = 0; r < 9; r++)
            {
                for (int c = 0; c < 9; c++)
                {
                    if (board.board[r, c].value != 0)
                        board.board[r, c].given = true;
                    else
                        board.board[r, c].given = false;
                }
            }
        }

        public void Shuffle (List<int> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = rand.Next(i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }
    }

}
