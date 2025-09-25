using SudokuSolver.Core.Constants;
using SudokuSolver.Core.Enums;

namespace SudokuSolver.Core.Services
{
    internal static class HiddenPairTriple
    {
        internal static int[] Search(Candidates[] array, Candidates values) 
        {
            int candLen = CountCandidates(values);
            int[] ret = GiveErrorArray(candLen);

            for (int i = 0; i < array.Length; i++)
            {
                if ((array[i] & values) == 0)               // The cell does not contain neither of the candidates
                {
                    continue;
                }
                else if ((array[i] & values) != values)      // Contains at least one of the candidates, no hidden pair/triple
                {
                    return GiveErrorArray(candLen);     
                }

                // Once reached to this point, the cell contains the candidates
                for (int k = 0; k < candLen; k++)
                {
                    if (ret[k] == -1)
                    {
                        ret[k] = i;
                        break;
                    }
                    else if (k == candLen - 1)
                    {
                        return GiveErrorArray(candLen);     // Too many cells contain the values
                    }
                }
            }
            return ret;
        }

        internal static CellPointer[] Search(Candidates[,] matrix, Candidates values) 
        {
            int candLen = CountCandidates(values);
            CellPointer[] ret = GiveErrorMatrix(candLen);

            for (int i = 0; i < ConstantData.FrameSize; i++)
            {
                for (int j = 0; j < ConstantData.FrameSize; j++)
                {
                    if ((matrix[i, j] & values) == 0)               // The cell does not contain the candidate
                    {
                        continue;
                    }
                    else if ((matrix[i, j] & values) != values)      // Contains at least one of the candidates, no hidden pair/triple
                    {
                        return GiveErrorMatrix(candLen);     
                    }

                    // Once reached to this point, the cell contains the candidates
                    for (int k = 0; k < candLen; k++)
                    {
                        if (ret[k].Row == -1)
                        {
                            ret[k].Row = i;
                            ret[k].Col = j;
                            break;
                        }
                        else if (k == candLen - 1)
                        {
                            return GiveErrorMatrix(candLen);     // Too many cells contain the values
                        }
                    }
                }                
            }
            return ret;
        }

        private static int CountCandidates(Candidates candidates)
        {
            int value = (int)candidates;
            int count = 0;
            while (value != 0)
            {
                count++;
                value &= value - 1;
            }
            return count;
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