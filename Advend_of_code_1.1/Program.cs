using Advend_of_code_1._1.Puzzles;

int day;
string puzzle, output = "De uitkomst is: ";


while (true)
{
    try
    {
        Console.WriteLine("Selecteer een dag");
        day = int.Parse(Console.ReadLine());

        StreamReader inputFile = new($"../../../Inputs/Day_{day}.txt");

        do
        {
            Console.Clear();
            Console.WriteLine("Selecteer een puzzel (1 of 2)");
            Console.WriteLine("type < om terug te gaan");
            puzzle = Console.ReadLine();

            if (puzzle == "<") break;

        } while (puzzle != "1" && puzzle != "2");

        if (puzzle == "<") continue;

        PuzzleSolution PuzzleSolver = day switch
        {
            1 => new Day_1(inputFile),
            2 => new Day_2(inputFile),
            _ => new Day_1(inputFile),
        };

        output += PuzzleSolver.ExecutePuzzle(int.Parse(puzzle));
        Console.WriteLine(output);
        Console.WriteLine("Nog een keer (y/n)");

        if (Console.ReadLine() == "n") break;

        Console.Clear();
    }
    catch (Exception)
    {
        Console.Clear();
        Console.WriteLine("Deze dag is (nog) niet beschikbaar");
        Console.WriteLine();
        continue;
    }


}

/*

StreamReader st = new("../../../Inputs/Day_1(2).txt");

var Solver = new Day_1(st);

Console.WriteLine(Solver.Puzzle1());
Console.ReadLine();

*/
