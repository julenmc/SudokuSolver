namespace SudokuSolver.Core
{
    internal static class Searcher
    {
        internal static Candidates SearchCandidates(Candidates[] row, Candidates[] column, Candidates[,] frame)
        {
            var posibles = Candidates.All;
            posibles &= SearchArrayCandidates(row);
            posibles &= SearchArrayCandidates(column);
            posibles &= SearchFrameCandidates(frame);
            return posibles;
        }

        private static Candidates SearchArrayCandidates(Candidates[] array)
        {
            if (array.Length != Constants.SudokuSize) throw new SudokuError(SudokuError.ErrorType.ArrayCountError);

            var posibles = Candidates.All;
            foreach (Candidates cell in array)
            {
                if (cell != Candidates.None && (cell & (cell - 1)) == 0) posibles &= ~cell;
            }

            return posibles;
        }

        private static Candidates SearchFrameCandidates(Candidates[,] frame)
        {
            if (frame.GetLength(0) != Constants.FrameSize) throw new SudokuError(SudokuError.ErrorType.FrameRowCountError);
            if (frame.GetLength(1) != Constants.FrameSize) throw new SudokuError(SudokuError.ErrorType.FrameColumnCountError);

            var posibles = Candidates.All;
            for (int i = 0; i < Constants.FrameSize; i++)
            {
                for (int j = 0; j < Constants.FrameSize; j++)
                {
                    if (frame[i, j] != Candidates.None && (frame[i, j] & (frame[i, j] - 1)) == 0) posibles &= ~frame[i, j];
                }
            }

            return posibles;
        }

        internal static int SearchNumberCellValueInArray(Candidates[] array, Candidates value)
        {
            int foundIndex = -1;
            for (int i = 0; i < Constants.SudokuSize; i++)
            {
                if (array[i] == value)
                {
                    return -1;          // The candidate is already assigned
                }
                else if ((array[i] & value) != 0)
                {
                    if (foundIndex != -1)
                    {
                        return -1;  // The candidate was already found in the array
                    }
                    foundIndex = i;
                }
            }
            return foundIndex;
        }

        internal static (int, int) SearchNumberCellValueInFrame(Candidates[,] frame, Candidates value)
        {
            int foundRowIndex = -1;
            int foundColumnIndex = -1;
            for (int i = 0; i < Constants.FrameSize; i++)
            {
                for (int j = 0; j < Constants.FrameSize; j++)
                {
                    if (frame[i, j] == value)
                    {
                        return (-1, -1);      // The candidate is already assigned
                    }
                    if ((frame[i, j] & value) != 0)
                    {
                        if (foundRowIndex != -1)
                        {
                            return (-1, -1);  // The candidate was already found in the frame
                        }
                        foundRowIndex = i;
                        foundColumnIndex = j;
                    }
                }
            }
            return (foundRowIndex, foundColumnIndex);
        }
    }
}