namespace SudokuSolver.Core
{
    internal static class Verifier
    {
        internal static bool VerifySudoku(Candidates[,] matrix)
        {
            // Chek input
            if (matrix.GetLength(0) != Constants.SudokuSize) throw new SudokuError(SudokuError.ErrorType.RowCountError);
            if (matrix.GetLength(1) != Constants.SudokuSize) throw new SudokuError(SudokuError.ErrorType.ColumnCountError);

            try
            {
                // Check rows
                for (int i = 0; i < Constants.SudokuSize; i++)
                {
                    if (!VerifyRow(ArrayTools<Candidates>.GetRow(matrix, i)))
                    {
                        Console.WriteLine($"Row {i} has a repeated value");
                        return false;
                    }
                }

                // Check columns
                for (int i = 0; i < Constants.SudokuSize; i++)
                {
                    if (!VerifyColumn(ArrayTools<Candidates>.GetColumn(matrix, i)))
                    {
                        Console.WriteLine($"Column {i} has a repeated value");
                        return false;
                    }
                }

                // Check frames
                double frameNum = Math.Pow(Constants.SudokuSize, 2) / Math.Pow(Constants.FrameSize, 2);
                int rowStart = 0;
                int columnStart = 0;
                int iterations = Constants.SudokuSize / Constants.FrameSize;
                for (int i = 0; i < iterations; i++)
                {
                    columnStart = 0;
                    for (int j = 0; j < iterations; j++)
                    {
                        if (!VerifyFrame(ArrayTools<Candidates>.Slice2DArray(matrix, rowStart, columnStart, Constants.FrameSize)))
                        {
                            Console.WriteLine($"Frame in {i}{j} has a repeated value");
                            return false;
                        }
                        columnStart += Constants.FrameSize;
                    }
                    rowStart += Constants.FrameSize;
                }

                return true;
            }
            catch (SudokuError)
            {
                throw;
            }
            catch
            {
                throw new SudokuError(SudokuError.ErrorType.UnknownError);
            }
        }

        internal static bool VerifyRow(Candidates[] row)
        {
            Candidates posibles = Candidates.All;
            foreach (Candidates cell in row)
            {
                if (cell == Candidates.None) throw new SudokuError(SudokuError.ErrorType.SudokuUncompleted);
                posibles &= ~cell;
            }
            return posibles == Candidates.None;
        }

        internal static bool VerifyColumn(Candidates[] column)
        {
            Candidates posibles = Candidates.All;
            foreach (Candidates cell in column)
            {
                if (cell == Candidates.None) throw new SudokuError(SudokuError.ErrorType.SudokuUncompleted);
                posibles &= ~cell;
            }
            return posibles == Candidates.None;
        }
        
        internal static bool VerifyFrame(Candidates[,] frame)
        {
            Candidates posibles = Candidates.All;
            for (int i = 0; i < Constants.FrameSize; i++)
            {
                for (int j = 0; j < Constants.FrameSize; j++)
                {
                    if (frame[i, j] == Candidates.None) throw new SudokuError(SudokuError.ErrorType.SudokuUncompleted);
                    posibles &= ~frame[i, j];
                }
            }
            return posibles == Candidates.None;
        }
    }
}