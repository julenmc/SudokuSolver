using SudokuSolver.Core.Enums;
using SudokuSolver.Core.Services;

using static SudokuSolver.Core.Services.HiddenPairTriple;

namespace SudokuSolver.Test
{
    [TestClass]
    public sealed class HiddenPairTest
    {
        #region PairArray

        [TestMethod]
        public void PairInArray()
        {
            Candidates[] array = {
                Candidates.One,
                Candidates.Two | Candidates.Three,
                Candidates.Two | Candidates.Three | Candidates.Four,
                Candidates.Four | Candidates.Five,
                Candidates.Five | Candidates.Six,
                Candidates.Six | Candidates.Seven,
                Candidates.Seven | Candidates.Eight,
                Candidates.Eight | Candidates.Nine,
                Candidates.Eight | Candidates.Nine
            };
            Candidates values = Candidates.Two | Candidates.Three;
            int[] ret = HiddenPairTriple.Search(array, values);
            Assert.AreEqual(2, ret.Length);
            CollectionAssert.AreEqual(new int[] { 1, 2 }, ret);
        }

        [TestMethod]
        public void NoPairInArray()
        {
            Candidates[] array = {
                Candidates.One,
                Candidates.Two | Candidates.Three,
                Candidates.Four,
                Candidates.Four | Candidates.Five,
                Candidates.Five | Candidates.Six,
                Candidates.Six | Candidates.Seven,
                Candidates.Seven | Candidates.Eight,
                Candidates.Eight | Candidates.Nine,
                Candidates.Eight | Candidates.Nine
            };
            Candidates values = Candidates.Two | Candidates.Three;
            int[] ret = HiddenPairTriple.Search(array, values);
            Assert.AreEqual(2, ret.Length);
            CollectionAssert.AreEqual(new int[] { 1, -1 }, ret);
        }

        [TestMethod]
        public void TooManyPairInArray()
        {
            Candidates[] array = {
                Candidates.One,
                Candidates.Two | Candidates.Three,
                Candidates.Two | Candidates.Three | Candidates.Four,
                Candidates.Two | Candidates.Five,
                Candidates.Five | Candidates.Six,
                Candidates.Six | Candidates.Seven,
                Candidates.Seven | Candidates.Eight,
                Candidates.Eight | Candidates.Nine,
                Candidates.Eight | Candidates.Nine
            };
            Candidates values = Candidates.Two | Candidates.Three;
            int[] ret = HiddenPairTriple.Search(array, values);
            Assert.AreEqual(2, ret.Length);
            CollectionAssert.AreEqual(new int[] { -1, -1 }, ret);
        }

        #endregion

        #region PairMatrix

        [TestMethod]
        public void PairInMatrix()
        {
            Candidates[,] matrix = {
                { Candidates.One, Candidates.Two | Candidates.Three, Candidates.Two | Candidates.Three | Candidates.Four },
                { Candidates.Four | Candidates.Five, Candidates.Five | Candidates.Six, Candidates.Six | Candidates.Seven },
                { Candidates.Seven | Candidates.Eight, Candidates.Eight | Candidates.Nine, Candidates.Eight | Candidates.Nine }
            };
            Candidates values = Candidates.Two | Candidates.Three;
            CellPointer[] ret = Search(matrix, values);
            Assert.AreEqual(2, ret.Length);
            CellPointer[] expected =
            {
                new CellPointer{ Row = 0, Col = 1},
                new CellPointer{ Row = 0, Col = 2}
            };
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i].Row, ret[i].Row);
                Assert.AreEqual(expected[i].Col, ret[i].Col);
            }
        }

        [TestMethod]
        public void NoPairInMatrix()
        {
            Candidates[,] matrix = {
                { Candidates.One, Candidates.Six | Candidates.Five, Candidates.Two | Candidates.Three | Candidates.Four },
                { Candidates.Four | Candidates.Five, Candidates.Five | Candidates.Six, Candidates.Six | Candidates.Seven },
                { Candidates.Seven | Candidates.Eight, Candidates.Eight | Candidates.Nine, Candidates.Eight | Candidates.Nine }
            };
            Candidates values = Candidates.Two | Candidates.Three;
            CellPointer[] ret = Search(matrix, values);
            Assert.AreEqual(2, ret.Length);
            CellPointer[] expected =
            {
                new CellPointer{ Row = 0, Col = 2},
                new CellPointer{ Row = -1, Col = -1}
            };
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i].Row, ret[i].Row);
                Assert.AreEqual(expected[i].Col, ret[i].Col);
            }
        }

        [TestMethod]
        public void TooManyPairInMatrix()
        {
            Candidates[,] matrix = {
                { Candidates.One, Candidates.Two | Candidates.Three, Candidates.Two | Candidates.Three | Candidates.Four },
                { Candidates.Two | Candidates.Four, Candidates.Five | Candidates.Six, Candidates.Six | Candidates.Seven },
                { Candidates.Seven | Candidates.Eight, Candidates.Eight | Candidates.Nine, Candidates.Eight | Candidates.Nine }
            };
            Candidates values = Candidates.Two | Candidates.Three;
            CellPointer[] ret = Search(matrix, values);
            Assert.AreEqual(2, ret.Length);
            CellPointer[] expected =
            {
                new CellPointer{ Row = -1, Col = -1},
                new CellPointer{ Row = -1, Col = -1}
            };
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i].Row, ret[i].Row);
                Assert.AreEqual(expected[i].Col, ret[i].Col);
            }
        }

        #endregion

        #region TripleArray

        [TestMethod]
        public void TripleInArray()
        {
            Candidates[] array = {
                Candidates.One | Candidates.Two | Candidates.Three,
                Candidates.One | Candidates.Two | Candidates.Three,
                Candidates.One | Candidates.Two | Candidates.Three | Candidates.Four,
                Candidates.Four | Candidates.Five,
                Candidates.Five | Candidates.Six,
                Candidates.Six | Candidates.Seven,
                Candidates.Seven | Candidates.Eight,
                Candidates.Eight | Candidates.Nine,
                Candidates.Eight | Candidates.Nine
            };
            Candidates values = Candidates.One | Candidates.Two | Candidates.Three;
            int[] ret = HiddenPairTriple.Search(array, values);
            Assert.AreEqual(3, ret.Length);
            CollectionAssert.AreEqual(new int[] { 0, 1, 2 }, ret);
        }

        [TestMethod]
        public void NoTripleInArray()
        {
            Candidates[] array = {
                Candidates.Five,
                Candidates.One | Candidates.Two | Candidates.Three,
                Candidates.One | Candidates.Two | Candidates.Three | Candidates.Four,
                Candidates.Four | Candidates.Five,
                Candidates.Five | Candidates.Six,
                Candidates.Six | Candidates.Seven,
                Candidates.Seven | Candidates.Eight,
                Candidates.Eight | Candidates.Nine,
                Candidates.Eight | Candidates.Nine
            };
            Candidates values = Candidates.One | Candidates.Two | Candidates.Three;
            int[] ret = HiddenPairTriple.Search(array, values);
            Assert.AreEqual(3, ret.Length);
            CollectionAssert.AreEqual(new int[] { 1, 2, -1 }, ret);
        }

        [TestMethod]
        public void TooManyTripleInArray()
        {
            Candidates[] array = {
                Candidates.One | Candidates.Two | Candidates.Three,
                Candidates.One | Candidates.Two | Candidates.Three,
                Candidates.One | Candidates.Two | Candidates.Three | Candidates.Four,
                Candidates.Two | Candidates.Five,
                Candidates.Five | Candidates.Six,
                Candidates.Six | Candidates.Seven,
                Candidates.Seven | Candidates.Eight,
                Candidates.Eight | Candidates.Nine,
                Candidates.Eight | Candidates.Nine
            };
            Candidates values = Candidates.One | Candidates.Two | Candidates.Three;
            int[] ret = HiddenPairTriple.Search(array, values);
            Assert.AreEqual(3, ret.Length);
            CollectionAssert.AreEqual(new int[] { -1, -1, -1 }, ret);
        }

        #endregion

        #region TripleMatrix

        [TestMethod]
        public void TripleInMatrix()
        {
            Candidates[,] matrix = {
                { Candidates.One | Candidates.Two | Candidates.Three, Candidates.One | Candidates.Two | Candidates.Three, Candidates.One | Candidates.Two | Candidates.Three | Candidates.Four },
                { Candidates.Four | Candidates.Five, Candidates.Five | Candidates.Six, Candidates.Six | Candidates.Seven },
                { Candidates.Seven | Candidates.Eight, Candidates.Eight | Candidates.Nine, Candidates.Eight | Candidates.Nine }
            };
            Candidates values = Candidates.One | Candidates.Two | Candidates.Three;
            CellPointer[] ret = Search(matrix, values);
            Assert.AreEqual(3, ret.Length);
            CellPointer[] expected =
            {
                new CellPointer{ Row = 0, Col = 0},
                new CellPointer{ Row = 0, Col = 1},
                new CellPointer{ Row = 0, Col = 2}
            };
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i].Row, ret[i].Row);
                Assert.AreEqual(expected[i].Col, ret[i].Col);
            }
        }

        [TestMethod]
        public void NoTripleInMatrix()
        {
            Candidates[,] matrix = {
                { Candidates.Five | Candidates.Six | Candidates.Seven, Candidates.One | Candidates.Two | Candidates.Three, Candidates.One | Candidates.Two | Candidates.Three | Candidates.Four },
                { Candidates.Four | Candidates.Five, Candidates.Five | Candidates.Six, Candidates.Six | Candidates.Seven },
                { Candidates.Seven | Candidates.Eight, Candidates.Eight | Candidates.Nine, Candidates.Eight | Candidates.Nine }
            };
            Candidates values = Candidates.One | Candidates.Two | Candidates.Three;
            CellPointer[] ret = Search(matrix, values);
            Assert.AreEqual(3, ret.Length);
            CellPointer[] expected =
            {
                new CellPointer{ Row = 0, Col = 1},
                new CellPointer{ Row = 0, Col = 2},
                new CellPointer{ Row = -1, Col = -1}
            };
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i].Row, ret[i].Row);
                Assert.AreEqual(expected[i].Col, ret[i].Col);
            }
        }

        [TestMethod]
        public void TooManyTripleInMatrix()
        {
            Candidates[,] matrix = {
                { Candidates.One | Candidates.Two | Candidates.Three, Candidates.One | Candidates.Two | Candidates.Three, Candidates.One | Candidates.Two | Candidates.Three | Candidates.Four },
                { Candidates.One | Candidates.Four, Candidates.Five | Candidates.Six, Candidates.Six | Candidates.Seven },
                { Candidates.Seven | Candidates.Eight, Candidates.Eight | Candidates.Nine, Candidates.Eight | Candidates.Nine }
            };
            Candidates values = Candidates.One | Candidates.Two | Candidates.Three;
            CellPointer[] ret = Search(matrix, values);
            Assert.AreEqual(3, ret.Length);
            CellPointer[] expected =
            {
                new CellPointer{ Row = -1, Col = -1},
                new CellPointer{ Row = -1, Col = -1},
                new CellPointer{ Row = -1, Col = -1}
            };
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i].Row, ret[i].Row);
                Assert.AreEqual(expected[i].Col, ret[i].Col);
            }
        }

        #endregion
    }
}