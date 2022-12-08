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
                    if (rowIndex == 5)
                    {
                        Console.WriteLine();
                    }

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
            MapTrees();

            int biggest = 0;

            for (int rowIndex = 0; rowIndex <= _rowLength; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex <= _columnLength; columnIndex++)
                {
                    int score = GetScenicScore(rowIndex, columnIndex);
                    if (score > biggest)
                    {
                        biggest = score;
                    }
                }
            }

            return biggest.ToString();
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

        /// <summary>
        /// Generates lists of trees between the current tree and the edge.
        /// 
        /// list of trees follows the order of current tree to the edge
        /// </summary>
        /// <param name="row">Row coordinates of the current tree</param>
        /// <param name="column">Column coordinates of the current tree</param>
        /// <returns>The trees for every direction. (left, right, top, bottom)</returns>
        private (List<int> left, List<int> right, List<int> top, List<int> bottom) GetTreesFromDirections(int row, int column)
        {
            List<int> left = new(),
                    right = new(),
                    top = new(),
                    bottom =  new();

            // left
            for (int columnIndex = column - 1; columnIndex >= 0; columnIndex--)
                left.Add(GetTree(row, columnIndex));

            // right
            for (int columnIndex = column + 1; columnIndex <= _columnLength; columnIndex++)
                right.Add(GetTree(row, columnIndex));

            // top
            for (int rowIndex = row - 1; rowIndex >= 0; rowIndex--)
                top.Add(GetTree(rowIndex, column));

            // bottom
            for (int rowIndex = row + 1; rowIndex <= _rowLength; rowIndex++)
                bottom.Add(GetTree(rowIndex, column));

            return (left, right, top, bottom);
        }

        private bool IsVisible(int row, int column)
        {
            if (IsOnEdge(row, column)) { return true; }

            byte currentTree = GetTree(row, column);
            byte visibleSides = 4;

            (List<int> left, List<int> right, List<int> top, List<int> bottom) = GetTreesFromDirections(row, column);

            if (left.Max() >= currentTree) { visibleSides--; }
            if (right.Max() >= currentTree) { visibleSides--; }
            if (top.Max() >= currentTree) { visibleSides--; }
            if (bottom.Max() >= currentTree) { visibleSides--; }

            return visibleSides > 0;
        }

        private int GetScenicScore(int row, int column)
        {
            if (IsOnEdge(row, column)) { return 0; }

            int currentTree = GetTree(row, column);

            (List<int> left, List<int> right, List<int> top, List<int> bottom) = GetTreesFromDirections(row, column);

            int leftScore = 0,
                rightScore = 0,
                topScore = 0,
                bottomScore = 0;

            // left
            foreach (int leftTree in left) 
            {
                leftScore++;
                if (leftTree >= currentTree) { break; }
            }

            // right
            foreach (int rightTree in right)
            {
                rightScore++;
                if (rightTree >= currentTree) { break; }
            }

            // top
            foreach (int topTree in top)
            {
                topScore++;
                if (topTree >= currentTree) { break; }
            }

            // bottom
            foreach (int bottomTree in bottom)
            {
                bottomScore++;
                if (bottomTree >= currentTree) { break; }
            }

            return leftScore * rightScore * topScore * bottomScore;
        }
    } 
}
