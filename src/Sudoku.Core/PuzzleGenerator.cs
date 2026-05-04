using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Core
{
    // DifficultyLevel holds the three levels of difficulty, each with different numbers of given values/ clues on the generated board.
    public enum DifficultyLevel
    {
        Easy,
        Medium,
        Hard
    }

    // PuzzleGenerator generates a solved puzzle by validating every game rule and using a backtracking
    // algorithm to ensure a unique puzzle every time.
    public class PuzzleGenerator
    {
        private readonly PuzzleSolver solver = new PuzzleSolver();
        Random rand = new Random();

        // I originally wanted the user to be able to choose the difficulty level with a different number of given clues,
        // but I ended up hardcoding the easy mode for testing purposes. I still might go back and add the ability to choose the difficulty level,
        // which is why I left the code in GenerateNewPuzzle().

        // GenerateNewPuzzle() calls other methods to generate a fully solved puzzle and remove numbers, then returns a solvable board.
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

        // GenerateSolvedPuzzle() generates random cell coordinates and a random value. It validates the board rules and places the random
        // value in the cell, then returns a solved puzzle.
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

        // RemoveValues takes a fully solved generated board and removes numbers from the board until there is only a specified number 
        // of given clues. It calls PuzzleSolver.Solutions() to ensure that each removed number still only leaves one solution for the board.
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

        // Shuffle() uses a shuffling algorithm to ensure extra randomness with removing numbers.
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
