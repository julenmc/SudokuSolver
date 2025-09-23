using SudokuSolver.Core.Constants;
using SudokuSolver.Core.Enums;

namespace SudokuSolver.Core.Services
{
    internal static class HiddenPairTriple
    {
        internal static int[] Search(Candidates[] array, Candidates[] values)   // TODO: values could use "|" and not be an array
        {
            int[] ret = GiveErrorArray(values.Length);

            for (int i = 0; i < array.Length; i++)
            {
                bool stopped = false;
                for (int j = 0; j < values.Length; j++)
                {
                    if (array[i] == values[j])
                    {
                        return GiveErrorArray(values.Length); // The candidate is already assigned
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
                        return GiveErrorArray(values.Length);     // Too many cells contain the values
                    }
                }
            }
            return ret;
        }

        internal static CellPointer[] Search(Candidates[,] matrix, Candidates[] values) // TODO: values could use "|" and not be an array 
        {
            CellPointer[] ret = GiveErrorMatrix(values.Length);

            for (int i = 0; i < ConstantData.FrameSize; i++)
            {
                for (int j = 0; j < ConstantData.FrameSize; j++)
                {
                    bool stopped = false;
                    for (int v = 0; v < values.Length; v++)
                    {
                        if (matrix[i,j] == values[v])
                        {
                            return GiveErrorMatrix(values.Length); // The candidate is already assigned
                        }
                        else if ((matrix[i, j] & values[v]) == 0)     // The cell does not contain the candidate
                        {
                            stopped = true;
                            break;
                        }
                    }
                    if (stopped) continue;

                    // Once reached to this point, the cell contains the candidates
                    for (int k = 0; k < values.Length; k++)
                    {
                        if (ret[k].Row == -1)
                        {
                            ret[k].Row = i;
                            ret[k].Col = j;
                            break;
                        }
                        else if (k == values.Length - 1)
                        {
                            return GiveErrorMatrix(values.Length);     // Too many cells contain the values
                        }
                    }
                }                
            }
            return ret;
        }

        private static int[] GiveErrorArray(int len)
        {
            int[] ret = new int[len];
            for (int i = 0; i < ret.Length; i++)
                ret[i] = -1;

            return ret;
        }

        private static CellPointer[] GiveErrorMatrix(int len)
        {
            CellPointer[] ret = new CellPointer[len];
            for (int i = 0; i < len; i++)
                ret[i] = new CellPointer { Row = -1, Col = -1 };

            return ret;
        }

        internal class CellPointer
        {
            internal int Row { get; set; }
            internal int Col { get; set; }
        }
    }
}