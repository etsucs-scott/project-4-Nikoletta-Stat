using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Core
{
    public class PuzzleSolver
    {
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
                    SudokuBoard.ValidBox(row / 3, col % 3))
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
