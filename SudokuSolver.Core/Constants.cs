namespace SudokuSolver.Core
{
    [Flags]
    internal enum Candidates : ushort
    {
        None = 0,
        One = 1 << 0,       // 0b000000001
        Two = 1 << 1,       // 0b000000010
        Three = 1 << 2,     // 0b000000100
        Four = 1 << 3,      // 0b000001000
        Five = 1 << 4,      // 0b000010000
        Six = 1 << 5,       // 0b000100000
        Seven = 1 << 6,     // 0b001000000
        Eight = 1 << 7,     // 0b010000000
        Nine = 1 << 8,      // 0b100000000
        All = (1 << 9) - 1  // 0b111111111
    }

    public static class Constants
    {
        public static int SudokuSize = 9;
        internal static int FrameSize = 3;
    }

    internal class SudokuError : Exception
    {
        internal enum ErrorType
        {
            None,
            RowCountError,
            ColumnCountError,
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