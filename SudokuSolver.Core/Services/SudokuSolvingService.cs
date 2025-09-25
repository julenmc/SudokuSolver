using System.ComponentModel;
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
            // Naked singles
            do
            {
                _changeMade = false;
                for (int i = 0; i < ConstantData.SudokuSize; i++)
                {
                    for (int j = 0; j < ConstantData.SudokuSize; j++)
                    {
                        CheckNakedSingle(i, j);
                    }
                }
                _firstCheck = false;
            } while (_changeMade);
            Console.WriteLine("Naked singles done");
            if (IsSudokuSolved()) goto Verify;

            // Hidden singles
            bool gotoHiddenPair = true;

            HiddenSingles:
            do
            {
                _changeMade = false;
                for (int i = 0; i < ConstantData.SudokuSize; i++)
                {
                    Candidates value = CandidateUtils.FromInt(i + 1);
                    CheckHiddenSingleRow(value);
                    CheckHiddenSingleColumn(value);
                    CheckHiddenSingleFrame(value);
                }
                gotoHiddenPair = _changeMade | gotoHiddenPair;
            } while (_changeMade);
            Console.WriteLine("Hidden singles done");
            if (IsSudokuSolved() || !gotoHiddenPair) goto Verify;

            // Hidden Pair/Triple
            do
            {
                _changeMade = false;
                for (int i = 0; i < ConstantData.SudokuSize; i++)
                {
                    CheckHiddenPairInRow(i);
                    CheckHiddenTripleInRow(i);
                    CheckHiddenPairInColumn(i);
                    CheckHiddenTripleInColumn(i);
                }
                for (int r = 0; r < ConstantData.SudokuSize; r += ConstantData.FrameSize)
                {
                    for (int c = 0; c < ConstantData.SudokuSize; c += ConstantData.FrameSize)
                    {
                        CheckHiddenPairFrame(r, c);
                        CheckHiddenTripleFrame(r, c);
                    }
                }
            } while (_changeMade);
            gotoHiddenPair = false;
            Console.WriteLine("Hidden pairs done");
            if (IsSudokuSolved()) goto Verify;
            else goto HiddenSingles;

            Verify:
            // Verify solution
            Console.WriteLine("Start verifing");
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
                Console.WriteLine("Unverified sudoku:");
                for (int i = 0; i < ConstantData.SudokuSize; i++)
                {
                    for (int j = 0; j < ConstantData.SudokuSize; j++)
                    {
                        Console.Write($"{_filledSudoku[i, j]} ");
                    }
                    Console.WriteLine();
                }
                throw new Exception("Sudoku verify failed!");
            }
        }

        #region NakedSingle
        private void CheckNakedSingle(int r, int c)
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
        #endregion

        #region HiddenSingle
        private void CheckHiddenSingleRow(Candidates value)
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

        private void CheckHiddenSingleColumn(Candidates value)
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

        private void CheckHiddenSingleFrame(Candidates value)
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
        #endregion

        #region HiddenPair
        private void CheckHiddenPairInRow(int r)
        {
            for (int i = 1; i < ConstantData.SudokuSize; i++)
            {
                for (int j = i + 1; j <= ConstantData.SudokuSize; j++)       // Starts from i + 1 so it doesnt repeat the combinations
                {
                    Candidates[] row = ArrayUtils<Candidates>.GetRow(_filledSudoku, r);
                    Candidates values = CandidateUtils.FromInt(i) | CandidateUtils.FromInt(j);
                    int[] res = HiddenPairTriple.Search(row, values);
                    if (res[1] != -1)
                    {
                        Console.WriteLine($"Hidden pairs ({values}) found at {r},{res[0]} and {r},{res[1]}");
                        for (int v = 1; v < ConstantData.SudokuSize + 1; v++)   // Remove the rest of the candidates from the found cells
                        {
                            Candidates candidate = CandidateUtils.FromInt(v);
                            if ((candidate & values) == 0)
                            {
                                UpdateCell(r, res[0], candidate);
                                UpdateCell(r, res[1], candidate);
                            }
                        }
                    }
                }
            }
        }

        private void CheckHiddenPairInColumn(int c)
        {
            for (int i = 1; i < ConstantData.SudokuSize; i++)
            {
                for (int j = i + 1; j <= ConstantData.SudokuSize; j++)   // Starts from i + 1 so it doesnt repeat the combinations
                {
                    Candidates[] column = ArrayUtils<Candidates>.GetColumn(_filledSudoku, c);
                    Candidates values = CandidateUtils.FromInt(i) | CandidateUtils.FromInt(j);
                    int[] res = HiddenPairTriple.Search(column, values);
                    if (res[1] != -1)
                    {
                        Console.WriteLine($"Hidden pairs ({values}) found at {res[0]},{c} and {res[1]},{c}");
                        for (int v = 1; v < ConstantData.SudokuSize + 1; v++)   // Remove the rest of the candidates from the found cells
                        {
                            Candidates candidate = CandidateUtils.FromInt(v);
                            if ((candidate & values) == 0)
                            {
                                UpdateCell(res[0], c, candidate);
                                UpdateCell(res[1], c, candidate);
                            }
                        }
                    }
                }
            }
        }

        private void CheckHiddenPairFrame(int rowStart, int columnStart)
        {
            for (int i = 1; i < ConstantData.SudokuSize; i++)
            {
                for (int j = i + 1; j <= ConstantData.SudokuSize; j++)   // Starts from i + 1 so it doesnt repeat the combinations
                {
                    Candidates[,] frame = ArrayUtils<Candidates>.Slice2DArray(_filledSudoku, rowStart, columnStart, ConstantData.FrameSize);
                    Candidates values = CandidateUtils.FromInt(i) | CandidateUtils.FromInt(j);
                    HiddenPairTriple.CellPointer[] res = HiddenPairTriple.Search(frame, values);
                    if (res[1].Row != -1)
                    {
                        Console.WriteLine($"Hidden pairs ({values}) found at {rowStart+res[0].Row},{columnStart+res[0].Col} and {rowStart+res[1].Row},{columnStart+res[1].Col}");
                        for (int v = 1; v < ConstantData.SudokuSize + 1; v++)   // Remove the rest of the candidates from the found cells
                        {
                            Candidates candidate = CandidateUtils.FromInt(v);
                            if ((candidate & values) == 0)
                            {
                                UpdateCell(res[0].Row + rowStart, res[0].Col + columnStart, candidate);
                                UpdateCell(res[1].Row + rowStart, res[1].Col + columnStart, candidate);
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region HiddenTriple
        private void CheckHiddenTripleInRow(int r)
        {
            for (int i = 1; i < ConstantData.SudokuSize; i++)
            {
                for (int j = i + 1; j < ConstantData.SudokuSize; j++)       // Starts from i + 1 so it doesnt repeat the combinations
                {
                    for (int k = j + 1; k <= ConstantData.SudokuSize; k++)
                    {
                        Candidates[] row = ArrayUtils<Candidates>.GetRow(_filledSudoku, r);
                        Candidates values = CandidateUtils.FromInt(i) | CandidateUtils.FromInt(j) | CandidateUtils.FromInt(k);
                        int[] res = HiddenPairTriple.Search(row, values);
                        if (res[1] != -1)
                        {
                            Console.WriteLine($"Hidden triples ({values}) found at {r},{res[0]} / {r},{res[1]} / {r},{res[2]}");
                            for (int v = 1; v < ConstantData.SudokuSize + 1; v++)   // Remove the rest of the candidates from the found cells
                            {
                                Candidates candidate = CandidateUtils.FromInt(v);
                                if ((candidate & values) == 0)
                                {
                                    UpdateCell(r, res[0], candidate);
                                    UpdateCell(r, res[1], candidate);
                                    UpdateCell(r, res[2], candidate);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void CheckHiddenTripleInColumn(int c)
        {
            for (int i = 1; i < ConstantData.SudokuSize; i++)
            {
                for (int j = i + 1; j < ConstantData.SudokuSize; j++)   // Starts from i + 1 so it doesnt repeat the combinations
                {
                    for (int k = j + 1; k <= ConstantData.SudokuSize; k++)
                    {
                        Candidates[] column = ArrayUtils<Candidates>.GetColumn(_filledSudoku, c);
                        Candidates values = CandidateUtils.FromInt(i) | CandidateUtils.FromInt(j) | CandidateUtils.FromInt(k);
                        int[] res = HiddenPairTriple.Search(column, values);
                        if (res[1] != -1)
                        {
                            Console.WriteLine($"Hidden pairs ({values}) found at {res[0]},{c} / {res[1]},{c} / {res[2]},{c}");
                            for (int v = 1; v < ConstantData.SudokuSize + 1; v++)   // Remove the rest of the candidates from the found cells
                            {
                                Candidates candidate = CandidateUtils.FromInt(v);
                                if ((candidate & values) == 0)
                                {
                                    UpdateCell(res[0], c, candidate);
                                    UpdateCell(res[1], c, candidate);
                                    UpdateCell(res[2], c, candidate);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void CheckHiddenTripleFrame(int rowStart, int columnStart)
        {
            for (int i = 1; i < ConstantData.SudokuSize; i++)
            {
                for (int j = i + 1; j < ConstantData.SudokuSize; j++)   // Starts from i + 1 so it doesnt repeat the combinations
                {
                    for (int k = j + 1; k <= ConstantData.SudokuSize; k++)
                    {
                        Candidates[,] frame = ArrayUtils<Candidates>.Slice2DArray(_filledSudoku, rowStart, columnStart, ConstantData.FrameSize);
                        Candidates values = CandidateUtils.FromInt(i) | CandidateUtils.FromInt(j) | CandidateUtils.FromInt(k);
                        HiddenPairTriple.CellPointer[] res = HiddenPairTriple.Search(frame, values);
                        if (res[1].Row != -1)
                        {
                            Console.WriteLine($"Hidden pairs ({values}) found at {rowStart + res[0].Row},{columnStart + res[0].Col} / {rowStart + res[1].Row},{columnStart + res[1].Col} / {rowStart + res[2].Row},{columnStart + res[2].Col}");
                            for (int v = 1; v < ConstantData.SudokuSize + 1; v++)   // Remove the rest of the candidates from the found cells
                            {
                                Candidates candidate = CandidateUtils.FromInt(v);
                                if ((candidate & values) == 0)
                                {
                                    UpdateCell(res[0].Row + rowStart, res[0].Col + columnStart, candidate);
                                    UpdateCell(res[1].Row + rowStart, res[1].Col + columnStart, candidate);
                                    UpdateCell(res[2].Row + rowStart, res[2].Col + columnStart, candidate);
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion

        private void UpdateCell(int r, int c, Candidates value)
        {
            Candidates contained = _filledSudoku[r, c] & value;
            bool singleValue = (_filledSudoku[r, c] & (_filledSudoku[r, c] - 1)) == 0;
            if (contained == Candidates.None || singleValue) return;
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