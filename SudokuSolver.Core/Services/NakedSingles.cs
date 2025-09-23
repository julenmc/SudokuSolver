using SudokuSolver.Core.Constants;
using SudokuSolver.Core.Enums;
using SudokuSolver.Core.Models;

namespace SudokuSolver.Core.Services
{
    internal static class NakedSingles
    {
        internal static Candidates Search(Candidates[] row, Candidates[] column, Candidates[,] frame)
        {
            var posibles = Candidates.All;
            posibles &= SearchArrayCandidates(row);
            posibles &= SearchArrayCandidates(column);
            posibles &= SearchFrameCandidates(frame);
            return posibles;
        }

        private static Candidates SearchArrayCandidates(Candidates[] array)
        {
            if (array.Length != ConstantData.SudokuSize) throw new SudokuError(SudokuError.ErrorType.ArrayCountError);

            var posibles = Candidates.All;
            foreach (Candidates cell in array)
            {
                if (cell != Candidates.None && (cell & (cell - 1)) == 0) posibles &= ~cell;
            }

            return posibles;
        }

        private static Candidates SearchFrameCandidates(Candidates[,] frame)
        {
            if (frame.GetLength(0) != ConstantData.FrameSize) throw new SudokuError(SudokuError.ErrorType.FrameRowCountError);
            if (frame.GetLength(1) != ConstantData.FrameSize) throw new SudokuError(SudokuError.ErrorType.FrameColumnCountError);

            var posibles = Candidates.All;
            for (int i = 0; i < ConstantData.FrameSize; i++)
            {
                for (int j = 0; j < ConstantData.FrameSize; j++)
                {
                    if (frame[i, j] != Candidates.None && (frame[i, j] & (frame[i, j] - 1)) == 0) posibles &= ~frame[i, j];
                }
            }

            return posibles;
        }
    }
}