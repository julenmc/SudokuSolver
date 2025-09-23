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

        internal SudokuError(ErrorType type) { Type = type; }
        internal SudokuError(ErrorType type, string message) : base(message) { Type = type; }
    }
}