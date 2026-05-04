using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Core
{
    // The PuzzleSolver class solves a Sudoku board. It is a helper class for the PuzzleGenerator.
    public class PuzzleSolver
    {
        // Solved() finds the first empty cell in the board and loops through every possible value to find a value that works in that cell.
        // It temporarily places the value and recursively calls Solved() again to solve the rest of the board. If at any point no number works, it
        // returns false.
        public bool Solved(Board SudokuBoard)
        {
            var emptyCell = SudokuBoard.NextEmptyCell();
            if (emptyCell == null)
                return true;

            int row = emptyCell.Value.Item1;
            int col = emptyCell.Value.Item2;

            for (int i = 1; i <= 9; i++)
            {
                SudokuBoard.board[row, col].value = i;

                if (SudokuBoard.ValidRow(row) &&
                    SudokuBoard.ValidCol(col) &&
                    SudokuBoard.ValidBox(row / 3, col / 3))
                {
                    if (Solved(SudokuBoard))
                        return true;

                }
                SudokuBoard.board[row, col].value = 0;
            }
            return false;
        }

        // Solutions() finds the first empty cell and loops through and temporarily places a value for that cell. It calls Solutions()
        // recursively to find values for all empty cells in the board. If more than one value can go in a cell and the board still be complete,
        // Solutions() counts a solution. Since the game sudoku can only have one solution for the game to be valid, any solution count greater than
        // or equal to two means that the board is invalid.
        public int Solutions(Board SudokuBoard)
        {
            var emptyCell = SudokuBoard.NextEmptyCell();
            if (emptyCell == null)
                return 1;

            int row = emptyCell.Value.Item1;
            int col = emptyCell.Value.Item2;
            int solutionCount = 0;

            for (int i = 1; i <= 9; i++)
            {
                SudokuBoard.board[row, col].value = i;
                if (SudokuBoard.ValidRow(row) &&
                    SudokuBoard.ValidCol(col) &&
                    SudokuBoard.ValidBox(row / 3, col / 3))
                {
                    solutionCount += Solutions(SudokuBoard);
                    if (solutionCount >= 2)
                    {
                        SudokuBoard.board[row, col].value = 0;
                        return solutionCount;
                    }
                }
            }

            SudokuBoard.board[row, col].value = 0;
            return solutionCount;
        }
    }
}
