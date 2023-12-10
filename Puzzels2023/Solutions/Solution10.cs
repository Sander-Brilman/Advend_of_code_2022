using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Puzzels2023.Solutions;
internal class Solution10(string path) : SolutionBase(path)
{

    private record Location(int X, int Y);

    private record Pipe(Location Location, Location[] FacingLocations)
    {
        public static Pipe? FromChar(char input, Location location)
        {
            if (input == '.') { return null; }

            Location[] facingLocations = input switch
            {
                '|' => [new Location(location.X, location.Y + 1), new Location(location.X, location.Y - 1)],
                '-' => [new Location(location.X + 1, location.Y), new Location(location.X - 1, location.Y)],

                'L' => [new Location(location.X + 1, location.Y), new Location(location.X, location.Y - 1)],
                'J' => [new Location(location.X - 1, location.Y), new Location(location.X, location.Y - 1)],
                '7' => [new Location(location.X - 1, location.Y), new Location(location.X, location.Y + 1)],
                'F' => [new Location(location.X + 1, location.Y), new Location(location.X, location.Y + 1)],
                'S' => [
                    new Location(location.X - 1, location.Y),
                    new Location(location.X + 1, location.Y),

                    new Location(location.X, location.Y + 1),
                    new Location(location.X, location.Y - 1),
                ],

                _ => throw new UnreachableException()
            };

            return new Pipe(location, facingLocations);
        }
    }

    private bool IsLocationWithinBounds(Location location) =>
        location.Y < _lines.Length &&
        location.X < _lines[0].Length &&
        location.Y >= 0 &&
        location.X >= 0;


    private Pipe? GetPipe(Location location)
    {
        if (IsLocationWithinBounds(location) is false)
        {
            return null;
        }

        char pipeRaw = _lines[location.Y][location.X];

        return Pipe.FromChar(pipeRaw, location);
    }

    private Pipe[] GetConnectedPipes(Pipe pipe)
    {
        return pipe.FacingLocations
            .Select(GetPipe)
            .Where(p => p != null && ArePipesConnected(p, pipe))
            .ToArray()!;
    }

    private static bool ArePipesConnected(Pipe? pipe1, Pipe? pipe2)
    {
        return
            pipe1 is not null &&
            pipe2 is not null &&
            pipe1.FacingLocations
                .Contains(pipe2.Location) &&
            pipe2.FacingLocations
                .Contains(pipe1.Location);
    }

    private Pipe[] GetLoopStartingFromLocation(Location location)
    {
        Pipe startingPipe = GetPipe(location) ?? throw new Exception("starting location is not a pipe");
        List<Pipe> pipes = 
            [
                startingPipe,
                GetConnectedPipes(startingPipe).First()
            ];


        while (true)
        {
            Pipe currentPipe = pipes.Last();

            Pipe[] connectedPipes = GetConnectedPipes(currentPipe);

            if (connectedPipes.Length == 0) { throw new Exception("pipe has no connections!"); }

            Pipe[] unDiscoveredPipes = connectedPipes
                .Where(p => pipes.Any(pp => pp.Location == p.Location) is false)
                .ToArray();

            if (unDiscoveredPipes.Length == 0)
            {
                break;
            }

            pipes.Add(unDiscoveredPipes.First());
        }

        return [.. pipes];
    }

    private Location GetStartLocation()
    {
        for (int y = 0; y < _lines.Length; y++)
        {
            var line = _lines[y];
            for (int x = 0; x < line.Length; x++)
            {
                if (line[x] == 'S')
                {
                    return new(x, y);
                }
            }
        }

        return new(0, 0);
    }

    public override string GetFirstSolution()
    {
        Location startLocation = GetStartLocation();

        Pipe[] pipesInLoop = GetLoopStartingFromLocation(startLocation);

        return ((pipesInLoop.Length / 2)).ToString();
    }

    public override string GetSecondSolution2()
    {
        Location startLocation = GetStartLocation();

        Pipe[] pipesInLoop = GetLoopStartingFromLocation(startLocation);

        int totalSquaresInLoop = 0;

        bool isInsideLoop = false;
        bool pointerIsOnHorizontalLine = false;

        for (int y = 0; y < _lines.Length; y++)
        {
            var line = _lines[y];

            if (line.Contains('|') is false) { continue; }

            isInsideLoop = false;
            for (int x = 0; x < line.Length; x++)
            {
                Location location = new(x, y);
                var lineChar = line[x];


                Pipe currentPipe = GetPipe(location)!;
                Pipe? nextPotentialPipe = GetPipe(new Location(x + 1, y));
                Pipe? previousPotentialPipe = GetPipe(new Location(x - 1, y));


                bool currentLocationIsPartOfLoop = pipesInLoop.Any(p => p.Location == location);

                bool previousPipeIsConnected = ArePipesConnected(currentPipe, previousPotentialPipe);
                bool nextPipeIsConnected = ArePipesConnected(nextPotentialPipe, currentPipe);
                bool isPartOfHorizontalLine = (nextPipeIsConnected || previousPipeIsConnected);


                if (currentLocationIsPartOfLoop)
                {
                    if (lineChar == '|')
                    {
                        isInsideLoop = !isInsideLoop;

                        continue;
                    }

                    if (isPartOfHorizontalLine)
                    {
                        int amountOfVerticalPipesBefore = line
                            .Take(x + 1)
                            .Where(c => c == '|')
                            .Count();

                        // end of the horizontal line
                        if (amountOfVerticalPipesBefore > 0 && nextPipeIsConnected is false)
                        {
                            isInsideLoop = amountOfVerticalPipesBefore % 2 == 0
                                ? false
                                : true;
                        }

                        continue;
                    }

                    continue;
                }

                if (isInsideLoop && currentLocationIsPartOfLoop is false)
                {
                    totalSquaresInLoop++;
                }
            }
        }

        return totalSquaresInLoop.ToString();
    }
}
