namespace SudokuSolver.Core.Models
{
    internal class SudokuError : Exception
    {
        internal enum ErrorType
        {
            None,
            ArrayCountError,
            FrameRowCountError,
            FrameColumnCountError,
            SudokuUncompletedError,
            MultipleCandidatesError,
            SudokuUnsolvable,
            UnknownError
        }

        internal ErrorType Type = ErrorType.None;
        internal int[,]? Sudoku;

        internal SudokuError(ErrorType type)
        {
            Type = type;
            Sudoku = null;
        }

        internal SudokuError(ErrorType type, int[,] sudoku)
        {
            Type = type;
            Sudoku = sudoku;
        }

        internal SudokuError(ErrorType type, string message) : base(message)
        {
            Type = type;
        }
    }
}