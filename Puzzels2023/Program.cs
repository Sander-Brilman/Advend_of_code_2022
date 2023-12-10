using Puzzels2023.Solutions;
using static System.Console;

do
{
    int puzzle;
    int section;
    bool loadExampleFile;


    //
    // fetch puzzle number
    //
    string input;
    do
    {
        WriteLine("Puzzel number: \n");
        input = ReadLine() ?? "";
        Clear();
    } while (int.TryParse(input, out puzzle) is false);

    //
    // get section number
    // 
    do
    {
        WriteLine("section number [1 or 2]: \n");
        input = ReadLine() ?? "";
        Clear();
    } while (int.TryParse(input, out section) is false || section > 2 || section < 1);

    //
    // ask for example file or not
    //
    do
    {
        WriteLine("load example? (1 | y) / (2 | n): \n");
        input = (ReadLine() ?? "n")
            .Replace("1", "True")
            .Replace("0", "False")
            .Replace("2", "False")
            .Replace("y", "True")
            .Replace("n", "False");
        Clear();
    } while (bool.TryParse(input, out loadExampleFile) is false);

    //
    // create instance of solution class
    //
    string className = $"Puzzels2023.Solutions.Solution{puzzle}";
    Type? type = Type.GetType(className);

    if (type is null)
    {
        WriteLine("this Solution does not exist yet..");
        WriteLine();
        Write("Again? [y/n]: ");

        continue;
    }


    //
    // assembly puzzle input file path
    //
    string puzzleInputPath = Path.Combine(
        Directory.GetCurrentDirectory(),
        "TextFiles",
        $"Puzzle{puzzle}{(loadExampleFile ? ".example" : "")}.txt");


    SolutionBase? puzzelSolution = (SolutionBase?)Activator.CreateInstance(type, puzzleInputPath);

    if (puzzelSolution is null)
    {
        WriteLine("this Solution does not exist yet..");
        WriteLine();
        Write("Again? [y/n]: ");
        continue;
    }

    //
    // get & print solution output
    //
    WriteLine("Solution:");
    WriteLine(puzzelSolution.GetSectionSolution(section));

    WriteLine();
    Write("Again? [y/n]: ");

} while (ReadLine() == "y");