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

        private static readonly bool _map = false;

        private int _headLocationX = 0;
        private int _headLocationY = 10;

        private int _tailLocationX = 0;
        private int _tailLocationY = 10;

        private HashSet<string> _tailLocaions = new();

        public override string Puzzle1()
        {
            PrintMap();
            SaveLocation();
            string line;
            while ((line = InputFile.ReadLine()) != null)
            {
                string[] movementInfo = line.Split(' ');
                MoveHead(int.Parse(movementInfo[1]), movementInfo[0][0]);



            }

            return _tailLocaions.Count.ToString();
        }

        public override string Puzzle2()
        {
            return base.Puzzle2();
        }

        private void MoveHead(int amount, char directon)
        {
            for (int i = 0; i < amount; i++)
            {
                switch (directon)
                {
                    case 'L':// left
                        _headLocationX--;
                        break;

                    case 'R':// right
                        _headLocationX++;
                        break;

                    case 'U':// up
                        _headLocationY--;
                        break;

                    case 'D':// down
                        _headLocationY++;
                        break;
                }
                MoveTail();

                if (_map)
                {
                    Console.WriteLine($"{directon} {amount} ({i + 1}/{amount}) - {_tailLocaions.Count} tail places");
                    Console.WriteLine();
                }

                PrintMap();
            }
        }

        private void MoveTail()
        {
            int distanceX = _headLocationX - _tailLocationX;
            int absDistanceX = Math.Abs(distanceX);

            int distanceY = _headLocationY - _tailLocationY;
            int absDistanceY = Math.Abs(distanceY);

            if (absDistanceX + absDistanceY == 3)
            {
                int increaseX = distanceX + (absDistanceX == 1 ? 0 : GetInt(distanceX));
                int increaseY = distanceY + (absDistanceY == 1 ? 0 : GetInt(distanceY));
                _tailLocationX += increaseX;
                _tailLocationY += increaseY;
            }
            else if (absDistanceX > 1 || absDistanceY > 1)
            {
                int increaseX = distanceX + GetInt(distanceX);
                int increaseY = distanceY + GetInt(distanceY);
                _tailLocationX += distanceX == 0 ? 0 : increaseX;
                _tailLocationY += distanceY == 0 ? 0 : increaseY;
            }
            SaveLocation();
        }

        private void SaveLocation()
        {
            _tailLocaions.Add($"{_tailLocationX} {_tailLocationY}");
        }

        private void PrintMap()
        {
            if (!_map)
            {
                return;
            }

            for (int i = 0; i < 20; i++)
            {
                Console.Write($"{i:D3} ");
                for (int j = -10; j < 10; j++)
                {
                    char character = '.';
                    if (i == _headLocationY && j == _headLocationX)
                    {
                        character = 'H';
                    }
                    else if (i == _tailLocationY && j == _tailLocationX)
                    {
                        character = 'T';
                    }
                    Console.Write(character);
                }
                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine();
        }

        private int GetInt(int input)
        {
            return input < 0 ? 1 : -1;
        }
    }
}
