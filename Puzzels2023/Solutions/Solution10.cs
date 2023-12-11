using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.NetworkInformation;
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

    private IEnumerable<Pipe> GetConnectedPipes(Pipe pipe)
    {
        return pipe.FacingLocations
            .Select(GetPipe)
            .Where(p => p != null && ArePipesConnected(p, pipe));
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

            Pipe[] connectedPipes = GetConnectedPipes(currentPipe)
                .Where(p => pipes.Any(pp => pp.Location == p.Location) is false)
                .ToArray();

            if (connectedPipes.Length == 0)
            {
                break;
            }

            pipes.Add(connectedPipes.First());
        }

        return [.. pipes];
    }

    private Location GetStartLocation()
    {
        for (int y = 0; y < _lines.Length; y++)
        {
            int lineLength = _lines[y].Length;
            for (int x = 0; x < lineLength; x++)
            {
                if (_lines[y][x] == 'S')
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

    private enum HorizontalLineDirection 
    { 
        Up, 
        Down 
    }

    private static char FindReplacementForStartCharacter(Location startLocation, Pipe[] pipesInLoop)
    {
        Pipe[] pipesAttachedToStart = pipesInLoop
            .Where(p => p.FacingLocations.Contains(startLocation))
            .ToArray();

        Pipe pipe1 = pipesAttachedToStart[0];
        Pipe pipe2 = pipesAttachedToStart[1];

        bool isTopConnected = pipe1.Location.Y == startLocation.Y - 1 || pipe2.Location.Y == startLocation.Y - 1;
        bool isBottomConnected = pipe1.Location.Y == startLocation.Y + 1 || pipe2.Location.Y == startLocation.Y + 1;

        bool isLeftConnected = pipe1.Location.X == startLocation.X - 1 || pipe2.Location.X == startLocation.X - 1;
        bool isRightConnected = pipe1.Location.X == startLocation.X + 1 || pipe2.Location.X == startLocation.X + 1;

        if (isTopConnected && isBottomConnected) 
        {
            return '|'; 
        }
        else if (isLeftConnected && isRightConnected)
        {
            return '-';
        }
        else if (isTopConnected && isRightConnected)
        {
            return 'L';
        }
        else if (isTopConnected && isLeftConnected)
        {
            return 'J';
        }
        else if (isBottomConnected && isRightConnected)
        {
            return 'F';
        }
        else if (isBottomConnected && isLeftConnected)
        {
            return '7';
        }

        throw new UnreachableException();
    }

    enum CornerDirection
    {
        Top,
        Bottom,
    }

    public override string GetSecondSolution2()
    {
        Location startLocation = GetStartLocation();

        Pipe[] pipesInLoop = GetLoopStartingFromLocation(startLocation);

        int totalSquaresInLoop = 0;
        int lineLength = _lines.First().Length;
        bool isCounting = false;

        for (int y = 0; y < _lines.Length; y++)
        {
            for (int x = 0; x < lineLength; x++)
            {
                char lineChar = _lines[y][x];

                bool isPartOfMainLoop = pipesInLoop.Any(p => p.Location.X == x && p.Location.Y == y);


                if (lineChar == 'S')
                {
                    lineChar = FindReplacementForStartCharacter(startLocation, pipesInLoop);
                    _lines[y] = _lines[y].Replace('S', lineChar);
                }

                if (lineChar == '|' && isPartOfMainLoop)
                {
                    isCounting = !isCounting;
                    continue;
                }

                if (lineChar == '-' && isPartOfMainLoop) { continue; }
                
                // other pipe parts such as corner pices
                if (isPartOfMainLoop)
                {
                    bool endOfHorizontalLine = lineChar == 'J' || lineChar == '7';

                    if (endOfHorizontalLine)
                    {
                        char startOfhorizontalLine = _lines[y]
                            .Take(x)
                            .Reverse()
                            .SkipWhile(x => x == '-')
                            .First();

                        CornerDirection endDirection = lineChar == '7' ? CornerDirection.Bottom : CornerDirection.Top;
                        CornerDirection startDirection = startOfhorizontalLine == 'F' ? CornerDirection.Bottom : CornerDirection.Top;

                        if (endDirection != startDirection)
                        {
                            isCounting = !isCounting;
                        }
                    }

                    continue;
                }

                if (isCounting)
                {
                    totalSquaresInLoop++;
                }
            }
        }

        return totalSquaresInLoop.ToString();
    }
}
