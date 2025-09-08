namespace SudokuSolver.Core
{
    internal static class Searcher
    {
        internal static Candidates SearchCandidates(Candidates[] row, Candidates[] column, Candidates[,] frame)
        {
            var posibles = Candidates.All;
            posibles &= SearchRow(row);
            posibles &= SearchColumn(column);
            posibles &= SearchFrame(frame);
            return posibles;
        }

        private static Candidates SearchRow(Candidates[] row)
        {
            if (row.Length != Constants.SudokuSize) throw new SearcherError(SearcherError.ErrorType.RowCountError);

            var posibles = Candidates.All;
            foreach (Candidates cell in row)
            {
                if (cell != Candidates.None) posibles &= ~cell;
            }

            return posibles;
        }

        private static Candidates SearchColumn(Candidates[] column)
        {
            if (column.Length != Constants.SudokuSize) throw new SearcherError(SearcherError.ErrorType.ColumnCountError);

            var posibles = Candidates.All;
            foreach (Candidates cell in column)
            {
                if (cell != Candidates.None) posibles &= ~cell;
            }

            return posibles;
        }

        private static Candidates SearchFrame(Candidates[,] frame)
        {
            if (frame.GetLength(0) != Constants.FrameSize) throw new SearcherError(SearcherError.ErrorType.FrameRowCountError);
            if (frame.GetLength(1) != Constants.FrameSize) throw new SearcherError(SearcherError.ErrorType.FrameColumnCountError);

            var posibles = Candidates.All;
            for (int i = 0; i < Constants.FrameSize; i++)
            {
                for (int j = 0; j < Constants.FrameSize; j++)
                {
                    if (frame[i,j] != Candidates.None) posibles &= ~frame[i,j];
                }
            }

            return posibles;
        }

        internal class SearcherError : Exception
        {
            internal enum ErrorType
            {
                None,
                RowCountError,
                ColumnCountError,
                FrameRowCountError,
                FrameColumnCountError
            }

            internal ErrorType Type = ErrorType.None;

            internal SearcherError(ErrorType type) { Type = type; }
        }
    }
}