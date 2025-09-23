using SudokuSolver.Core.Constants;
using SudokuSolver.Core.Enums;

namespace SudokuSolver.Core.Services
{
    internal static class HiddenSingles
    {
        internal static int Search(Candidates[] array, Candidates value)
        {
            int foundIndex = -1;
            for (int i = 0; i < ConstantData.SudokuSize; i++)
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

        internal static (int, int) Search(Candidates[,] frame, Candidates value)
        {
            int foundRowIndex = -1;
            int foundColumnIndex = -1;
            for (int i = 0; i < ConstantData.FrameSize; i++)
            {
                for (int j = 0; j < ConstantData.FrameSize; j++)
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