using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advend_of_code_1._1.Puzzles
{
    internal class Day_4 : PuzzleSolution
    {
        public Day_4(StreamReader file) : base(file) { }

        public override string Puzzle1()
        {
            int total = 0;
            string line;
            while ((line = InputFile.ReadLine()) != null)
            {
                List<string> elfsStrings = line.Split(',').ToList();

                string elf1 = $" {string.Join(" ", GetAllElfTasks(elfsStrings[0]))} ";
                string elf2 = $" {string.Join(" ", GetAllElfTasks(elfsStrings[1]))} ";

                if (elf1.Contains(elf2) || elf2.Contains(elf1))
                {
                    total++;
                }
            }
            return total.ToString();
        }

        public override string Puzzle2()
        {
            int total = 0;
            string line;
            while ((line = InputFile.ReadLine()) != null)
            {
                List<string> elfsStrings = line.Split(',').ToList();
                List<List<int>> elfs = new()
                {
                    GetAllElfTasks(elfsStrings[0]),
                    GetAllElfTasks(elfsStrings[1])
                };
                elfs = elfs.OrderBy(x => x.Count).ToList();

                foreach (int elf1Task in elfs[0])
                {
                    if (elfs[1].Contains(elf1Task))
                    {
                        total++;
                        break;
                    }
                }
            }
            return total.ToString();
        }

        private static (int min, int max) GetTaskLimits(string elf)
        {
            List<string> tasksStr = elf.Split("-").ToList();
            return (int.Parse(tasksStr[0]), int.Parse(tasksStr[1]));
        }

        private static List<int> GetAllElfTasks(string elf) {
            (int min, int max) = GetTaskLimits(elf);
            List<int> tasks = new();

            for (int counter = min; counter <= max; counter++)
            {
                tasks.Add(counter);
            }

            return tasks;
        }

        private static bool AreOverlapping(string elf1, string elf2)
        {
            (int elf1Min, int elf1Max) = GetTaskLimits(elf1);
            (int elf2Min, int elf2Max) = GetTaskLimits(elf2);


            return false;
        }


    }
}
