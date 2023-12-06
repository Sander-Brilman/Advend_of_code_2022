using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
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

        public string? StartFromMapRule { get; set; }

        public int OriginalSeedOffset { get; set; }

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



    private readonly List<Source> _sources = [];

    /// <summary>
    /// returns false if it did noting returns true if it changed the source
    /// </summary>
    /// <param name="source"></param>
    /// <param name="mapRule"></param>
    /// <returns></returns>
    private bool ProcessMapRuleForSource(Source source, string mapRule)
    {
        long[] mapNumbers = GetNumbersFromString(mapRule);

        long mapDestinationStart = mapNumbers[0];
        long mapRangeLength = mapNumbers[2];

        long mapSourceStart = mapNumbers[1];
        long mapSourceEnd = mapSourceStart + (mapRangeLength - 1);


        // source is completely left out 
        if (source.End < mapSourceStart || source.Start > mapSourceEnd)
        {
            return false;
        }

        // source falls completely within the boundries
        if (source.Start >= mapSourceStart && source.End <= mapSourceEnd)
        {
            source.Start += (mapDestinationStart - mapSourceStart);
            return true;
        }



        // source gets cut off from the left 
        if (source.Start < mapSourceStart && 
            source.End >= mapSourceStart &&
            source.End <= mapSourceEnd) 
        {
            long newSourceLength = mapSourceStart - source.Start;

            Source newSource = new()
            {
                Start = source.Start,
                RangeLength = newSourceLength,
                StartFromMapRule = mapRule
            };

            _sources.Add(newSource);

            source.Start = mapDestinationStart;

            if (source.Start < 0)
            {
                Console.WriteLine();
            }


            source.RangeLength -= newSourceLength;

            return true;
        }

        // source gets cut off from the right
        if (source.Start >= mapSourceStart &&
            source.Start <= mapSourceEnd &&
            source.End > mapSourceEnd)
        {
            long newSourceLength = source.End - mapSourceEnd;

            Source newSource = new()
            {
                Start = mapSourceEnd + 1,
                RangeLength = newSourceLength,
                StartFromMapRule = mapRule
            };

            _sources.Add(newSource);

            source.Start += (mapDestinationStart - mapSourceStart);

            if (source.Start < 0)
            {
                Console.WriteLine();
            }


            source.RangeLength -= newSourceLength;

            return true;
        }


        // if the source is encapsulates the entire rule and has parts left out on both sides
        if (source.Start < mapSourceStart &&
            source.End > mapSourceEnd)
        {
            // left part
            long newLeftSourceLength = mapSourceStart - source.Start;

            Source newLeftSource = new()
            {
                Start = source.Start,
                RangeLength = newLeftSourceLength,
                StartFromMapRule = mapRule
            };

            _sources.Add(newLeftSource);


            // right part
            long newRightSourceLength = source.End - mapSourceEnd;

            Source newRightSource = new()
            {
                Start = mapSourceEnd + 1,
                RangeLength = newRightSourceLength,
                StartFromMapRule = mapRule
            };

            _sources.Add(newRightSource);


            // update original source
            source.Start = mapDestinationStart;

            if (source.Start < 0)
            {
                Console.WriteLine();
            }

            source.RangeLength = mapRangeLength;

            return true;
        }

        throw new UnreachableException();
    }

    public override string GetSecondSolution2()
    {
        long[] seedValues = GetFirstLineNumbers();

        for (int i = 0; i < seedValues.Length; i += 2)
        {
            _sources.Add(new Source()
            {
                Start = seedValues[i],
                RangeLength = seedValues[i + 1],
            });
        }


        var maps = GetMaps();
        foreach (var map in maps)
        {

            for (int sourceIndex = 0; sourceIndex < _sources.Count; sourceIndex++)
            {
                Source source = _sources[sourceIndex];

                foreach (var mapRule in map)
                {
                    if (source.StartFromMapRule is not null && source.StartFromMapRule != mapRule)
                    {
                        continue;
                    }


                    bool doneForThisMap = ProcessMapRuleForSource(source, mapRule);



                    source.StartFromMapRule = null;

                    if (doneForThisMap)
                    {
                        break;
                    }
                    



                } // map rule loop


            }// sources loop

        }// map loop

        long smallest = _sources
            .MinBy(x => x.Start)
            .Start;


        return smallest.ToString();
    }
}
