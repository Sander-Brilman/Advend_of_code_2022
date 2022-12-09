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

        private int _headLocationX = 0;
        private int _headLocationY = 0;

        private int _tailLocationX = 0;
        private int _tailLocationY = 0;

        private HashSet<string> _tailLocaions = new() { "00" };

        public override string Puzzle1()
        {
            string line;
            while ((line = InputFile.ReadLine()) != null)
            {
                string[] movementInfo = line.Split(' ');
                MoveHead(int.Parse(movementInfo[1]), movementInfo[0][0]);
            }

            return "oeleh";
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
                        _headLocationY++;
                        break;

                    case 'D':// down
                        _headLocationY--;
                        break;
                }
                MoveTail();
            }
        }

        private void MoveTail()
        {
            int distanceX = _headLocationX - _tailLocationX;
            int distanceY = _headLocationY - _tailLocationY;

            if (Math.Abs(distanceX) > 1 || Math.Abs(distanceY) > 1)
            {
                _tailLocationX += distanceX == 0 ? 0 : distanceX - 1;
                _tailLocationY += distanceY == 0 ? 0 : distanceY - 1;
            }

            _tailLocaions.Add($"{_tailLocationX}{_tailLocationY}");
        }
    }
}
