using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advend_of_code_1._1.Puzzles
{
    internal class PuzzleSolution
    {
        protected readonly StreamReader InputFile;

        protected PuzzleSolution(StreamReader inputFile) 
        {
            InputFile = inputFile;
        }

        public string ExecutePuzzle(int puzzle)
        {
            return puzzle == 1 
                ? Puzzle1()
                : Puzzle2();
        }

        public virtual string Puzzle1()
        {
            throw new NotImplementedException();
        }

        public virtual string Puzzle2()
        {
            throw new NotImplementedException();
        }
    }
}
