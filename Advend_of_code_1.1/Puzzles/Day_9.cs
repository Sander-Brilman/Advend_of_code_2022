using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advend_of_code_1._1.Puzzles
{
    internal class Day_9 : PuzzleSolution
    {
        public Day_9(StreamReader file) : base(file) { }

        private bool _showMap = false;

        private HashSet<string> _tailLocaions = new();

        private List<KnotLocation> _knots = new();

        public override string Puzzle1()
        {
            CreateKnots(2);
            
            PrintMap();

            SaveLocation();

            FollowMovementInstructions();

            return _tailLocaions.Count.ToString();
        }

        public override string Puzzle2()
        {
            CreateKnots(10);
            
            PrintMap();

            SaveLocation();

            FollowMovementInstructions();

            return _tailLocaions.Count.ToString();
        }

        private void CreateKnots(int length)
        {
            int startLocationX = 10;
            int startLocationY = 10;

            _knots.Add(new KnotLocation(startLocationX, startLocationY, 'H'));

            length -= 2;
            for (int i = 1; i <= length; i++)
            {
                _knots.Add(new KnotLocation(startLocationX, startLocationY, i.ToString()[0]));
            }

            _knots.Add(new KnotLocation(startLocationX, startLocationY, 'T'));
        }

        private void FollowMovementInstructions()
        {
            string line;
            while ((line = InputFile.ReadLine()) != null)
            {
                string[] movementInfo = line.Split(' ');
                MoveHead(int.Parse(movementInfo[1]), movementInfo[0][0]);
            }
        }

        private void MoveHead(int amount, char directon)
        {
            KnotLocation head = _knots.First();
            for (int i = 0; i < amount; i++)
            {
                switch (directon)
                {
                    case 'L':// left
                        head.X--;
                        break;

                    case 'R':// right
                        head.X++; 
                        break;

                    case 'U':// up
                        head.Y--;
                        break;

                    case 'D':// down
                        head.Y++;
                        break;
                }

                MoveKnots();

                if (_showMap)
                {
                    Console.WriteLine($"{directon} {amount} ({i + 1}/{amount}) - {_tailLocaions.Count} tail places");
                    Console.WriteLine();
                }

                PrintMap();
            }
        }

        private void MoveKnots()
        {
            int knotsLength = _knots.Count - 1;
            for (int i = 0; i < knotsLength; i++)
            {
                KnotLocation knot1 = _knots[i];
                KnotLocation knot2 = _knots[i + 1];

                int distanceX = knot1.X - knot2.X;
                int absDistanceX = Math.Abs(distanceX);

                int distanceY = knot1.Y - knot2.Y;
                int absDistanceY = Math.Abs(distanceY);

                if (absDistanceX + absDistanceY == 3)
                {
                    int increaseX = distanceX + (absDistanceX == 1 ? 0 : GetInt(distanceX));
                    int increaseY = distanceY + (absDistanceY == 1 ? 0 : GetInt(distanceY));
                    knot2.X += increaseX;
                    knot2.Y += increaseY;
                }
                else if (absDistanceX > 1 || absDistanceY > 1)
                {
                    int increaseX = distanceX + GetInt(distanceX);
                    int increaseY = distanceY + GetInt(distanceY);
                    knot2.X += distanceX == 0 ? 0 : increaseX;
                    knot2.Y += distanceY == 0 ? 0 : increaseY;
                }
            }
            SaveLocation();
        }

        private void SaveLocation()
        {
            _tailLocaions.Add($"{_knots.Last().X} {_knots.Last().Y}");
        }

        private void PrintMap()
        {
            if (!_showMap)
            {
                return;
            }

            for (int i = 0; i < 27; i++)
            {
                Console.Write($"{i:D3} ");
                for (int j = 0; j < 21; j++)
                {
                    char character = '.';
                    
                    foreach (KnotLocation knot in _knots)
                    {
                        if (knot.X == j && knot.Y == i)
                        {
                            character = knot.Name[0];
                            break;
                        }
                    }
                    
                    Console.Write(character);
                }
                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine();
        }

        private static int GetInt(int input)
        {
            return input < 0 ? 1 : -1;
        }
    }

    class KnotLocation
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Name { get; set; }

        public KnotLocation(int x, int y, char name)
        {
            X = x;
            Y = y;
            Name = name.ToString();
        }
    }
}
