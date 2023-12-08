using System.Drawing;
using System.IO.Pipes;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;

namespace Puzzels2023.Solutions;

internal class Solution8(string path) : SolutionBase(path)
{
    private enum Direction
    {
        Left,
        Right
    }

    private static Direction CharToDirection(char input) => input switch
    {
        'L' => Direction.Left,
        'R' => Direction.Right,
        _ => throw new UnreachableException()
    };

    private Direction[] GetDirections()
    {
        return _lines[0]
            .ToCharArray()
            .Select(CharToDirection)
            .ToArray();
    }

    private record MapPoint(string Start, string LeftOption, string RightOption)
    {
        public static MapPoint FromString(string input)
        {
            string[] points = input
                .Replace("= (", "")
                .Replace(",", "")
                .Replace(")", "")
                .Split(' ')
                .ToArray();

            return new MapPoint(points[0], points[1], points[2]);
        }
    }

    private MapPoint[] GetPoints()
    {
        return _lines
            .Skip(2)
            .Select(MapPoint.FromString)
            .ToArray();
    }


    public override string GetFirstSolution()
    {
        Direction[] directions = GetDirections();
        int directionsPointer = 0;

        Direction GetNextDirection()
        {
            directionsPointer %= directions.Length;
            Direction direction = directions[directionsPointer];
            directionsPointer++;
            return direction;
        }

        var points = GetPoints();
        string finalPoint = "ZZZ";
        

        string currentPoint = "AAA";

        int amountOfStepsTaken = 0;
        while (currentPoint != finalPoint)
        {
            MapPoint startingFrom = points
                .FirstOrDefault(x => x.Start == currentPoint)!;

            Direction direction = GetNextDirection();

            currentPoint = direction switch
            {
                Direction.Left => startingFrom.LeftOption,
                Direction.Right => startingFrom.RightOption,
                _ => throw new UnreachableException()
            };

            amountOfStepsTaken++;
        }

        return amountOfStepsTaken.ToString();
    }

    private record PathInformation(MapPoint Start, double LoopLength);

    private double GetLoopLengthForStartingPoint(MapPoint start)
    {
        var points = GetPoints();

        Direction[] directions = GetDirections();
        int directionsPointer = 0;

        Direction GetNextDirection()
        {
            directionsPointer %= directions.Length;
            Direction direction = directions[directionsPointer];
            directionsPointer++;
            return direction;
        }

        double amountOfStepsTaken = 0;
        while (start.Start.Last() != 'Z')
        {
            Direction direction = GetNextDirection();

            string nextPoint = direction switch
            {
                Direction.Left => start.LeftOption,
                Direction.Right => start.RightOption,
                _ => throw new UnreachableException()
            };

            start = points.FirstOrDefault(x => x.Start == nextPoint)!;
            
            amountOfStepsTaken++;
        }

        return amountOfStepsTaken;
    }


    public override string GetSecondSolution2()
    {
        var points = GetPoints();

        PathInformation[] paths = points
            .Where(p => p.Start.Last() == 'A')
            .Select(p => new PathInformation(p, GetLoopLengthForStartingPoint(p)))
            .ToArray();

        int gcd = 2;
        for (int i = 1000; i >= 2; i--)
        {
            if (paths.Any(p => p.LoopLength % i == 0))
            {
                gcd = i;
                break;
            }
        }

        for (int i = 0; i < paths.Length; i++)
        {
            var path = paths[i];

            paths[i] = path with
            {
                LoopLength = path.LoopLength / gcd
            };
        }

        double total = 1;
        foreach (var path in paths)
        {
            total *= path.LoopLength;
        }

        total *= gcd;

        return "";
    }
}
