namespace SudokuSolver.Core
{
    internal static class ArrayTools<T>
    {
        internal static T[] GetColumn(T[,] matrix, int columnNumber)
        {
            return Enumerable.Range(0, matrix.GetLength(0))
                    .Select(x => matrix[x, columnNumber])
                    .ToArray();
        }

        internal static T[] GetRow(T[,] matrix, int rowNumber)
        {
            return Enumerable.Range(0, matrix.GetLength(1))
                    .Select(x => matrix[rowNumber, x])
                    .ToArray();
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