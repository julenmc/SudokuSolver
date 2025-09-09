namespace SudokuSolver.Core
{
    public class Manager
    {
        private Candidates[,] _unfilledSudoku;
        private Candidates[,] _filledSudoku;
        bool _changeMade = false;

        public Manager(int[,] sudoku)
        {
            // Chek input
            if (sudoku.GetLength(0) != Constants.SudokuSize) throw new SudokuError(SudokuError.ErrorType.RowCountError);
            if (sudoku.GetLength(1) != Constants.SudokuSize) throw new SudokuError(SudokuError.ErrorType.ColumnCountError);

            _unfilledSudoku = new Candidates[Constants.SudokuSize, Constants.SudokuSize];
            _filledSudoku = _unfilledSudoku;
            for (int i = 0; i < Constants.SudokuSize; i++)
            {
                for (int j = 0; j < Constants.SudokuSize; j++)
                {
                    _unfilledSudoku[i, j] = CandidateUtils.FromInt(sudoku[i, j]);
                }
            }
        }

        public int[,] SolveSudoku()
        {
            // First check
            for (int i = 0; i < Constants.SudokuSize; i++)
            {
                for (int j = 0; j < Constants.SudokuSize; j++)
                {
                    CheckCell(i, j);
                }
            }

            do
            {
                _changeMade = false;
                for (int i = 0; i < Constants.SudokuSize; i++)
                {
                    for (int j = 0; j < Constants.SudokuSize; j++)
                    {
                        CheckCell(i, j);
                    }
                }
            } while (_changeMade);

            // Verify solution
            if (Verifier.VerifySudoku(_filledSudoku))
            {
                Console.WriteLine("Sudoku verified!");
                int[,] solution = new int[Constants.SudokuSize, Constants.SudokuSize];
                for (int i = 0; i < Constants.SudokuSize; i++)
                {
                    for (int j = 0; j < Constants.SudokuSize; j++)
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
            int frameRowStart = Convert.ToInt32(Math.Floor((double)(r / Constants.FrameSize))) * 3;
            int frameColStart = Convert.ToInt32(Math.Floor((double)(c / Constants.FrameSize))) * 3;
            Candidates[,] frame = ArrayUtils<Candidates>.Slice2DArray(_filledSudoku, frameRowStart, frameColStart, Constants.FrameSize);
            Candidates candidates = Searcher.SearchCandidates(row, column, frame);
            if (candidates == Candidates.None) throw new SudokuError(SudokuError.ErrorType.SudokuUnsolvable, $"No candidates at row {r}, column {c}");
            _filledSudoku[r, c] = candidates;
            if ((candidates & (candidates - 1)) == 0)
            {
                Console.WriteLine($"Cell at row {r} and column {c} has the direct candidate {candidates}");
                _changeMade = true;
                UpdateCellsFromRow(r, candidates);
                UpdateCellsFromColumn(c, candidates);
                UpdateCellsFromFrame(frameRowStart, frameColStart, candidates);
            }
        }

        private void UpdateCell(int r, int c, Candidates value)
        {
            if (((_filledSudoku[r, c] & (_filledSudoku[r, c] - 1)) == 0) || _filledSudoku[r, c] == Candidates.None) return;
            _filledSudoku[r, c] &= ~value;

            if ((_filledSudoku[r, c] & (_filledSudoku[r, c] - 1)) == 0)
            {
                Console.WriteLine($"Cell at row {r} and column {c} has the direct candidate {_filledSudoku[r, c]}");
                // UpdateCellsFromRow(r, value);
                // UpdateCellsFromColumn(c, value);
                // int frameRowStart = Convert.ToInt32(Math.Floor((double)(r / Constants.FrameSize))) * 3;
                // int frameColStart = Convert.ToInt32(Math.Floor((double)(c / Constants.FrameSize))) * 3;
                // UpdateCellsFromFrame(frameRowStart, frameColStart, value);
            }
        }

        private void UpdateCellsFromRow(int r, Candidates value)
        {
            for (int i = 0; i < Constants.SudokuSize; i++)
            {
                UpdateCell(r, i, value);
            }
        }

        private void UpdateCellsFromColumn(int c, Candidates value)
        {
            for (int i = 0; i < Constants.SudokuSize; i++)
            {
                UpdateCell(i, c, value);
            }
        }

        private void UpdateCellsFromFrame(int startRow, int startColumn, Candidates value)
        {
            for (int i = startRow; i < startRow + Constants.FrameSize; i++)
            {
                for (int j = startColumn; j < startColumn + Constants.FrameSize; j++)
                {
                    UpdateCell(i, j, value);
                }
            }
        }
    }
}