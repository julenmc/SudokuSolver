using SudokuSolver.Core;

namespace SudokuSolver.Test
{
    [TestClass]
    public sealed class VerifierTest
    {
        private readonly Candidates[,] FilledSudoku =
        {
            { Candidates.One, Candidates.Two, Candidates.Three, Candidates.Four, Candidates.Five, Candidates.Six, Candidates.Seven, Candidates.Eight, Candidates.Nine },
            { Candidates.Four, Candidates.Five, Candidates.Six, Candidates.Seven, Candidates.Eight, Candidates.Nine, Candidates.One, Candidates.Two, Candidates.Three },
            { Candidates.Seven, Candidates.Eight, Candidates.Nine, Candidates.One, Candidates.Two, Candidates.Three, Candidates.Four, Candidates.Five, Candidates.Six },
            { Candidates.Two, Candidates.Three, Candidates.Four, Candidates.Five, Candidates.Six, Candidates.Seven, Candidates.Eight, Candidates.Nine, Candidates.One },
            { Candidates.Five, Candidates.Six, Candidates.Seven, Candidates.Eight, Candidates.Nine, Candidates.One, Candidates.Two, Candidates.Three, Candidates.Four },
            { Candidates.Eight, Candidates.Nine, Candidates.One, Candidates.Two, Candidates.Three, Candidates.Four, Candidates.Five, Candidates.Six, Candidates.Seven },
            { Candidates.Three, Candidates.Four, Candidates.Five, Candidates.Six, Candidates.Seven, Candidates.Eight, Candidates.Nine, Candidates.One, Candidates.Two },
            { Candidates.Six, Candidates.Seven, Candidates.Eight, Candidates.Nine, Candidates.One, Candidates.Two, Candidates.Three, Candidates.Four, Candidates.Five },
            { Candidates.Nine, Candidates.One, Candidates.Two, Candidates.Three, Candidates.Four, Candidates.Five, Candidates.Six, Candidates.Seven, Candidates.Eight }
        };

        [TestMethod]
        public void OkSudoku()
        {
            Assert.IsTrue(Verifier.VerifySudoku(FilledSudoku));
        }

        [TestMethod]
        public void EmptyCell()
        {
            Candidates[,] sudoku = FilledSudoku;
            sudoku[0, 0] = Candidates.None;
            try
            {
                Verifier.VerifySudoku(sudoku);
                Assert.Fail();
            }
            catch (SudokuError ex)
            {
                if (ex.Type == SudokuError.ErrorType.SudokuUncompletedError) Console.WriteLine($"OK. Error: {ex.Type}");
                else Assert.Fail();
            }
        }

        [TestMethod]
        public void MultipleCandidates()
        {
            Candidates[,] sudoku = FilledSudoku;
            sudoku[0, 0] = Candidates.One | Candidates.Two;
            try
            {
                Verifier.VerifySudoku(sudoku);
                Assert.Fail();
            }
            catch (SudokuError ex)
            {
                if (ex.Type == SudokuError.ErrorType.MultipleCandidatesError) Console.WriteLine($"OK. Error: {ex.Type}");
                else Assert.Fail();
            }
        }

        [TestMethod]
        public void DuplicateInRow()
        {
            Candidates[,] sudoku = FilledSudoku;
            sudoku[0, 1] = Candidates.One;
            Assert.IsFalse(Verifier.VerifyRow(ArrayUtils<Candidates>.GetRow(sudoku, 0)));
        }

        [TestMethod]
        public void DuplicateInColumn()
        {
            Candidates[,] sudoku = FilledSudoku;
            sudoku[1, 0] = Candidates.One;
            Assert.IsFalse(Verifier.VerifyColumn(ArrayUtils<Candidates>.GetColumn(sudoku, 0)));
        }

        [TestMethod]
        public void DuplicateInFrame()
        {
            Candidates[,] sudoku = FilledSudoku;
            sudoku[1, 1] = Candidates.One;
            Assert.IsFalse(Verifier.VerifyFrame(ArrayUtils<Candidates>.Slice2DArray(sudoku, 0, 0, Constants.FrameSize)));
        }

        #region InputError

        [TestMethod]
        public void RowCountError()
        {
            Candidates[,] sudoku =
            {
                { Candidates.One, Candidates.Two, Candidates.Three, Candidates.Four, Candidates.Five, Candidates.Six, Candidates.Seven, Candidates.Eight, Candidates.Nine },
                { Candidates.Four, Candidates.Five, Candidates.Six, Candidates.Seven, Candidates.Eight, Candidates.Nine, Candidates.One, Candidates.Two, Candidates.Three },
                { Candidates.Seven, Candidates.Eight, Candidates.Nine, Candidates.One, Candidates.Two, Candidates.Three, Candidates.Four, Candidates.Five, Candidates.Six },
                { Candidates.Two, Candidates.Three, Candidates.Four, Candidates.Five, Candidates.Six, Candidates.Seven, Candidates.Eight, Candidates.Nine, Candidates.One },
                { Candidates.Five, Candidates.Six, Candidates.Seven, Candidates.Eight, Candidates.Nine, Candidates.One, Candidates.Two, Candidates.Three, Candidates.Four },
                { Candidates.Eight, Candidates.Nine, Candidates.One, Candidates.Two, Candidates.Three, Candidates.Four, Candidates.Five, Candidates.Six, Candidates.Seven },
                { Candidates.Three, Candidates.Four, Candidates.Five, Candidates.Six, Candidates.Seven, Candidates.Eight, Candidates.Nine, Candidates.One, Candidates.Two },
                { Candidates.Six, Candidates.Seven, Candidates.Eight, Candidates.Nine, Candidates.One, Candidates.Two, Candidates.Three, Candidates.Four, Candidates.Five },
            };
            try
            {
                Verifier.VerifySudoku(sudoku);
                Assert.Fail();
            }
            catch (SudokuError ex)
            {
                if (ex.Type == SudokuError.ErrorType.ArrayCountError) Console.WriteLine($"OK. Error: {ex.Type}");
                else Assert.Fail();
            }
        }

        [TestMethod]
        public void ColumnCountError()
        {
            Candidates[,] sudoku =
        {
            { Candidates.One, Candidates.Two, Candidates.Three, Candidates.Four, Candidates.Five, Candidates.Six, Candidates.Seven, Candidates.Eight },
            { Candidates.Four, Candidates.Five, Candidates.Six, Candidates.Seven, Candidates.Eight, Candidates.Nine, Candidates.One, Candidates.Two },
            { Candidates.Seven, Candidates.Eight, Candidates.Nine, Candidates.One, Candidates.Two, Candidates.Three, Candidates.Four, Candidates.Five },
            { Candidates.Two, Candidates.Three, Candidates.Four, Candidates.Five, Candidates.Six, Candidates.Seven, Candidates.Eight, Candidates.Nine },
            { Candidates.Five, Candidates.Six, Candidates.Seven, Candidates.Eight, Candidates.Nine, Candidates.One, Candidates.Two, Candidates.Three },
            { Candidates.Eight, Candidates.Nine, Candidates.One, Candidates.Two, Candidates.Three, Candidates.Four, Candidates.Five, Candidates.Six },
            { Candidates.Three, Candidates.Four, Candidates.Five, Candidates.Six, Candidates.Seven, Candidates.Eight, Candidates.Nine, Candidates.One },
            { Candidates.Six, Candidates.Seven, Candidates.Eight, Candidates.Nine, Candidates.One, Candidates.Two, Candidates.Three, Candidates.Four },
            { Candidates.Nine, Candidates.One, Candidates.Two, Candidates.Three, Candidates.Four, Candidates.Five, Candidates.Six, Candidates.Seven }
        };
            try
            {
                Verifier.VerifySudoku(sudoku);
                Assert.Fail();
            }
            catch (SudokuError ex)
            {
                if (ex.Type == SudokuError.ErrorType.ColumnCountError) Console.WriteLine($"OK. Error: {ex.Type}");
                else Assert.Fail();
            }
        }


        #endregion
    }
}