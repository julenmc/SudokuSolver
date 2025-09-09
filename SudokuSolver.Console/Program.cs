using SudokuSolver.Core;

namespace SudokuSolver.ConsoleApp
{
    class Program
    {
        static readonly int[,] Sudoku =
        {
            { 5, 0, 0, 2, 0, 7, 4, 1, 0 },
            { 0, 6, 0, 9, 0, 8, 0, 0, 0 },
            { 0, 9, 0, 0, 1, 0, 7, 6, 0 },
            { 0, 0, 0, 1, 8, 0, 0, 4, 0 },
            { 0, 0, 0, 0, 9, 0, 2, 0, 6 },
            { 0, 0, 0, 0, 5, 4, 0, 0, 7 },
            { 0, 5, 9, 0, 7, 0, 0, 0, 3 },
            { 0, 1, 3, 5, 0, 0, 9, 0, 0 },
            { 0, 0, 7, 8, 0, 9, 0, 0, 0 }
        };

        // static readonly int[,] Sudoku =
        // {
        //     { 0, 2, 3, 4, 5, 6, 7, 8, 9 },
        //     { 4, 5, 6, 7, 8, 9, 1, 2, 3 },
        //     { 7, 8, 9, 1, 2, 3, 4, 5, 6 },
        //     { 2, 3, 4, 5, 6, 7, 8, 9, 1 },
        //     { 5, 6, 7, 8, 9, 1, 2, 3, 4 },
        //     { 8, 9, 1, 2, 3, 4, 5, 6, 7 },
        //     { 3, 4, 5, 6, 7, 8, 9, 1, 2 },
        //     { 6, 7, 8, 9, 1, 2, 3, 4, 5 },
        //     { 9, 1, 2, 3, 4, 5, 6, 7, 8 }
        // };

        static void Main()
        {
            Console.WriteLine("Starting Sudoku Solver");

            try
            {
                Manager manager = new Manager(Sudoku);
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
                Console.WriteLine($"Error while solving Sudoku");
            }
        }
    }
}