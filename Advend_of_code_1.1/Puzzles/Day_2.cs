using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advend_of_code_1._1.Puzzles
{
    internal class Day_2 : PuzzleSolution
    {
        public Day_2(StreamReader input) : base(input) { }

        public override string Puzzle1()
        {
            string line;
            int totalScore = 0;

            while ((line = InputFile.ReadLine()) != null)
            {
                int oppomentChoice = TranslateChar(line[0]) ?? throw new Exception("unknown char");
                int myChoice = TranslateChar(line[2]) ?? throw new Exception("unknown char");

                totalScore += GetGameScore(myChoice, oppomentChoice);
            }

            return totalScore.ToString();
        }

        public override string Puzzle2()
        {
            string line;
            int totalScore = 0;

            while ((line = InputFile.ReadLine()) != null)
            {
                int oppomentChoice = TranslateChar(line[0]) ?? throw new Exception("unknown char");
                int myChoice = GetCorrectShape(oppomentChoice, line[2]);

                totalScore += GetGameScore(myChoice, oppomentChoice);
            }

            return totalScore.ToString();

        }


        /// <summary>
        /// Converts a char into a number representing the value
        /// 1 = rock
        /// 2 = paper
        /// 3 = scissors
        /// </summary>
        /// <param name="c">The char to convert</param>
        /// <returns>the result. Will be NULL if char is unknown</returns>
        private static int? TranslateChar(char c)
        {
            return c switch
            {
                'A' or 'X' => 1,
                'B' or 'Y' => 2,
                'C' or 'Z' => 3,
                _ => null,
            };
        }

        private static bool DidIWin(int myChoice, int oppomentChoice) 
        {
            if (myChoice == 3 && oppomentChoice == 1) return false;
            if (myChoice == 1 && oppomentChoice == 3) return true;

            return myChoice > oppomentChoice;
        }

        private static int GetGameScore(int myChoice, int oppomentChoice)
        {
            return myChoice + (myChoice == oppomentChoice
                    ? 3
                    : DidIWin(myChoice, oppomentChoice)
                        ? 6
                        : 0);
        }

        private static int GetCorrectShape(int oppomentChoise, char assingment)
        {
            if (assingment == 'Y') return oppomentChoise;

            if (assingment == 'X')
            {
                if (oppomentChoise == 1) return 3;
                return oppomentChoise - 1;
            }

            if (assingment == 'Z')
            {
                if (oppomentChoise == 3) return 1;
                return oppomentChoise + 1;
            }

            throw new Exception("unknown char");
        }
    }
}
