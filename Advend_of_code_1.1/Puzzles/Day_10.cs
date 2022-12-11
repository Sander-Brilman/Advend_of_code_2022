using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advend_of_code_1._1.Puzzles
{
    internal class Day_10 : PuzzleSolution
    {
        public Day_10(StreamReader file) : base(file) { }

        private Dictionary<int, int> _cycleValues = new();
        private List<string> _lines = new();

        public override string Puzzle1()
        {
            StoreCycleValues();

            int total = 0;
            List<int> requestedCycles = new()
            {
                19,
                59,
                99,
                139,
                179,
                219,
            };

            foreach (KeyValuePair<int, int> cycleValue in _cycleValues)
            {
                if (requestedCycles.Count == 0)
                {
                    break;
                }

                if (cycleValue.Key == requestedCycles.First())
                {
                    int value = (requestedCycles.First() + 1) * cycleValue.Value;
                    Console.WriteLine($"{requestedCycles.First() + 1} * {cycleValue.Value} = {value}");
                    total += value;
                    requestedCycles.Remove(cycleValue.Key);
                }
            }

            return total.ToString();
        }

        public override string Puzzle2() 
        {
            for (int i = 0; i < 240; i++)
            {
                if (i % 40 == 0)
                {
                    Console.WriteLine();
                }



            }
            return "e";
        }

        private void StoreCycleValues()
        {
            string line;
            int linesAmount, i, cycles = 0, value = 1;
            

            while ((line = InputFile.ReadLine()) != null)
                _lines.Add(line);


            linesAmount = _lines.Count;
            for (i = 0; i < linesAmount + 2; i++)
            {
                line = linesAmount > i ? _lines[i] : "noop";
                cycles++;

                _cycleValues.Add(cycles, value);

                if (line == "noop")
                {
                    continue;
                }

                value += int.Parse(line.Replace("addx ", ""));
                cycles++;

                _cycleValues.Add(cycles, value);
            }
        }

        private static int ProcessCommand(int value, string command) 
        {
            return value + (command == "noop" ? 0 : int.Parse(command.Replace("addx ", "")));
        }
    }
}
