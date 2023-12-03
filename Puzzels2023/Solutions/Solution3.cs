using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Puzzels2023.Solutions;
public class Solution3(string filePath) : SolutionBase(filePath)
{
    private static readonly char[] _symbols = ['+', '#', '*', '$', '%', '=', '@', '/', '-', '&'];

    private record SymbolLocation(int Column, int Row, char Symbol);

    private record NumberLocation(int Column, int Row, int Length, int Value, SymbolLocation[] AdjacentSymbols);

    private int _lineLength = 0;

    private SymbolLocation[] GetCharsArountNumber(int column, int row, int length)
    {
        List<SymbolLocation> chars = [];

        // check left 
        int adjacentColumnLeft  = column - 1 <  0             ? 0      : column - 1;
        int adjacentColumnRight = (column + length - 1) + 1 >= _lineLength ? (column + length - 1) : (column + length - 1) + 1;
        int adjacentRowBottom   = row    + 1 >= _lines.Length ? row    : row    + 1;
        int adjacentRowTop      = row    - 1 <  0             ? 0      : row    - 1;

        void GetCharsForLine(int start, int row, int length)
        {
            string scanString = _lines[row]
                .Substring(start, length);

            var symbolsInString = scanString
                .Select((symbol, index) => new SymbolLocation(start + index, row, symbol))
                .Where(c => c.Symbol != '.' && char.IsNumber(c.Symbol) is false);

            chars.AddRange(symbolsInString);
        }

        int scanLength = (adjacentColumnRight - adjacentColumnLeft) + 1;

        // top row
        GetCharsForLine(adjacentColumnLeft, adjacentRowTop, scanLength);

        // bottom row
        GetCharsForLine(adjacentColumnLeft, adjacentRowBottom, scanLength);


        // scan middle row if needed
        bool hasScannedMidRow = adjacentRowTop == row || adjacentRowBottom == row;
        if (hasScannedMidRow is false)
        {
            GetCharsForLine(adjacentColumnLeft, row, (adjacentColumnRight - adjacentColumnLeft) + 1);
        }

        return [..chars];
    }

    private NumberLocation[] GetNumberLocations()
    {
        List<NumberLocation> locations = [];

        for (int row = 0; row < _lines.Length; row++)
        {
            string line = _lines[row];
            for (int column = 0; column < line.Length; column++)
            {
                char lineChar = line[column];

                if (lineChar == '.' || char.IsNumber(lineChar) is false) { continue; }

                // if we got here that means we found a number

                // index boundries are inclusive 
                int endOfNumber = line.IndexOfAny(['.', .._symbols], column);
                endOfNumber = endOfNumber  == -1 ? line.Length : endOfNumber;

                // - 1 to make the index inclusive
                int endIndex = endOfNumber - 1;

                int numberLength = (endIndex - column) + 1;

                SymbolLocation[] charsAroundNumber = GetCharsArountNumber(column, row, numberLength);

                string valueRaw = line.Substring(column, numberLength);
                int value = int.Parse(valueRaw);

                locations.Add(new NumberLocation(column, row, numberLength, value, charsAroundNumber));


                // skip this number for next cycle
                column = endIndex + 1;
            }
        }

        return [..locations]; 
    }

    public override string GetFirstSolution()
    {
        _lineLength = _lines[0].Length;

        NumberLocation[] locations = GetNumberLocations();

        return locations
            .Where(l => l.AdjacentSymbols.Length > 0)
            .Select(l => l.Value)
            .Sum()
            .ToString();
    }

    public override string GetSecondSolution2()
    {
        _lineLength = _lines[0].Length;

        List<(NumberLocation, NumberLocation)> ratioCache = [];
        int total = 0;

        NumberLocation[] locations = GetNumberLocations();

        var numbersWithGearSymbol = locations
            .Where(l => l.AdjacentSymbols.Any(c => c.Symbol == '*'));

        foreach (var currentNumber in numbersWithGearSymbol)
        {
            foreach (var currentGear in currentNumber.AdjacentSymbols)
            {
                var secondNumberWithSameGear = numbersWithGearSymbol
                    .FirstOrDefault(n =>
                    {
                        bool hasCurrentGear = n.AdjacentSymbols.Contains(currentGear);

                        bool isSameNumber = n != currentNumber;

                        return hasCurrentGear && isSameNumber; // check he doesnt request himself from the list
                    });

                if (secondNumberWithSameGear is null)
                {
                    continue;
                }

                if (ratioCache.Contains((currentNumber, secondNumberWithSameGear))) { continue; }
                ratioCache.Add((currentNumber, secondNumberWithSameGear));
                ratioCache.Add((secondNumberWithSameGear, currentNumber));

                int ratio = (currentNumber.Value * secondNumberWithSameGear.Value);
                total += ratio;
            }
        }

        return total.ToString();
    }
}
