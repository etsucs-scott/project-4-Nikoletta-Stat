using System.Reflection.Metadata.Ecma335;

namespace Sudoku.Core;

// The Board class holds the rules and methods for the Sudoku grid.
public class Board
{
    public Cell[,] board = new Cell[9, 9];

    // A board is a 9x9 2-dimensional array of cells.
    // The constructor creates a new cell for every grid coordinate.
    public Board()
    {
        for (int r = 0; r < 9; r++)
        {
            for (int c = 0; c < 9; c++)
                board[r, c] = new Cell();
        }
    }

    // SetCellValue takes in a row, column, and value and checks if the coordinate holds a given clue.
    // If given, it throws an exception. If not given, the cell now holds the value input by the user.
    public void SetCellValue(int row, int col, int value)
    {
        if (board[row, col].given)
        {
            throw new Exception("Cannot change a given value.");
        }
        board[row, col].value = value;
    }

    // ValidRow() checks to ensure that the user input value only appears once in the row.
    // It uses a HashSet to hold seen values, and if the loop tries to add the same value twice, it returns false.
    public bool ValidRow(int row)
    {
        var seen = new HashSet<int>();
        for (int c = 0; c < 9; c++)
        {
            int v = board[row, c].value;
            if (v == 0)
                continue;
            if (!seen.Add(v))
                return false;
        }
        return true;
    }

    // ValidCol() checks to ensure that the user input value only appears once in the column.
    // It uses a HashSet to hold seen values, and if the loop tries to add the same value twice, it returns false.
    public bool ValidCol(int col)
    {
        var seen = new HashSet<int>();
        for (int r = 0; r < 9; r++)
        {
            int v = board[r, col].value;
            if (v == 0)
                continue;
            if (!seen.Add(v))
                return false;
        }
        return true;
    }

    // ValidBox() checks to ensure that the user input value only appears once in the 3x3 box.
    // It uses a HashSet to hold seen values, and if the loop tries to add the same value twice, it returns false.
    // boxRow and boxCol are the coordinates for the specific box on a 3x3 grid of boxes. Ranging from 0-2.
    public bool ValidBox(int boxRow, int boxCol)
    {
        var seen = new HashSet<int>();

        int startingRow = boxRow * 3;
        int startingCol = boxCol * 3;

        for (int r =  startingRow; r < startingRow + 3; r++)
        {
            for (int c = startingCol;  c < startingCol + 3; c++)
            {
                int v = board[r, c].value;
                if (v == 0) continue;
                if (!seen.Add(v)) return false;
            }
        }
        return true;
    }

    // ValidBoard() checks each row, column, and box in the board to make sure there are no duplicate values per row, col, or box.
    // Validates the board based on Sudoku game rules.
    public bool ValidBoard()
    {
        for (int i = 0; i < 9; i++)
        {
            if (!ValidRow(i)) return false;
            if (!ValidCol(i)) return false;
            if (!ValidBox(i / 3, i % 3)) return false;
        }
        return true;
    }

    // Complete() checks to make sure every cell in the board is filled in. Returns false if there is an empty cell.
    // If the board is completely filled, it calls ValidBoard() to make sure the board is valid.
    public bool Complete()
    {
        for (int r = 0; r < 9; r++)
        {
            for (int c = 0; c < 9; c++)
            {
                if (board[r, c].value == 0)
                    return false;
            }
        }
        return ValidBoard();
    }

    // NextEmptyCell() returns the coordinates of the first cell with no value.
    public (int, int)? NextEmptyCell()
    {
        for (int r = 0; r < 9; r++)
        {
            for (int c = 0; c < 9; c++)
            {
                if (board[r, c].value == 0)
                    return (r, c);
            }
        }
        return null;
    }
}
