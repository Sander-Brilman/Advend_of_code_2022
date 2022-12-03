using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advend_of_code_1._1.Puzzles
{
    internal class Day_3 : PuzzleSolution
    {
        public Day_3(StreamReader file) : base(file) { }

        private static readonly List<char> Alphabet = "abcdefghijklmnopqrstuvwxyz".ToCharArray().ToList();

        public override string Puzzle1()
        {
            string line;
            while ((line = InputFile.ReadLine()) != null)
            {
                int middle = line.Length / 2;
                string part1 = line[..middle];
                string part2 = line[middle..];
            }
        }

        public override string Puzzle2()
        {
            return base.Puzzle2();
        }

        private int 
    }
}
