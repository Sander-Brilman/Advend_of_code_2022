using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Puzzels2023.Solutions;
public class Solution5(string path) : SolutionBase(path)
{
    private class Source
    {
        public long Start { get; set; }

        public long RangeLength { get; set; }

        public bool IsDoneForCurrentMap { get; set; } = false;

        public long End => Start + (RangeLength - 1);
    }

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

        int forLength = seedRanges.Length;
        for (int i = 0; i <= forLength; i += 2)
        {
            long seedStart = seedRanges[i];
            long seedRange = seedRanges[i + 1];


            List<Source> sources = [new Source()
            {
                RangeLength = seedRange,
                Start = seedStart,
            }];

            var maps = GetMaps();
            foreach (var map in maps)
            {
                foreach (Source s in sources)
                    s.IsDoneForCurrentMap = false;

                foreach (var mapRule in map)
                {

                    for (int sourceIndex = 0; sourceIndex < sources.Count; sourceIndex++)
                    {
                        var source = sources[sourceIndex];
                        if (source.IsDoneForCurrentMap) { continue; }


                        long[] numbers = GetNumbersFromString(mapRule);

                        long mapDesinationStart = numbers[0];
                        long mapSourceStart = numbers[1];
                        long mapRangeLength = numbers[2];
                        long mapSourceEnd = mapSourceStart + (mapRangeLength - 1);

                        if (source.End < mapSourceStart || source.Start > mapSourceEnd)
                        {
                            //the whole set is completely ignored, continue to next
                            continue;
                        }
                        else if (source.Start >= mapSourceStart && source.End <= mapSourceEnd)
                        {
                            // the whole set can be incremented as a whole, continue to next set
                            source.Start += (mapDesinationStart - mapSourceStart);
                            source.IsDoneForCurrentMap = true;
                            continue;
                        }
                        else if (source.Start < mapSourceStart && source.End > mapSourceEnd)
                        {
                            // create source on the front
                            long newSetLength = mapSourceStart - source.Start;
                            Source newSplitSourceLeft = new()
                            {
                                Start = source.Start,
                                RangeLength = newSetLength,
                            };

                            sources.Add(newSplitSourceLeft);


                            // create source on the back
                            newSetLength = source.End - mapSourceEnd;
                            Source newSplitSourceRight = new()
                            {
                                Start = mapSourceEnd + 1,
                                RangeLength = newSetLength,
                            };

                            sources.Add(newSplitSourceRight);



                            // update current source
                            source.Start = mapSourceStart;
                            source.RangeLength = mapRangeLength;

                            source.Start += (mapDesinationStart - mapSourceStart);
                            source.IsDoneForCurrentMap = true;

                            continue;
                        }
                        else if (source.Start < mapSourceStart)
                        {
                            // split on mapSourceStart

                            long newSetLength = mapSourceStart - source.Start;

                            Source newSplitSource = new()
                            {
                                Start = source.Start,
                                RangeLength = newSetLength,
                            };

                            sources.Add(newSplitSource);

                            source.Start = mapSourceStart;
                            source.Start += (mapDesinationStart - mapSourceStart);

                            source.RangeLength -= newSetLength;
                            source.IsDoneForCurrentMap = true;

                            continue;
                        }
                        else if (source.End < mapSourceEnd)
                        {
                            // split on mapSourceEnd

                            long newSetLength = source.End - mapSourceEnd;

                            Source newSplitSource = new()
                            {
                                Start = mapSourceEnd + 1,
                                RangeLength = newSetLength,
                            };

                            sources.Add(newSplitSource);

                            source.Start += (mapDesinationStart - mapSourceStart);
                            source.RangeLength -= newSetLength;
                            source.IsDoneForCurrentMap = true;

                            continue;
                        }
                    }// source foreach

                }// mapRule foreach

            }// maps foreach


            long minFromTheseSets = sources
                .MinBy(x => x.Start)
                !.Start;

            if (minSoFar is null)
            {
                minSoFar = minFromTheseSets;
                continue;
            }

            minSoFar = minFromTheseSets < minSoFar
                ? minFromTheseSets : minSoFar;
        }

        return minSoFar!.ToString()!;
    }
}
