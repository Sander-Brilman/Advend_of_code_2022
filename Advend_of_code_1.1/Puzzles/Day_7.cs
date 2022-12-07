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

        private Dictionary<string, object> _root = new();
        private List<string> _path = new();

        private List<int> _dirsInRange = new();

        public override string Puzzle1()
        {
            ParseInput();
            MapDirSizes(_root, 0, 100000);
            return _dirsInRange.Sum().ToString();
        }

        public override string Puzzle2()
        {
            ParseInput();
            MapDirSizes(_root, 0, null);
            int takenSize = _dirsInRange.Max();
            int maxSize = 70000000;
            int availableSize = maxSize - takenSize;
            int minClearSize = 30000000 - availableSize;

            _dirsInRange.Clear();
            MapDirSizes(_root, minClearSize, null);


            return _dirsInRange.Min().ToString();
        }

        private void ParseInput()
        {
            string line;
            Dictionary<string, object> currentDir = _root;
            while ((line = InputFile.ReadLine()) != null)
            {
                // execute commands
                if (line.StartsWith('$'))
                {
                    string command = line[2..];

                    if (command.StartsWith("cd"))
                    {
                        currentDir = MoveToDir(command[3..]);
                    }

                    continue;
                }


                // save the listed files in the file system
                if (line.StartsWith("dir"))
                {
                    string dirName = line[4..];
                    currentDir.Add(dirName, new Dictionary<string, object>());
                }
                else
                {
                    string[] fileInfo = line.Split(' ');
                    currentDir.Add(fileInfo[1], int.Parse(fileInfo[0]));
                }

            }
        }

        private Dictionary<string, object> GetDirFromPath(List<string> path)
        {
            if (path.Count == 0) { return _root; }

            Dictionary<string, object> layer = (Dictionary<string, object>)_root[path[0]];

            if (path.Count == 1) { return layer; }

            for (int i = 1; i < path.Count; i++)
            {

                layer = (Dictionary<string, object>)layer[path[i]];
            }

            return layer;
        }

        private Dictionary<string, object> MoveToDir(string dirName)
        {
            switch (dirName)
            {
                case "..":
                    _path.RemoveAt(_path.Count - 1);
                    break;
                case "/":
                    _path.Clear();
                    break;
                default:
                    _path.Add(dirName);
                    break;
            }
            return GetDirFromPath(_path);
        }

        /// <summary>
        /// Gets the total size of the directory only counting the files.
        /// Saves the directories with a size within the limits to the _dirsInRange propperty
        /// 
        /// Uses recursion to map underlying dirs
        /// </summary>
        /// <param name="path">The path of the directory</param>
        /// <returns>The total size</returns>
        private int MapDirSizes(Dictionary<string, object> startDir, int minSize, int? maxSize)
        {
            int total = 0;

            foreach (KeyValuePair<string, object> dirItem in startDir)
            {
                if (dirItem.Value.GetType() == typeof(int))
                {
                    total += (int)dirItem.Value;
                    continue;
                }
                if (dirItem.Value.GetType() == typeof(Dictionary<string, object>))
                {
                    total += MapDirSizes((Dictionary<string, object>)startDir[dirItem.Key], minSize, maxSize);
                    continue;
                }
            }

            if (total > minSize && (maxSize == null || total < maxSize))
            {
                _dirsInRange.Add(total);
            }

            return total;
        }
    }
}
