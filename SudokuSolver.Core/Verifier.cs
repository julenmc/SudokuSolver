namespace SudokuSolver.Core
{
    internal static class Verifier
    {
        internal static bool VerifySudoku(Candidates[,] matrix)
        {
            // Chek input
            if (matrix.GetLength(0) != Constants.SudokuSize) throw new SudokuError(SudokuError.ErrorType.ArrayCountError);
            if (matrix.GetLength(1) != Constants.SudokuSize) throw new SudokuError(SudokuError.ErrorType.ColumnCountError);

            try
            {
                // Check rows
                for (int i = 0; i < Constants.SudokuSize; i++)
                {
                    try
                    {
                        if (!VerifyRow(ArrayUtils<Candidates>.GetRow(matrix, i)))
                        {
                            Console.WriteLine($"Row {i} has a repeated value");
                            return false;
                        }
                    }
                    catch (SudokuError ex)
                    {
                        switch (ex.Type)
                        {
                            case SudokuError.ErrorType.SudokuUncompletedError:
                                throw new SudokuError(SudokuError.ErrorType.SudokuUncompletedError, $"Empty cell at row {i}");

                            case SudokuError.ErrorType.MultipleCandidatesError:
                                throw new SudokuError(SudokuError.ErrorType.MultipleCandidatesError, $"Multiple candidates in cell at row {i}");

                            default: throw;
                        }
                    }
                }

                // Check columns
                for (int i = 0; i < Constants.SudokuSize; i++)
                {
                    try
                    {
                        if (!VerifyColumn(ArrayUtils<Candidates>.GetColumn(matrix, i)))
                        {
                            Console.WriteLine($"Column {i} has a repeated value");
                            return false;
                        }
                    }
                    catch (SudokuError ex)
                    {
                        switch (ex.Type)
                        {
                            case SudokuError.ErrorType.SudokuUncompletedError:
                                throw new SudokuError(SudokuError.ErrorType.SudokuUncompletedError, $"Empty cell at row {i}");

                            case SudokuError.ErrorType.MultipleCandidatesError:
                                throw new SudokuError(SudokuError.ErrorType.MultipleCandidatesError, $"Multiple candidates in cell at row {i}");

                            default: throw;
                        }
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
                        try
                        {
                            if (!VerifyFrame(ArrayUtils<Candidates>.Slice2DArray(matrix, rowStart, columnStart, Constants.FrameSize)))
                            {
                                Console.WriteLine($"Frame in {i}{j} has a repeated value");
                                return false;
                            }
                            columnStart += Constants.FrameSize;
                        }
                        catch (SudokuError ex)
                        {
                            switch (ex.Type)
                            {
                                case SudokuError.ErrorType.SudokuUncompletedError:
                                    throw new SudokuError(SudokuError.ErrorType.SudokuUncompletedError, $"Empty cell at row {i}");

                                case SudokuError.ErrorType.MultipleCandidatesError:
                                    throw new SudokuError(SudokuError.ErrorType.MultipleCandidatesError, $"Multiple candidates in cell at row {i}");

                                default: throw;
                            }
                        }
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
                if (cell == Candidates.None) throw new SudokuError(SudokuError.ErrorType.SudokuUncompletedError);
                if ((cell & (cell - 1)) != 0) throw new SudokuError(SudokuError.ErrorType.MultipleCandidatesError);
                posibles &= ~cell;
            }
            return posibles == Candidates.None;
        }

        internal static bool VerifyColumn(Candidates[] column)
        {
            Candidates posibles = Candidates.All;
            foreach (Candidates cell in column)
            {
                if (cell == Candidates.None) throw new SudokuError(SudokuError.ErrorType.SudokuUncompletedError);
                if ((cell & (cell - 1)) != 0) throw new SudokuError(SudokuError.ErrorType.MultipleCandidatesError);
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
                    if (frame[i, j] == Candidates.None) throw new SudokuError(SudokuError.ErrorType.SudokuUncompletedError);
                    if ((frame[i, j] & (frame[i, j] - 1)) != 0) throw new SudokuError(SudokuError.ErrorType.MultipleCandidatesError);
                    posibles &= ~frame[i, j];
                }
            }
            return posibles == Candidates.None;
        }
    }
}