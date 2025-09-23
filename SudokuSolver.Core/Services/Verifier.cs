using SudokuSolver.Core.Constants;
using SudokuSolver.Core.Enums;
using SudokuSolver.Core.Models;

namespace SudokuSolver.Core.Services
{
    internal static class Verifier
    {
        internal static bool VerifySudoku(Candidates[,] matrix)
        {
            // Chek input
            if (matrix.GetLength(0) != ConstantData.SudokuSize) throw new SudokuError(SudokuError.ErrorType.ArrayCountError);
            if (matrix.GetLength(1) != ConstantData.SudokuSize) throw new SudokuError(SudokuError.ErrorType.ArrayCountError);

            try
            {
                // Check rows
                for (int i = 0; i < ConstantData.SudokuSize; i++)
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
                for (int i = 0; i < ConstantData.SudokuSize; i++)
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
                double frameNum = Math.Pow(ConstantData.SudokuSize, 2) / Math.Pow(ConstantData.FrameSize, 2);
                int rowStart = 0;
                int columnStart = 0;
                int iterations = ConstantData.SudokuSize / ConstantData.FrameSize;
                for (int i = 0; i < iterations; i++)
                {
                    columnStart = 0;
                    for (int j = 0; j < iterations; j++)
                    {
                        try
                        {
                            if (!VerifyFrame(ArrayUtils<Candidates>.Slice2DArray(matrix, rowStart, columnStart, ConstantData.FrameSize)))
                            {
                                Console.WriteLine($"Frame in {i}{j} has a repeated value");
                                return false;
                            }
                            columnStart += ConstantData.FrameSize;
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
                    rowStart += ConstantData.FrameSize;
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
            for (int i = 0; i < ConstantData.FrameSize; i++)
            {
                for (int j = 0; j < ConstantData.FrameSize; j++)
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