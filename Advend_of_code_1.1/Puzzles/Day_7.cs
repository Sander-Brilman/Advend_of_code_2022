using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advend_of_code_1._1.Puzzles
{
    internal class Day_7 : PuzzleSolution
    {
        public Day_7(StreamReader file) : base(file) { }

        private ArrayList _root = new();
        private List<int> _path = new();

        public override string Puzzle1()
        {
            string line;
            bool isListing = false;
            ArrayList currentDir = 
            while ((line = InputFile.ReadLine()) != null)
            {
                if (isListing)
                {

                }
            }
        }

        public override string Puzzle2()
        {
            return base.Puzzle2();
        }

        private ArrayList GetDirFromPath()
        {
            ArrayList dir = _root;

            if (_path.Count > 0)
            {
                foreach (int pathIndex in _path)
                {
                    var item = dir[pathIndex];

                    if (item.GetType() == typeof(ArrayList))
                    {
                        dir = (ArrayList)item;
                    }
                }
            }

            return dir;
        }

        private ArrayList MoveToDir(string dirName)
        {
            if (dirName == "..")
            {
                _path.RemoveAt(_path.Count - 1);
                return GetDirFromPath();
            }

            ArrayList currentDir = GetDirFromPath();
            int index = currentDir.IndexOf(dirName);
            _path.Add(index);
            currentDir = (ArrayList)currentDir[index];

            return currentDir;
        }
    }
}
