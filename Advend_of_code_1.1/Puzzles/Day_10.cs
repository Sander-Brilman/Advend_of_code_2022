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
        public Day_10(StreamReader file) : base(file) { StoreCycleValues(); }

        private List<Dictionary<string, int>> _cycleValues = new();
        private List<string> _lines = new();

        public override string Puzzle1()
        {
            int total = 0;
            List<int> requestedCycles = new()
            {
                20,
                60,
                100,
                140,
                180,
                220,
            };

            foreach (int cycle in requestedCycles)
            {
                int cycleValue = _cycleValues[cycle - 1]["during"];
                int value = cycle * cycleValue;
                Console.WriteLine($"{cycle} * {cycleValue} = {value}");
                total += value;
            }

            return total.ToString();
        }

        public override string Puzzle2() 
        {
            for (int pixels = 0; pixels < 240; pixels++)
            {
                if (pixels % 40 == 0) {  Console.WriteLine(); }
                int horizontalPixel = pixels % 40;

                Dictionary<string, int> cycleStages = _cycleValues[pixels];

                char printChar = (cycleStages["during"] >= horizontalPixel - 1 && cycleStages["during"] <= horizontalPixel + 1)
                    ? '#'
                    : '.';

                Console.Write(printChar);
            }
            return "";
        }

        private void StoreCycleValues()
        {
            string line;
            int value = 1;

            while ((line = InputFile.ReadLine()) != null)
            {
                //
                // cycle 1
                //
                _cycleValues.Add(new Dictionary<string, int>()
                {
                    {"during", value},
                    {"end", value},
                });

                if (line == "noop") { continue; }

                //
                // add extra cycle if command is addx
                //
                _cycleValues.Add(new Dictionary<string, int>(){{"during", value}});
                value += int.Parse(line.Replace("addx ", ""));
                _cycleValues.Last().Add("end", value);
            }
        }
    }
}
