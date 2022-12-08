using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advend_of_code_1._1.Puzzles
{
    internal class Day_8 : PuzzleSolution
    {
        public Day_8(StreamReader file) : base(file) { }

        private List<List<byte>> _treeMap = new();
        private int _rowLength;
        private int _columnLength;

        public override string Puzzle1()
        {
            MapTrees();

            int total = 0;

            for (int rowIndex = 0; rowIndex <= _rowLength; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex <= _columnLength; columnIndex++)
                {
                    if (!IsVisible(rowIndex, columnIndex))
                    {
                        continue;
                    }    
                    total++;
                }
            }

            return total.ToString();
        }

        public override string Puzzle2()
        {
            return base.Puzzle1();
        }

        private void MapTrees()
        {
            string line;
            while ((line = InputFile.ReadLine()) != null)
            {
                char[] trees = line.ToCharArray();
                List<byte> treeList = new();

                foreach (char tree in trees)
                {
                    treeList.Add(byte.Parse(tree.ToString()));
                }

                _treeMap.Add(treeList);
            }
            _rowLength = _treeMap.Count - 1;
            _columnLength = _treeMap[0].Count - 1;
        }

        private byte GetTree(int row, int column)
        {
            return _treeMap[row][column];
        }

        private bool IsOnEdge(int row, int column)
        {
            bool isVisible = row == 0 ||
                column == 0 ||
                _treeMap.Count - 1 == row ||
                _treeMap[0].Count - 1 == column;

            return isVisible;
        }

        private bool IsVisible(int row, int column)
        {
            if (IsOnEdge(row, column))
            {
                return false;
            }

            byte currentTree = GetTree(row, column);
            byte visibleSides = 4;

            //
            // left
            //
            for (int rowIndex = 0; rowIndex < row; rowIndex++)
            {
                byte tree = GetTree(rowIndex, column);
                if (tree >= currentTree)
                {
                    visibleSides--;
                    break;
                }
            }

            if (row == 2 && column == 1)
            {
                Console.WriteLine();
            }

            //
            // right
            //
            for (int rowIndex = row + 1; rowIndex <= _rowLength; rowIndex++)
            {
                byte tree = GetTree(rowIndex, column);
                if (tree >= currentTree)
                {
                    visibleSides--;
                    break;
                }
            }

            //
            // top
            //
            for (int columnIndex = 0; columnIndex < row; columnIndex++)
            {
                byte tree = GetTree(row, columnIndex);
                if (tree >= currentTree)
                {
                    visibleSides--;
                    break;
                }
            }

            //
            // bottom
            //
            for (int columnIndex = column; columnIndex <= _columnLength; columnIndex++)
            {
                byte tree = GetTree(row, columnIndex);
                if (tree >= currentTree)
                {
                    visibleSides--;
                    break;
                }
            }

            return visibleSides > 0;
        }
    }
}
