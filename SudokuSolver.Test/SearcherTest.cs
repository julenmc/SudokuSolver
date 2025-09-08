using SudokuSolver.Core;

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
            Assert.AreEqual(Candidates.Nine, Searcher.SearchCandidates(row, column, frame));
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
            Assert.AreEqual(Candidates.Nine, Searcher.SearchCandidates(row, column, frame));
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
            Assert.AreEqual(Candidates.Nine, Searcher.SearchCandidates(row, column, frame));
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

            Assert.AreEqual(Candidates.Nine, Searcher.SearchCandidates(row, column, frame));
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

            Assert.AreEqual(Candidates.Nine, Searcher.SearchCandidates(row, column, frame));
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

            Assert.AreEqual(Candidates.Nine, Searcher.SearchCandidates(row, column, frame));
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

            Assert.AreEqual(Candidates.Nine, Searcher.SearchCandidates(row, column, frame));
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
                Searcher.SearchCandidates(row, column, frame);
                Assert.Fail();
            }
            catch (Searcher.SearcherError ex)
            {
                if (ex.Type == Searcher.SearcherError.ErrorType.RowCountError) Console.WriteLine($"OK. Error: {ex.Type}");
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
                Searcher.SearchCandidates(row, column, frame);
                Assert.Fail();
            }
            catch (Searcher.SearcherError ex)
            {
                if (ex.Type == Searcher.SearcherError.ErrorType.ColumnCountError) Console.WriteLine($"OK. Error: {ex.Type}");
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
                Searcher.SearchCandidates(row, column, frame);
                Assert.Fail();
            }
            catch (Searcher.SearcherError ex)
            {
                if (ex.Type == Searcher.SearcherError.ErrorType.FrameRowCountError) Console.WriteLine($"OK. Error: {ex.Type}");
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
                Searcher.SearchCandidates(row, column, frame);
                Assert.Fail();
            }
            catch (Searcher.SearcherError ex)
            {
                if (ex.Type == Searcher.SearcherError.ErrorType.FrameColumnCountError) Console.WriteLine($"OK. Error: {ex.Type}");
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
            Assert.AreEqual(Candidates.Nine | Candidates.Eight, Searcher.SearchCandidates(row, column, frame));
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
            Assert.AreEqual(Candidates.Nine | Candidates.Eight, Searcher.SearchCandidates(row, column, frame));
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
            Assert.AreEqual(Candidates.Nine | Candidates.Eight, Searcher.SearchCandidates(row, column, frame));
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

            Assert.AreEqual(Candidates.Nine | Candidates.Eight, Searcher.SearchCandidates(row, column, frame));
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

            Assert.AreEqual(Candidates.Nine | Candidates.Eight, Searcher.SearchCandidates(row, column, frame));
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

            Assert.AreEqual(Candidates.Nine | Candidates.Eight, Searcher.SearchCandidates(row, column, frame));
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

            Assert.AreEqual(Candidates.Nine | Candidates.Eight, Searcher.SearchCandidates(row, column, frame));
        }
    }
}