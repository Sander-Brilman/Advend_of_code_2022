using Puzzels2023.Solutions;
using static System.Console;

do
{
    int puzzel;

    string input;
    do
    {
        WriteLine("Puzzel number: \n");
        input = ReadLine() ?? "";
        Clear();
    } while (int.TryParse(input, out puzzel) is false);

    string className = $"Puzzels2023.Solutions.Solution{puzzel}";
    Type? type = Type.GetType(className);

    if (type is null)
    {
        WriteLine("this Solution does not exist yet..");
        continue;
    }

    SolutionBase? puzzelSolution = (SolutionBase?)Activator.CreateInstance(type);

    if (puzzelSolution is null)
    {
        WriteLine("this Solution does not exist yet..");
        continue;
    }

    int section;
    do
    {
        WriteLine("section number [1 or 2]: \n");
        input = ReadLine() ?? "";
        Clear();
    } while (int.TryParse(input, out section) is false || section > 2 || section < 1);


    WriteLine("Solution:");
    WriteLine(puzzelSolution.GetSectionSolution(section));

    WriteLine();
    Write("Again? [y/n]: ");
} while (ReadLine() == "y");