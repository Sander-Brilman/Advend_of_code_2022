


using System.Text.RegularExpressions;

string input = "000one00";

string search = "one!";


var thing = Regex.Match(input, search);

Console.WriteLine(thing.Value == "");


Console.WriteLine(thing.NextMatch());