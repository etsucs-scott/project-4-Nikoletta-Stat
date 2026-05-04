using Sudoku.Core;
namespace Sudoku.Tests
{
    public class SudokuTests
    {
        [Fact]
        public void BoardStartsEmpty()
        {
            var board = new Board();
            for (int r = 0; r < 9;  r++)
            {
                for (int c= 0; c < 9; c++)
                {
                    Assert.Equal(0, board.board[r, c].value);
                }
            }
        }

        [Fact]
        public void ValidRowChecksForDuplicateValues()
        {
            var board = new Board();
            board.board[0, 0].value = 1;
            board.board[0, 3].value = 1;
            Assert.False(board.ValidRow(0));
        }

        [Fact]
        public void ValidColChecksForDuplicateValues()
        {
            var board = new Board();
            board.board[0, 0].value = 1;
            board.board[3, 0].value = 1;
            Assert.False(board.ValidCol(0));
        }

        [Fact]
        public void ValidBoxChecksForDuplicateValues()
        {
            var board = new Board();
            board.board[0, 0].value = 1;
            board.board[1, 1].value = 1;
            Assert.False(board.ValidBox(0, 0));
        }

        [Fact]
        public void NextEmptyCell_ReturnsZeroValueCell()
        {
            var board = new Board();
            board.board[0, 0].value = 1;

            var cell = board.NextEmptyCell();
            int cellRow = cell.Value.Item1;
            int cellCol = cell.Value.Item2;

            Assert.Equal(0, cellRow);
            Assert.Equal(1, cellCol);
        }

        [Fact]
        public void NextEmptyCell_ReturnsNull_IfBoardIsFull()
        {
            var board = new Board();
            for (int r = 0; r < 9; r++)
            {
                for (int c = 0; c < 9;  c++)
                {
                    board.board[r, c].value = 1;
                }
            }
            Assert.Null(board.NextEmptyCell());
        }

        [Fact]
        public void Complete_ReturnsFalse_WhenBoardIsNotFull()
        {
            var board = new Board();
            Assert.False(board.Complete());
        }

        [Fact]
        public void SolverFails_WhenPuzzleIsInvalid()
        {
            var board = new Board();
            board.board[0, 0].value = 1;
            board.board[0, 1].value = 1;

            var solver = new PuzzleSolver();

            Assert.False(solver.Solved(board));
        }

        [Fact]
        public void Solutions_ReturnsTwo_WhenBoardHasManySolutions()
        {
            var board = new Board();
            var solver  = new PuzzleSolver();
            int solCount = solver.Solutions(board);
            Assert.Equal(2, solCount);
        }

        [Fact]
        public void PuzzleGenerator_GeneratesValidBoard()
        {
            var generator = new PuzzleGenerator();
            var board = generator.GenerateNewPuzzle(DifficultyLevel.Easy);
            Assert.True(board.ValidBoard());
        }
            
    }
}