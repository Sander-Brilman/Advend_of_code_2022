global using CommonLiberary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Puzzels2023.Solutions;
public class Solution1() : SolutionBase(1)
{
    private static readonly string[] _numbersAsText = [
        "one",
        "two",
        "three",
        "four",
        "five",
        "six",
        "seven",
        "eight",
        "nine",
    ];


    /// <summary>
    /// basically the same method but for puzzle 2
    /// </summary>
    /// <param name="line"></param>
    /// <returns></returns>
    private static int[] GetNumbersFromLineAdvances(string line)
    {
        List<KeyValuePair<int, int>> columnIndexAndValue = [];

        // string form search
        for (int i = 0; i < _numbersAsText.Length; i++)
        {
            string stringNumber = _numbersAsText[i];
            int numericalValue = i + 1;
            int chunkSize = stringNumber.Length;

            for(int j = 0; j <= line.Length - chunkSize; j++)
            {
                string chunk = line.Substring(j, chunkSize);

                int index = chunk.IndexOf(stringNumber);

                if (index == -1) { continue; }


                // fix offset
                index += j;

                columnIndexAndValue.Add(new(index, numericalValue));
            }
        }

        // number form search
        var lineChars = line
            .Select((character, index) => (index, character))
            .Where(x => char.IsNumber(x.character));

        foreach (var (index, character) in lineChars)
        {
            columnIndexAndValue.Add(new(index, (int)char.GetNumericValue(character)));
        }

        return columnIndexAndValue
            .OrderBy(x => x.Key)
            .Select(x => x.Value)
            .ToArray();
    }

    private static int[] GetNumbersFromLine(string line)
    {
        return line
            .Where(char.IsDigit)
            .Select(c => (int)char.GetNumericValue(c))
            .ToArray(); 
    }

    public override string GetFirstSolution()
    {
        int total = 0;

        for (int i = 0; i < _lines.Length; i++)
        {
            int[] numbersInLine = GetNumbersFromLine(_lines[i]);

            int fullNumber = int.Parse($"{numbersInLine.First()}{numbersInLine.Last()}");

            total += fullNumber;
        }

        return total.ToString();
    }

    public override string GetSecondSolution2()
    {
        return "";
        //int total = 0;

        //    string line = _lines[i];
        //    int[] numbersInLine = GetNumbersFromLineAdvances(line);

        //    int fullNumber = int.Parse($"{numbersInLine.First()}{numbersInLine.Last()}");

        //    total += fullNumber;
        //}

        //return total.ToString();
    }
}
