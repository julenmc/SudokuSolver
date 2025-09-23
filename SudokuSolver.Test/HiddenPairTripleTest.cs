using SudokuSolver.Core.Enums;
using SudokuSolver.Core.Services;

namespace SudokuSolver.Test
{
    [TestClass]
    public sealed class HiddenPairTest
    {
        [TestMethod]
        public void PairInArray()
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
            Candidates[] values = { Candidates.Two, Candidates.Three };
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
                Candidates.Two | Candidates.Five,
                Candidates.Five | Candidates.Six,
                Candidates.Six | Candidates.Seven,
                Candidates.Seven | Candidates.Eight,
                Candidates.Eight | Candidates.Nine,
                Candidates.Eight | Candidates.Nine
            };
            Candidates[] values = { Candidates.Two, Candidates.Three };
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
                Candidates.Two | Candidates.Three | Candidates.Five,
                Candidates.Five | Candidates.Six,
                Candidates.Six | Candidates.Seven,
                Candidates.Seven | Candidates.Eight,
                Candidates.Eight | Candidates.Nine,
                Candidates.Eight | Candidates.Nine
            };
            Candidates[] values = { Candidates.Two, Candidates.Three };
            int[] ret = HiddenPairTriple.Search(array, values);
            Assert.AreEqual(2, ret.Length);
            CollectionAssert.AreEqual(new int[] { -1, -1 }, ret);
        }

        [TestMethod]
        public void TripleInArray()
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
            Candidates[] values = { Candidates.One, Candidates.Two, Candidates.Three };
            int[] ret = HiddenPairTriple.Search(array, values);
            Assert.AreEqual(3, ret.Length);
            CollectionAssert.AreEqual(new int[] { 0, 1, 2 }, ret);
        }
    }
}