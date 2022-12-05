using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advend_of_code_1._1.Puzzles
{
    internal class Day_5 : PuzzleSolution
    {
        public Day_5(StreamReader file) : base(file) { }

        private List<List<string>> _stacks = new();

        public override string Puzzle1()
        {
            string line;

            ImportStacksFromReader();

            // follow the movement instuctions
            while ((line = InputFile.ReadLine()) != null)
            {
                (int amount, int fromStack, int toStack) = GetMovementInfoFromLine(line);

                for (int counter = 0; counter < amount; counter++)
                {
                    var crate = _stacks[fromStack][0];
                    _stacks[fromStack].Remove(crate);
                    _stacks[toStack] = _stacks[toStack].Prepend(crate).ToList();
                }
            }

            return GetOutputString();
        }

        public override string Puzzle2()
        {
            string line;

            ImportStacksFromReader();

            // follow the movement instuctions
            while ((line = InputFile.ReadLine()) != null)
            {
                (int amount, int fromStack, int toStack) = GetMovementInfoFromLine(line);

                List<string> crates = _stacks[fromStack].GetRange(0, amount);
                crates.Reverse();

                _stacks[fromStack].RemoveRange(0, amount);

                _stacks[toStack].Reverse();
                _stacks[toStack].AddRange(crates);
                _stacks[toStack].Reverse();
            }

            return GetOutputString();
        }

        /// <summary>
        /// Process all stacks from the input file into the stacks list proppery.
        /// </summary>
        private void ImportStacksFromReader()
        {
            string line;
            while ((line = InputFile.ReadLine()) != null)
            {
                if (!line.Contains('['))
                {
                    InputFile.ReadLine();
                    break;
                }

                AddStacksFromLine(line);
            }
        }

        /// <summary>
        /// Add the crates in 1 stack line to the total list.
        /// </summary>
        /// <param name="line">The input line containing the crates</param>
        private void AddStacksFromLine(string line)
        {
            StringBuilder input = new(line);
            input
                .Replace(" [", "|[")
                .Replace("] ", "]|")
                .Replace("    ", "   |");
            List<string> crates = input.ToString().Split('|').ToList();

            for (int index = 0; index < crates.Count; index++)
            {
                StringBuilder crate = new(crates[index]);
                if (index >= _stacks.Count)
                {
                    _stacks.Add(new List<string>());
                }

                if (!crate.ToString().Contains('[')) {
                    continue;
                }

                crate
                    .Replace("[", "")
                    .Replace("]", "");

                _stacks[index].Add(crate.ToString());
            }
        }

        /// <summary>
        /// Return the crates movement info from the line input
        /// </summary>
        /// <param name="line">The line containing the input in text format</param>
        /// <returns>The amount, and the 2 stacks to preform the operations on</returns>
        private static (int amount, int fromStack, int toStack) GetMovementInfoFromLine(string line)
        {
            List<string> movementInfo = line.Split(' ').ToList();

            return (int.Parse(movementInfo[1]), int.Parse(movementInfo[3]) - 1, int.Parse(movementInfo[5]) - 1);
        }

        /// <summary>
        /// Generate the output sting for the puzzle based on the current crate stack
        /// </summary>
        /// <returns>The sting ready to be printed</returns>
        private string GetOutputString()
        {
            string output = "";
            foreach (List<string> stack in _stacks)
            {
                output += stack.First();
            }
            return output;
        }
    }
}
