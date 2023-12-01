using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLiberary;
public abstract class SolutionBase(int puzzle, bool loadExampleFile = false)
{
    protected readonly string[] _lines = File.ReadAllLines(
        Path.Combine(
            _textFilesFolder, 
            $"Puzzle{puzzle}{(loadExampleFile ? ".example" : "")}.txt"
            )
        );

    private static readonly string _textFilesFolder = Path.Combine(
        Directory.GetCurrentDirectory(),
        "TextFiles"
    );

    public string GetSectionSolution(int section) => section == 1 ? GetFirstSolution() : GetSecondSolution2();

    abstract public string GetFirstSolution();

    abstract public string GetSecondSolution2();
}
