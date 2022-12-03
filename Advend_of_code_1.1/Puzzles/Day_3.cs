using System;
using System.Collections.Generic;
using System.Globalization;
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
            int total = 0;
            string line;
            while ((line = InputFile.ReadLine()) != null)
            {
                int middle = line.Length / 2;
                string part1 = line[..middle];
                string part2 = line[middle..];

                List<char> part1Chars = part1.ToCharArray().ToList();

                foreach (char part in part1Chars)
                {
                    if (!part2.Contains(part))
                    {
                        continue;
                    }

                    total += GetLetterScore(part);
                    break;
                }
            }

            return total.ToString();
        }

        public override string Puzzle2()
        {
            int total = 0;

            string line;
            List<string> lines = new();
            while ((line = InputFile.ReadLine()) != null)
            {
                lines.Add(line);
            }

            for (int index = 0; index < lines.Count; index += 3)
            {
                List<string> backpacks = new()
                {
                    lines[index],
                    lines[index + 1],
                    lines[index + 2]
                };
                backpacks = backpacks.OrderBy(x => x.Length).ToList();

                List<char> commonChars = backpacks[0].ToCharArray().ToList();
                backpacks.RemoveAt(0);

                foreach(string backpack in backpacks)
                {
                    int length = commonChars.Count;
                    for (int commonCharindex = length - 1; commonCharindex >= 0; commonCharindex--)
                    {
                        char commonChar = commonChars[commonCharindex];
                        if (backpack.Contains(commonChar))
                        {
                            continue;
                        }

                        commonChars.Remove(commonChar);
                    }


                }

                total += GetLetterScore(commonChars[0]);
            }

            return total.ToString();

        }

        private static int GetLetterScore(char letter)
        {
            int score = Alphabet.IndexOf(char.ToLower(letter)) + 1;
            score += char.IsUpper(letter) ? 26 : 0;
            return score;
        }
    }
}
