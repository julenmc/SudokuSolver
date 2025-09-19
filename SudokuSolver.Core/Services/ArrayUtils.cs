using SudokuSolver.Core.Constants;

namespace SudokuSolver.Core.Services
{
    internal static class ArrayUtils<T>
    {
        internal static T[] GetColumn(T[,] matrix, int columnNumber)
        {
            var result = new T[ConstantData.SudokuSize];
            for (int r = 0; r < ConstantData.SudokuSize; r++)
            {
                result[r] = matrix[r, columnNumber];
            }
            return result;
        }

        internal static T[] GetRow(T[,] matrix, int rowNumber)
        {
            var result = new T[ConstantData.SudokuSize];
            for (int c = 0; c < ConstantData.SudokuSize; c++)
            {
                result[c] = matrix[rowNumber, c];
            }
            return result;
        }

        internal static T[,] Slice2DArray(T[,] matrix, int rowStart, int columnStart, int size)
        {
            var result = new T[size, size];

            for (int r = 0; r < size; r++)
            {
                for (int c = 0; c < size; c++)
                {
                    result[r, c] = matrix[rowStart + r, columnStart + c];
                }
            }

            return result;
        }
    }
}