using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puzzels2023.Solutions;
public class Solution5(string path) : SolutionBase(path)
{
    private static long[] GetNumbersFromString(string str)
    {
        return str
            .Split(" ")
            .Select(long.Parse)
            .ToArray();
    }

    private long[] GetFirstLineNumbers()
    {
        return GetNumbersFromString(_lines[0]
            .Replace("seeds: ", ""));
    }

    private List<List<string>> GetMaps()
    {
        List<List<string>> maps = [];

        bool isTrackingMap = false;
        for (long i = 2; i < _lines.Length; i++)
        {
            var line = _lines[i];

            if (line == "")
            {
                isTrackingMap = false;
                continue;
            }

            if (isTrackingMap)
            {
                maps.Last().Add(line);
                continue;
            }

            if (line.Contains(':'))
            {
                isTrackingMap = true;
                maps.Add([]);
                continue;
            }
        }

        return maps;
    }

    private long GetLowestLocationForSeed(long seed)
    {
        List<List<string>> maps = GetMaps();

        foreach (List<string> map in maps)
        {
            foreach (var mapString in map)
            {
                long[] numbers = GetNumbersFromString(mapString);

                long rangeLength = numbers[2];
                long sourceStart = numbers[1];
                long destinationStart = numbers[0];

                if (seed < sourceStart || seed >= sourceStart + rangeLength)
                {
                    continue;
                }

                seed += (destinationStart - sourceStart);
                break;
            }// map line foreach
        }// maps foreach

        return seed;
    }

    public override string GetFirstSolution()
    {
        long[] source = GetFirstLineNumbers();

        for (int i = 0; i < source.Length; i++)
        {
            source[i] = GetLowestLocationForSeed(source[i]);
        }

        return source.Min().ToString();
    }

    public override string GetSecondSolution2()
    {
        long[] seedRanges = GetFirstLineNumbers();

        long? minSoFar = null;

        Console.WriteLine($"[{DateTime.UtcNow}] Start");

        int forLength = seedRanges.Length / 2;
        for (int i = 0; i < forLength; i += 2)
        {
            long seedStart = seedRanges[i];
            long seedRange = seedRanges[i + 1];

            Parallel.For(
                seedStart,
                seedStart + seedRange,
                new ParallelOptions()
                {
                    MaxDegreeOfParallelism = 150
                },
                seed =>
                {
                    long minOfSelection = GetLowestLocationForSeed(seed);

                    minSoFar = minSoFar == null
                        ? minOfSelection
                        : (minSoFar > minOfSelection ? minOfSelection : minSoFar);
                });

            Console.WriteLine($"[{DateTime.UtcNow}] complete {i} / {forLength}");
        }


        return minSoFar!.ToString()!;
    }
}
