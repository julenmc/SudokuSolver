using SudokuSolver.Core.Constants;
using SudokuSolver.Core.Enums;
using SudokuSolver.Core.Models;
using SudokuSolver.Core.Services;

namespace SudokuSolver.Test
{
    [TestClass]
    public sealed class SearcherTestSingle
    {
        [TestMethod]
        public void OnlyRow()
        {
            Candidates[] row = { Candidates.None, Candidates.One, Candidates.Two, Candidates.Three, Candidates.Four, Candidates.Five, Candidates.Six, Candidates.Seven, Candidates.Eight };
            Candidates[] column = { Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None };
            Candidates[,] frame = {
                { Candidates.None, Candidates.None, Candidates.None },
                { Candidates.None, Candidates.None, Candidates.None },
                { Candidates.None, Candidates.None, Candidates.None },
            };
            Assert.AreEqual(Candidates.Nine, NakedSingles.Search(row, column, frame));
        }

        [TestMethod]
        public void OnlyColumn()
        {
            Candidates[] row = { Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None };
            Candidates[] column = { Candidates.None, Candidates.One, Candidates.Two, Candidates.Three, Candidates.Four, Candidates.Five, Candidates.Six, Candidates.Seven, Candidates.Eight };
            Candidates[,] frame = {
                { Candidates.None, Candidates.None, Candidates.None },
                { Candidates.None, Candidates.None, Candidates.None },
                { Candidates.None, Candidates.None, Candidates.None },
            };
            Assert.AreEqual(Candidates.Nine, NakedSingles.Search(row, column, frame));
        }

        [TestMethod]
        public void OnlyFrame()
        {
            Candidates[] row = { Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None };
            Candidates[] column = { Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None };
            Candidates[,] frame = {
                { Candidates.None, Candidates.One, Candidates.Two },
                { Candidates.Three, Candidates.Four, Candidates.Five },
                { Candidates.Six, Candidates.Seven, Candidates.Eight },
            };
            Assert.AreEqual(Candidates.Nine, NakedSingles.Search(row, column, frame));
        }

        [TestMethod]
        public void RowAndColumn()
        {
            Candidates[] row = { Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.Eight };
            Candidates[] column = { Candidates.None, Candidates.One, Candidates.Two, Candidates.Three, Candidates.Four, Candidates.Five, Candidates.Six, Candidates.Seven, Candidates.None };
            Candidates[,] frame = {
                { Candidates.None, Candidates.None, Candidates.None },
                { Candidates.None, Candidates.None, Candidates.None },
                { Candidates.None, Candidates.None, Candidates.None },
            };

            Assert.AreEqual(Candidates.Nine, NakedSingles.Search(row, column, frame));
        }

        [TestMethod]
        public void RowAndFrame()
        {
            Candidates[] row = { Candidates.None, Candidates.One, Candidates.Two, Candidates.Three, Candidates.Four, Candidates.Five, Candidates.Six, Candidates.Seven, Candidates.None };
            Candidates[] column = { Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None };
            Candidates[,] frame = {
                { Candidates.None, Candidates.None, Candidates.None },
                { Candidates.None, Candidates.None, Candidates.None },
                { Candidates.None, Candidates.None, Candidates.Eight },
            };

            Assert.AreEqual(Candidates.Nine, NakedSingles.Search(row, column, frame));
        }

        [TestMethod]
        public void ColumnAndFrame()
        {
            Candidates[] row = { Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None };
            Candidates[] column = { Candidates.None, Candidates.One, Candidates.Two, Candidates.Three, Candidates.Four, Candidates.Five, Candidates.Six, Candidates.Seven, Candidates.None };
            Candidates[,] frame = {
                { Candidates.None, Candidates.None, Candidates.None },
                { Candidates.None, Candidates.None, Candidates.None },
                { Candidates.None, Candidates.None, Candidates.Eight },
            };

            Assert.AreEqual(Candidates.Nine, NakedSingles.Search(row, column, frame));
        }

        [TestMethod]
        public void AllThree()
        {
            Candidates[] row = { Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.Seven };
            Candidates[] column = { Candidates.None, Candidates.One, Candidates.Two, Candidates.Three, Candidates.Four, Candidates.Five, Candidates.Six, Candidates.None, Candidates.None };
            Candidates[,] frame = {
                { Candidates.None, Candidates.None, Candidates.None },
                { Candidates.None, Candidates.None, Candidates.None },
                { Candidates.None, Candidates.None, Candidates.Eight },
            };

            Assert.AreEqual(Candidates.Nine, NakedSingles.Search(row, column, frame));
        }

        #region InputError

        [TestMethod]
        public void RowCountError()
        {
            Candidates[] row = { Candidates.None, Candidates.One, Candidates.Two, Candidates.Three, Candidates.Four, Candidates.Five, Candidates.Six, Candidates.Seven };
            Candidates[] column = { Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None };
            Candidates[,] frame = {
                { Candidates.None, Candidates.None, Candidates.None },
                { Candidates.None, Candidates.None, Candidates.None },
                { Candidates.None, Candidates.None, Candidates.None },
            };
            try
            {
                NakedSingles.Search(row, column, frame);
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
            Candidates[] row = { Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None };
            Candidates[] column = { Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None };
            Candidates[,] frame = {
                { Candidates.None, Candidates.None, Candidates.None },
                { Candidates.None, Candidates.None, Candidates.None },
                { Candidates.None, Candidates.None, Candidates.None },
            };
            try
            {
                NakedSingles.Search(row, column, frame);
                Assert.Fail();
            }
            catch (SudokuError ex)
            {
                if (ex.Type == SudokuError.ErrorType.ArrayCountError) Console.WriteLine($"OK. Error: {ex.Type}");
                else Assert.Fail();
            }
        }

        [TestMethod]
        public void FrameRowCountError()
        {
            Candidates[] row = { Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None };
            Candidates[] column = { Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None };
            Candidates[,] frame = {
                { Candidates.None, Candidates.None, Candidates.None },
                { Candidates.None, Candidates.None, Candidates.None },
            };
            try
            {
                NakedSingles.Search(row, column, frame);
                Assert.Fail();
            }
            catch (SudokuError ex)
            {
                if (ex.Type == SudokuError.ErrorType.FrameRowCountError) Console.WriteLine($"OK. Error: {ex.Type}");
                else Assert.Fail();
            }
        }

        [TestMethod]
        public void FrameColumnCountError()
        {
            Candidates[] row = { Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None };
            Candidates[] column = { Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None };
            Candidates[,] frame = {
                { Candidates.None, Candidates.None },
                { Candidates.None, Candidates.None },
                { Candidates.None, Candidates.None },
            };
            try
            {
                NakedSingles.Search(row, column, frame);
                Assert.Fail();
            }
            catch (SudokuError ex)
            {
                if (ex.Type == SudokuError.ErrorType.FrameColumnCountError) Console.WriteLine($"OK. Error: {ex.Type}");
                else Assert.Fail();
            }
        }

        #endregion
    }

    [TestClass]
    public sealed class SearcherTestMultiple
    {
        [TestMethod]
        public void OnlyRow()
        {
            Candidates[] row = { Candidates.None, Candidates.One, Candidates.Two, Candidates.Three, Candidates.Four, Candidates.Five, Candidates.Six, Candidates.Seven, Candidates.None };
            Candidates[] column = { Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None };
            Candidates[,] frame = {
                { Candidates.None, Candidates.None, Candidates.None },
                { Candidates.None, Candidates.None, Candidates.None },
                { Candidates.None, Candidates.None, Candidates.None },
            };
            Assert.AreEqual(Candidates.Nine | Candidates.Eight, NakedSingles.Search(row, column, frame));
        }

        [TestMethod]
        public void OnlyColumn()
        {
            Candidates[] row = { Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None };
            Candidates[] column = { Candidates.None, Candidates.One, Candidates.Two, Candidates.Three, Candidates.Four, Candidates.Five, Candidates.Six, Candidates.Seven, Candidates.None };
            Candidates[,] frame = {
                { Candidates.None, Candidates.None, Candidates.None },
                { Candidates.None, Candidates.None, Candidates.None },
                { Candidates.None, Candidates.None, Candidates.None },
            };
            Assert.AreEqual(Candidates.Nine | Candidates.Eight, NakedSingles.Search(row, column, frame));
        }

        [TestMethod]
        public void OnlyFrame()
        {
            Candidates[] row = { Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None };
            Candidates[] column = { Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None };
            Candidates[,] frame = {
                { Candidates.None, Candidates.One, Candidates.Two },
                { Candidates.Three, Candidates.Four, Candidates.Five },
                { Candidates.Six, Candidates.Seven, Candidates.None },
            };
            Assert.AreEqual(Candidates.Nine | Candidates.Eight, NakedSingles.Search(row, column, frame));
        }

        [TestMethod]
        public void RowAndColumn()
        {
            Candidates[] row = { Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.Seven };
            Candidates[] column = { Candidates.None, Candidates.One, Candidates.Two, Candidates.Three, Candidates.Four, Candidates.Five, Candidates.Six, Candidates.None, Candidates.None };
            Candidates[,] frame = {
                { Candidates.None, Candidates.None, Candidates.None },
                { Candidates.None, Candidates.None, Candidates.None },
                { Candidates.None, Candidates.None, Candidates.None },
            };

            Assert.AreEqual(Candidates.Nine | Candidates.Eight, NakedSingles.Search(row, column, frame));
        }

        [TestMethod]
        public void RowAndFrame()
        {
            Candidates[] row = { Candidates.None, Candidates.One, Candidates.Two, Candidates.Three, Candidates.Four, Candidates.Five, Candidates.Six, Candidates.None, Candidates.None };
            Candidates[] column = { Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None };
            Candidates[,] frame = {
                { Candidates.None, Candidates.None, Candidates.None },
                { Candidates.None, Candidates.None, Candidates.None },
                { Candidates.None, Candidates.None, Candidates.Seven },
            };

            Assert.AreEqual(Candidates.Nine | Candidates.Eight, NakedSingles.Search(row, column, frame));
        }

        [TestMethod]
        public void ColumnAndFrame()
        {
            Candidates[] row = { Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None };
            Candidates[] column = { Candidates.None, Candidates.One, Candidates.Two, Candidates.Three, Candidates.Four, Candidates.Five, Candidates.Six, Candidates.None, Candidates.None };
            Candidates[,] frame = {
                { Candidates.None, Candidates.None, Candidates.None },
                { Candidates.None, Candidates.None, Candidates.None },
                { Candidates.None, Candidates.None, Candidates.Seven },
            };

            Assert.AreEqual(Candidates.Nine | Candidates.Eight, NakedSingles.Search(row, column, frame));
        }

        [TestMethod]
        public void AllThree()
        {
            Candidates[] row = { Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.None, Candidates.Six };
            Candidates[] column = { Candidates.None, Candidates.One, Candidates.Two, Candidates.Three, Candidates.Four, Candidates.Five, Candidates.None, Candidates.None, Candidates.None };
            Candidates[,] frame = {
                { Candidates.None, Candidates.None, Candidates.None },
                { Candidates.None, Candidates.None, Candidates.None },
                { Candidates.None, Candidates.None, Candidates.Seven },
            };

            Assert.AreEqual(Candidates.Nine | Candidates.Eight, NakedSingles.Search(row, column, frame));
        }
    }

    [TestClass]
    public sealed class SearcherNumberCellValue
    {
        [TestMethod]
        public void Array()
        {
            Candidates[] array = {
                Candidates.One | Candidates.Two,
                Candidates.Two | Candidates.Three,
                Candidates.Three | Candidates.Four,
                Candidates.Four | Candidates.Five,
                Candidates.Five | Candidates.Six,
                Candidates.Six | Candidates.Seven,
                Candidates.Seven | Candidates.Eight,
                Candidates.Eight | Candidates.Nine,
                Candidates.Eight | Candidates.Nine
            };
            Assert.AreEqual(0, HiddenSingles.Search(array, Candidates.One));
        }

        [TestMethod]
        public void Frame()
        {
            Candidates[,] frame = {
                { Candidates.One | Candidates.Two, Candidates.Two | Candidates.Three, Candidates.Three | Candidates.Four },
                { Candidates.Four | Candidates.Five, Candidates.Five | Candidates.Six, Candidates.Six | Candidates.Seven },
                { Candidates.Seven | Candidates.Eight, Candidates.Eight | Candidates.Nine, Candidates.Eight | Candidates.Nine }
            };
            var res = HiddenSingles.Search(frame, Candidates.One);
            Assert.AreEqual(0, res.Item1);
            Assert.AreEqual(0, res.Item2);
        }

        [TestMethod]
        public void AlreadyAssigned()
        {
            Candidates[] array = {
                Candidates.One,
                Candidates.Two | Candidates.Three,
                Candidates.Three | Candidates.Four,
                Candidates.Four | Candidates.Five,
                Candidates.Five | Candidates.Six,
                Candidates.Six | Candidates.Seven,
                Candidates.Seven | Candidates.Eight,
                Candidates.Eight | Candidates.Nine,
                Candidates.Eight | Candidates.Nine
            };
            Assert.AreEqual(-1, HiddenSingles.Search(array, Candidates.One));
        }
    }
}