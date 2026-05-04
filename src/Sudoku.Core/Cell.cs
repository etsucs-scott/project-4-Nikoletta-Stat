using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Core
{
    // The cell class holds the value and whether or not the cell is a given clue for each cell on the board.
    public class Cell
    {
        public int value {  get; set; }
        public bool given { get; set; }
    }
}
