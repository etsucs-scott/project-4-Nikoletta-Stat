using System.Reflection.Metadata.Ecma335;

namespace Sudoku.Core;

public class Board
{
    public Cell[,] board = new Cell[9, 9];

    public Board()
    {
        for (int r = 0; r < 9; r++)
        {
            for (int c = 0; c < 9; c++)
                board[r, c] = new Cell();
        }
    }

    public void SetCellValue(int row, int col, int value)
    {
        if (board[row, col].given)
        {
            throw new Exception("Cannot change a given value.");
        }
        board[row, col].value = value;
    }

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
