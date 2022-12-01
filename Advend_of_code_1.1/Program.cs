string puzzle;
string output;
do
{
    Console.WriteLine("Choose the puzzle (1, 2)");
    puzzle = Console.ReadLine();
    Console.Clear();
} while(puzzle != "1" && puzzle != "2");




StreamReader sr = new($"../../../input.txt");

string line;
int index = 0;
List<int> calories = new() { 0 };

while ((line = sr.ReadLine()) != null)
{
    if (line == "")
    {
        index++;
        calories.Add(0);
        continue;
    }

    calories[index] += int.Parse(line);
}

if (puzzle == "1")
{
    output = calories.Max().ToString();
} else
{
    calories.Sort();
    calories.Reverse();

    output = (calories[0] + calories[1] + calories[2]).ToString();
}



Console.WriteLine($"Output puzzle {puzzle}: {output}");
Console.ReadLine();