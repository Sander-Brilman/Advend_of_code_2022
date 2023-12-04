using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puzzels2023.Solutions;
public class Solution4(string filePath) : SolutionBase(filePath)
{
    private int[] GetWinningNumbersForCard(string card)
    {
        List<int> numbers = [];

        string allNumbers = card.Split(": ")[1].Trim();

        string[] numberSections = allNumbers.Split(" | ");


        int[] winningNumbers = numberSections[0]
            .Split(" ")
            .Where(w => w != "")
            .Select(w => int.Parse(w.Trim()))
            .ToArray();

        int[] currentNumbers = numberSections[1]
            .Split(" ")
            .Where(w => w != "")
            .Select(w => int.Parse(w.Trim()))
            .ToArray();


        return winningNumbers
            .Where(w => currentNumbers.Contains(w))
            .ToArray();
    }



    public override string GetFirstSolution()
    {
        int total = 0;

        foreach (var line in _lines)
        {
            int[] winningNumbers = GetWinningNumbersForCard(line);
            
            if (winningNumbers.Length == 0) { continue; }

            if (winningNumbers.Length == 1) 
            { 
                total++;
                continue;
            }

            int addNumber = (int)Math.Pow(2, winningNumbers.Length - 1);
            total += addNumber;
        }

        return total.ToString();
    }

    public override string GetSecondSolution2()
    {
        int[] cardInstances = Enumerable.Range(1, _lines.Length)
            .Select(i => 1)
            .ToArray();


        for (int lineIndex = 0; lineIndex < _lines.Length; lineIndex++)
        {
            string line = _lines[lineIndex];
            int cardNumber = lineIndex + 1;
            int amountOfInstances = cardInstances[cardNumber - 1];

            int amountOfWinningNumbers = GetWinningNumbersForCard(line).Length;

            for (int i = 1; i <= amountOfWinningNumbers; i++)
            {
                int nextCardIndex = cardNumber + i;
                cardInstances[nextCardIndex - 1] += amountOfInstances;
            }
        }

        return cardInstances.Sum().ToString();
    }
}
