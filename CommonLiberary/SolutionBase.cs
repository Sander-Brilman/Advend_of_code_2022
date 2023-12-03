using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLiberary;
public abstract class SolutionBase(string puzzleInputPath)
{
    protected readonly string[] _lines = File.ReadAllLines(puzzleInputPath);

    public string GetSectionSolution(int section) => section == 1 ? GetFirstSolution() : GetSecondSolution2();

    abstract public string GetFirstSolution();

    abstract public string GetSecondSolution2();
}
