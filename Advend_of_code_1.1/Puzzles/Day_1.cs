using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advend_of_code_1._1.Puzzles
{
    internal class Day_1 : PuzzleSolution
    {
        public Day_1(StreamReader inputFile) : base(inputFile) { }

        public override string Puzzle1()
        {
            return GetCalories().Max().ToString();
        }

        public override string Puzzle2()
        {
            List<int> calories = GetCalories();
            calories.Sort();
            calories.Reverse();

            return (calories[0] + calories[1] + calories[2]).ToString();
        }

        private List<int> GetCalories()
        {
            string line;
            int index = 0;
            List<int> calories = new() { 0 };

            while ((line = InputFile.ReadLine()) != null)
            {
                if (line == "")
                {
                    index++;
                    calories.Add(0);
                    continue;
                }

                calories[index] += int.Parse(line);
            }

            return calories;
        }
    }
}
