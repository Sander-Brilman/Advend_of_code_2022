using System.Text;

StringBuilder input = new("    [P]                 [C] [C]    ");
input.Replace(" [", "|[").Replace("] ", "]|");
input.Replace("    ", "   |");
List<string> crates = input.ToString().Split('|').ToList();
