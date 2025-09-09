namespace SudokuSolver.Core
{
    internal static class CandidateUtils
    {
        internal static Candidates FromInt(int value)
        {
            return value switch
            {
                1 => Candidates.One,
                2 => Candidates.Two,
                3 => Candidates.Three,
                4 => Candidates.Four,
                5 => Candidates.Five,
                6 => Candidates.Six,
                7 => Candidates.Seven,
                8 => Candidates.Eight,
                9 => Candidates.Nine,
                0 => Candidates.None,
                _ => throw new ArgumentOutOfRangeException(nameof(value), "Value out of range")
            };
        }

        internal static int ToInt(Candidates candidate)
        {
            if (candidate == Candidates.None) return 0;

            // Check if there's only one active bit
            if ((candidate & (candidate - 1)) != 0)
                throw new InvalidOperationException("Candidate has multiple values");

            return (int)Math.Log2((int)candidate) + 1;
        }
    }
}