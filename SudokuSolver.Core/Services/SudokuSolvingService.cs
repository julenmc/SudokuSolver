using SudokuSolver.Core.Constants;
using SudokuSolver.Core.Enums;
using SudokuSolver.Core.Models;

namespace SudokuSolver.Core.Services
{
    public class SudokuSolvingService
    {
        private Candidates[,] _unfilledSudoku;
        private Candidates[,] _filledSudoku;
        bool _changeMade = false;
        bool _firstCheck = true;

        public SudokuSolvingService(int[,] sudoku)
        {
            // Chek input
            if (sudoku.GetLength(0) != ConstantData.SudokuSize) throw new SudokuError(SudokuError.ErrorType.ArrayCountError);
            if (sudoku.GetLength(1) != ConstantData.SudokuSize) throw new SudokuError(SudokuError.ErrorType.ArrayCountError);

            _unfilledSudoku = new Candidates[ConstantData.SudokuSize, ConstantData.SudokuSize];
            _filledSudoku = _unfilledSudoku;
            for (int i = 0; i < ConstantData.SudokuSize; i++)
            {
                for (int j = 0; j < ConstantData.SudokuSize; j++)
                {
                    _unfilledSudoku[i, j] = CandidateUtils.FromInt(sudoku[i, j]);
                }
            }
        }

        public int[,] SolveSudoku()
        {
            // Direct check
            do
            {
                _changeMade = false;
                for (int i = 0; i < ConstantData.SudokuSize; i++)
                {
                    for (int j = 0; j < ConstantData.SudokuSize; j++)
                    {
                        CheckCell(i, j);
                    }
                }
                _firstCheck = false;
            } while (_changeMade);
            Console.WriteLine("Direct checks done");
            if (IsSudokuSolved()) goto verify;

            // Check numbers
            do
            {
                _changeMade = false;
                for (int i = 0; i < ConstantData.SudokuSize; i++)
                {
                    Candidates value = CandidateUtils.FromInt(i + 1);
                    CheckNumberByRow(value);
                    CheckNumberByColumn(value);
                    CheckNumberByFrame(value);
                }
            } while (_changeMade);
            Console.WriteLine("Number checks done");
            if (IsSudokuSolved()) goto verify;

            // Hidden Pair/Triple

            verify:
            // Verify solution
            if (Verifier.VerifySudoku(_filledSudoku))
            {
                Console.WriteLine("Sudoku verified!");
                int[,] solution = new int[ConstantData.SudokuSize, ConstantData.SudokuSize];
                for (int i = 0; i < ConstantData.SudokuSize; i++)
                {
                    for (int j = 0; j < ConstantData.SudokuSize; j++)
                    {
                        solution[i, j] = CandidateUtils.ToInt(_filledSudoku[i, j]);
                    }
                }

                return solution;
            }
            else
            {
                throw new Exception("Sudoku verify failed!");
            }
        }

        private void CheckCell(int r, int c)
        {
            if ((_filledSudoku[r, c] != Candidates.None) && (_filledSudoku[r, c] & (_filledSudoku[r, c] - 1)) == 0) return;   // If already filled go to next

            Candidates[] row = ArrayUtils<Candidates>.GetRow(_filledSudoku, r);
            Candidates[] column = ArrayUtils<Candidates>.GetColumn(_filledSudoku, c);
            int frameRowStart = Convert.ToInt32(Math.Floor((double)(r / ConstantData.FrameSize))) * ConstantData.FrameSize;
            int frameColStart = Convert.ToInt32(Math.Floor((double)(c / ConstantData.FrameSize))) * ConstantData.FrameSize;
            Candidates[,] frame = ArrayUtils<Candidates>.Slice2DArray(_filledSudoku, frameRowStart, frameColStart, ConstantData.FrameSize);
            Candidates candidates = NakedSingles.Search(row, column, frame);
            if (candidates == Candidates.None) throw new SudokuError(SudokuError.ErrorType.SudokuUnsolvable, $"No candidates at row {r}, column {c}");
            _filledSudoku[r, c] = candidates;
            if ((candidates & (candidates - 1)) == 0)
            {
                Console.WriteLine($"Value {_filledSudoku[r, c]} found in {r},{c} while checking direct values");
                _changeMade = true;
                UpdateCellsFromRow(r, candidates);
                UpdateCellsFromColumn(c, candidates);
                UpdateCellsFromFrame(frameRowStart, frameColStart, candidates);
            }
            else if (_firstCheck)
            {
                Console.WriteLine($"Candidates at {r},{c}: {_filledSudoku[r, c]}");
            }
        }

        private void CheckNumberByRow(Candidates value)
        {
            for (int i = 0; i < ConstantData.SudokuSize; i++)
            {
                Candidates[] row = ArrayUtils<Candidates>.GetRow(_filledSudoku, i);
                int res = HiddenSingles.Search(row, value);
                if (res != -1)
                {
                    _filledSudoku[i, res] = value;
                    Console.WriteLine($"Value {value} found in {i},{res} while checking row {i}");
                    _changeMade = true;
                    UpdateCellsFromRow(i, value);
                    UpdateCellsFromColumn(res, value);
                    int frameRowStart = Convert.ToInt32(Math.Floor((double)(i / ConstantData.FrameSize))) * ConstantData.FrameSize;
                    int frameColStart = Convert.ToInt32(Math.Floor((double)(res / ConstantData.FrameSize))) * ConstantData.FrameSize;
                    UpdateCellsFromFrame(frameRowStart, frameColStart, value);
                }
            }
        }

        private void CheckNumberByColumn(Candidates value)
        {
            for (int i = 0; i < ConstantData.SudokuSize; i++)
            {
                Candidates[] column = ArrayUtils<Candidates>.GetColumn(_filledSudoku, i);
                int res = HiddenSingles.Search(column, value);
                if (res != -1)
                {
                    _filledSudoku[res, i] = value;
                    Console.WriteLine($"Value {value} found in {res},{i} while checking column {i}");
                    _changeMade = true;
                    UpdateCellsFromRow(res, value);
                    UpdateCellsFromColumn(i, value);
                    int frameRowStart = Convert.ToInt32(Math.Floor((double)(res / ConstantData.FrameSize))) * ConstantData.FrameSize;
                    int frameColStart = Convert.ToInt32(Math.Floor((double)(i / ConstantData.FrameSize))) * ConstantData.FrameSize;
                    UpdateCellsFromFrame(frameRowStart, frameColStart, value);
                }
            }
        }

        private void CheckNumberByFrame(Candidates value)
        {
            for (int i = 0; i < ConstantData.SudokuSize; i += ConstantData.FrameSize)
            {
                for (int j = 0; j < ConstantData.SudokuSize; j += ConstantData.FrameSize)
                {
                    Candidates[,] frame = ArrayUtils<Candidates>.Slice2DArray(_filledSudoku, i, j, ConstantData.FrameSize);
                    var res = HiddenSingles.Search(frame, value);
                    if (res.Item1 != -1)
                    {
                        _filledSudoku[res.Item1 + i, res.Item2 + j] = value;
                        Console.WriteLine($"Value {value} found in {res.Item1 + i},{res.Item2 + j} while checking frame ({i}, {j})");
                        _changeMade = true;
                        UpdateCellsFromRow(res.Item1 + i, value);
                        UpdateCellsFromColumn(res.Item2 + j, value);
                        UpdateCellsFromFrame(i, j, value);
                    }
                }
            }
        }

        private void UpdateCell(int r, int c, Candidates value)
        {
            if (((_filledSudoku[r, c] & value) == 0) || (_filledSudoku[r, c] == value)) return;
            _filledSudoku[r, c] &= ~value;

            if ((_filledSudoku[r, c] & (_filledSudoku[r, c] - 1)) == 0)
            {
                Console.WriteLine($"Value {_filledSudoku[r, c]} found in {r},{c} while updating");
                UpdateCellsFromRow(r, _filledSudoku[r, c]);
                UpdateCellsFromColumn(c, _filledSudoku[r, c]);
                int frameRowStart = Convert.ToInt32(Math.Floor((double)(r / ConstantData.FrameSize))) * 3;
                int frameColStart = Convert.ToInt32(Math.Floor((double)(c / ConstantData.FrameSize))) * 3;
                UpdateCellsFromFrame(frameRowStart, frameColStart, _filledSudoku[r, c]);
            }
            else
            {
                Console.WriteLine($"Candidates at {r},{c}: {_filledSudoku[r, c]}");
            }
        }

        private void UpdateCellsFromRow(int r, Candidates value)
        {
            for (int i = 0; i < ConstantData.SudokuSize; i++)
            {
                UpdateCell(r, i, value);
            }
        }

        private void UpdateCellsFromColumn(int c, Candidates value)
        {
            for (int i = 0; i < ConstantData.SudokuSize; i++)
            {
                UpdateCell(i, c, value);
            }
        }

        private void UpdateCellsFromFrame(int startRow, int startColumn, Candidates value)
        {
            for (int i = startRow; i < startRow + ConstantData.FrameSize; i++)
            {
                for (int j = startColumn; j < startColumn + ConstantData.FrameSize; j++)
                {
                    UpdateCell(i, j, value);
                }
            }
        }

        private bool IsCellSolved(Candidates cell)
        {
            return cell != Candidates.None && (cell & (cell - 1)) == 0;
        }

        private bool IsSudokuSolved()
        {
            for (int r = 0; r < ConstantData.SudokuSize; r++)
            {
                for (int c = 0; c < ConstantData.SudokuSize; c++)
                {
                    if (!IsCellSolved(_filledSudoku[r, c]))
                        return false;
                }
            }
            return true;
        }
    }
}