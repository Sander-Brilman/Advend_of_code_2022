string puzzle = "";
string output = "";
do
{
    Console.WriteLine("Choose the puzzle (1, 2)");
    Console.ReadLine();
    Console.Clear();
} while(puzzle != "1" && puzzle != "2");




StreamReader sr = new($"../../../input{puzzle}.txt");


if (puzzle == "1")
{
    string line;
    int index = 0;
    List<int> calories = new() {0};

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

    output = calories.Max().ToString();
} else
{

}



Console.WriteLine($"Output puzzle {puzzle}: {output}");
Console.ReadLine();