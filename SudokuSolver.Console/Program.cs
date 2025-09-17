using SudokuSolver.Core;

namespace SudokuSolver.ConsoleApp
{
    class Program
    {
        /* Template
        {
            { , , , , , , , ,  },
            { , , , , , , , ,  },
            { , , , , , , , ,  },
            { , , , , , , , ,  },
            { , , , , , , , ,  },
            { , , , , , , , ,  },
            { , , , , , , , ,  },
            { , , , , , , , ,  },
            { , , , , , , , ,  }
        };
        */
        static readonly int[,] VeryHardSudoku =
        {
            { 0, 0, 0, 0, 0, 0, 9, 0, 8 },
            { 0, 7, 4, 0, 0, 0, 5, 0, 0 },
            { 0, 0, 0, 0, 0, 3, 0, 0, 7 },
            { 0, 3, 5, 0, 4, 0, 1, 0, 0 },
            { 6, 0, 0, 0, 0, 1, 0, 0, 0 },
            { 0, 8, 0, 0, 3, 0, 0, 2, 0 },
            { 0, 0, 0, 3, 7, 0, 0, 0, 1 },
            { 0, 5, 0, 0, 0, 0, 0, 6, 0 },
            { 7, 6, 8, 0, 0, 0, 0, 0, 2 }
        };

        static readonly int[,] HardSudoku =
        {
            { 0, 4, 0, 8, 2, 0, 3, 0, 5 },
            { 0, 0, 0, 0, 0, 6, 0, 4, 2 },
            { 0, 0, 0, 0, 1, 0, 8, 0, 0 },
            { 0, 2, 0, 5, 0, 0, 0, 1, 0 },
            { 5, 0, 8, 0, 0, 0, 4, 0, 7 },
            { 0, 9, 0, 0, 0, 2, 0, 8, 0 },
            { 0, 0, 2, 0, 6, 0, 0, 0, 0 },
            { 9, 1, 0, 2, 0, 0, 0, 0, 0 },
            { 4, 0, 6, 0, 5, 1, 0, 9, 0 }
        };

        static readonly int[,] MediumSudoku =
        {
            { 1, 7, 0, 0, 0, 2, 0, 0, 0 },
            { 0, 0, 2, 5, 0, 0, 6, 7, 0 },
            { 0, 0, 0, 0, 0, 1, 2, 3, 4 },
            { 3, 6, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 4, 7, 0, 0, 0, 1, 9, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 6, 2 },
            { 8, 1, 5, 4, 0, 0, 0, 0, 0 },
            { 0, 9, 4, 0, 0, 8, 3, 0, 0 },
            { 0, 0, 0, 1, 0, 0, 0, 8, 6 }
        };

        static readonly int[,] EasySudoku =
        {
            { 7, 0, 0, 6, 0, 0, 1, 0, 0 },
            { 0, 0, 3, 0, 9, 4, 0, 8, 0 },
            { 8, 0, 0, 0, 0, 5, 9, 7, 0 },
            { 0, 7, 0, 0, 6, 0, 0, 4, 0 },
            { 0, 0, 0, 2, 1, 7, 0, 0, 0 },
            { 0, 2, 0, 0, 4, 0, 0, 1, 0 },
            { 0, 9, 1, 8, 0, 0, 0, 0, 7 },
            { 0, 5, 0, 4, 2, 0, 6, 0, 0 },
            { 0, 0, 6, 0, 0, 3, 0, 0, 1 }
        };

        static readonly int[,] SuperEasySudoku =
        {
            { 0, 2, 3, 4, 5, 6, 7, 8, 9 },
            { 4, 5, 6, 7, 8, 9, 1, 2, 3 },
            { 7, 8, 9, 1, 2, 3, 4, 5, 6 },
            { 2, 3, 4, 5, 6, 7, 8, 9, 1 },
            { 5, 6, 7, 8, 9, 1, 2, 3, 4 },
            { 8, 9, 1, 2, 3, 4, 5, 6, 7 },
            { 3, 4, 5, 6, 7, 8, 9, 1, 2 },
            { 6, 7, 8, 9, 1, 2, 3, 4, 5 },
            { 9, 1, 2, 3, 4, 5, 6, 7, 8 }
        };

        static void Main()
        {
            Console.WriteLine("Starting Sudoku Solver");

            try
            {
                Manager manager = new Manager(VeryHardSudoku);
                int[,] solution = manager.SolveSudoku();

                Console.WriteLine("Sudoku completed! Result:");
                for (int i = 0; i < Constants.SudokuSize; i++)
                {
                    for (int j = 0; j < Constants.SudokuSize; j++)
                    {
                        Console.Write($"{solution[i, j]} ");
                    }
                    Console.WriteLine();
                }
            }
            catch (SudokuError ex)
            {
                Console.WriteLine($"Error while solving Sudoku: {ex.Type}, {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while solving Sudoku: {ex.Message}");
            }
        }
    }
}