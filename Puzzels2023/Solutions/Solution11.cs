using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puzzels2023.Solutions;
internal class Solution11(string path) : SolutionBase(path)
{
    private void CountEmptyLinesInMap()
    {
        long rowLength = _lines.First().Length;

        for (int y = 0; y < _lines.Length; y++)
        {
            string row = _lines[y];

            if (row.Distinct().Count() == 1) 
            {
                emptyHorizontalIndexesOfMap.Add(y);
            }
        }

        for (int x = 0; x < rowLength; x++)
        {
            string verticalRow = "";
            for (int y = 0; y < _lines.Length; y++)
            {
                verticalRow += _lines[y][x];
            }

            if (verticalRow.Distinct().Count() == 1) 
            {
                emptyVerticalRowIndexesOfMap.Add(x);
            }
        }
    }

    private Point[] GetGalaxyLocations()
    {
        List<Point> locations = [];

        for (int y = 0; y < _lines.Length; y++)
        {
            string line = _lines[y];
            for (int x = 0; x < line.Length; x++)
            {
                if (line[x] == '#') {
                    locations.Add(new Point(x, y));
                }
            }
        }

        return [.. locations];
    }

    private long GetShortestPathBetweenPoints(Point point1, Point point2, long expantionFactor)
    {
        long steps = 0;

        while (point1 != point2)
        {
            if (point1.X != point2.X)
            {
                long xDifference = point2.X - point1.X;

                point1.X += xDifference < 0
                    ? -1
                    : 1;

                if (emptyVerticalRowIndexesOfMap.Contains(point1.X))
                {
                    steps += expantionFactor;
                }

                steps++;
            }

            if (point1.Y != point2.Y)
            {
                long yDifference = point2.Y - point1.Y;

                point1.Y += yDifference < 0
                    ? -1
                    : 1;

                if (emptyHorizontalIndexesOfMap.Contains(point1.Y))
                {
                    steps += expantionFactor;
                }

                steps++;
            }
        }

        return steps;
    }

    private readonly List<long> emptyVerticalRowIndexesOfMap = [];
    private readonly List<long> emptyHorizontalIndexesOfMap = [];

    private long GetSumOfDistancesBetweenGalaxies(long universeExpansionIndex) 
    {
        CountEmptyLinesInMap();

        Point[] galaxies = GetGalaxyLocations();

        List<(Point, Point)> pointsDone = [];

        long totalSteps = 0;
        for (long i = 0; i < galaxies.Length; i++)
        {
            Point galaxy = galaxies[i];

            for (long j = 0; j < galaxies.Length; j++)
            {
                if (i == j) { continue; }

                Point galaxy2 = galaxies[j];

                if (pointsDone.Contains((galaxy, galaxy2))) { continue; }

                pointsDone.AddRange([(galaxy, galaxy2), (galaxy2, galaxy)]);

                long steps = GetShortestPathBetweenPoints(galaxy, galaxy2, universeExpansionIndex);
                totalSteps += steps;
            }
        }

        return totalSteps;
    }

    public override string GetFirstSolution()
    {
        return GetSumOfDistancesBetweenGalaxies(1).ToString();
    }

    public override string GetSecondSolution2()
    {
        const long expantion = 1000000 - 1;
        return GetSumOfDistancesBetweenGalaxies(expantion).ToString();
    }
}
