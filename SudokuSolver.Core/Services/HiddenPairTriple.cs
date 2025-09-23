using SudokuSolver.Core.Enums;

namespace SudokuSolver.Core.Services
{
    internal static class HiddenPairTriple
    {
        // Finds 2/3 cells with the same 2/3 candidates that none other cell has
        internal static int[] Search(Candidates[] array, Candidates[] values)
        {
            int[] ret = new int[values.Length];
            for (int i = 0; i < ret.Length; i++)
                ret[i] = -1;
                
            for (int i = 0; i < array.Length; i++)
            {
                bool stopped = false;
                for (int j = 0; j < values.Length; j++)
                {
                    if (array[i] == values[j])
                    {
                        return new int[] { -1, -1 };          // The candidate is already assigned
                    }
                    else if ((array[i] & values[j]) == 0)     // The cell does not contain the candidate
                    {
                        stopped = true;
                        break;
                    }
                }
                if (stopped) continue;

                // Once reached to this point, the cell contains the candidates
                for (int k = 0; k < values.Length; k++)
                {
                    if (ret[k] == -1)
                    {
                        ret[k] = i;
                        break;
                    }
                    else if (k == values.Length - 1)
                    {
                        return new int[] { -1, -1 };          // Too many cells contain the values
                    }
                }
            }
            // returns matrix with the positions found for the values
            return ret;
        }

        private static void SearchNumber()
        {

        }
    }
}