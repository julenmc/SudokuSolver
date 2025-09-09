namespace SudokuSolver.Core
{
    internal static class ArrayUtils<T>
    {
        internal static T[] GetColumn(T[,] matrix, int columnNumber)
        {
            var result = new T[Constants.SudokuSize];
            for (int r = 0; r < Constants.SudokuSize; r++)
            {
                result[r] = matrix[r, columnNumber];
            }
            return result;
        }

        internal static T[] GetRow(T[,] matrix, int rowNumber)
        {
            var result = new T[Constants.SudokuSize];
            for (int c = 0; c < Constants.SudokuSize; c++)
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