using Advend_of_code_1._1.Puzzles;
using System.Reflection.Metadata;
using System.Security.Cryptography;

int dayNumber;
string puzzle, day, output = "De uitkomst is: ";

bool runApp()
{
    Console.WriteLine("Selecteer een dag");
    Console.WriteLine("type < om terug te gaan");
    day = Console.ReadLine();

    if (day == "<") return false;
    if (!int.TryParse(day, out dayNumber)) return true;

    StreamReader inputFile;
    try
    {
        inputFile = new($"../../../Inputs/Day_{dayNumber}.txt");
    }
    catch (Exception)
    {
        Console.Clear();
        Console.WriteLine("Deze dag is (nog) niet beschikbaar");
        Console.WriteLine("Druk op enter om verder te gaan");
        Console.ReadLine();
        return true;
    }

    do
    {
        Console.Clear();
        Console.WriteLine($"dag {dayNumber}: Selecteer een puzzel (1 of 2)");
        Console.WriteLine("type < om terug te gaan");
        puzzle = Console.ReadLine();

        if (puzzle == "<") return true;

    } while (puzzle != "1" && puzzle != "2");

    PuzzleSolution PuzzleSolver;
    try
    {
        PuzzleSolver = dayNumber switch
        {
            1 => new Day_1(inputFile),
            2 => new Day_2(inputFile),
            3 => new Day_3(inputFile),
            4 => new Day_4(inputFile),
            5 => new Day_5(inputFile),
            6 => new Day_6(inputFile),
            7 => new Day_7(inputFile),
            8 => new Day_8(inputFile),
            9 => new Day_9(inputFile),
            _ => throw new NotImplementedException(),
        };
    } 
    catch (NotImplementedException)
    {
        return true;
    }


    output = PuzzleSolver.ExecutePuzzle(int.Parse(puzzle));
    Console.WriteLine(output);
    Console.WriteLine("Nog een keer (y/n)");

    if (Console.ReadLine() == "n") return false;

    return true;
}

do
{
    Console.Clear();
} while (runApp());
