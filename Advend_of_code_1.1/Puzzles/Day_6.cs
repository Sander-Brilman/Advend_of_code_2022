using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advend_of_code_1._1.Puzzles
{
    internal class Day_6 : PuzzleSolution
    {
        public Day_6(StreamReader file) : base(file) { }

        public override string Puzzle1()
        {
            return FindMarkerInRange(4).ToString();
        }

        private static bool ContainsDouble(string input)
        {
            return input.Distinct().Count() != input.Length;
        }

        public override string Puzzle2()
        {
            return FindMarkerInRange(14).ToString();
        }

        private int FindMarkerInRange(int length)
        {
            length--;
            char[] chunk = new char[length];
            InputFile.Read(chunk, 0, length);
            string recentChars = string.Join("", chunk);

            int currentCharNumber, index = length;
            do
            {
                index++;
                currentCharNumber = InputFile.Read();
                recentChars += (char)currentCharNumber;

                if (!ContainsDouble(recentChars))
                {
                    return index;
                }

                recentChars = recentChars.Remove(0, 1);
            } while (currentCharNumber > -1);

            return -1;
        }
    }
}
