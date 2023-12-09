using System.Xml.Serialization;

namespace Puzzels2023.Solutions;

internal class Solution9(string path) : SolutionBase(path)
{
    private int[][] GetHistoryRows()
    {
        return _lines
            .Select(line => line
                .Split(" ")
                .Select(int.Parse)
                .ToArray())
            .ToArray();
    }

    private EnviormentData GetEnviormentDataForHistory(int[] history)
    {
        List<int[]> sequences = [history];

        for (int i = 0; i < sequences.Count; i++)
        {
            int[] sequence = sequences[i];
            
            var distinctValues = sequence
                .Distinct()
                .ToArray();

            if (distinctValues.Length == 1 && distinctValues[0] == 0) { break; }
            
            
            List<int> sepsValues = new(sequence.Length - 1);

            for (int sequenceIndex = 0; sequenceIndex < (sequence.Length - 1); sequenceIndex++)
            {
                int value1 = sequence[sequenceIndex];
                int value2 = sequence[sequenceIndex + 1];

                sepsValues.Add(value2 - value1);
            }

            sequences.Add([..sepsValues]);
        }

        return new EnviormentData(history, [..sequences]);
    }

    private record EnviormentData(int[] History, int[][] Sequences);

    public override string GetFirstSolution()
    {
        int[][] initalRows = GetHistoryRows();

        EnviormentData[] enviormentDataRows = initalRows
            .Select(GetEnviormentDataForHistory)
            .ToArray();


        int total = 0;
        foreach (var data in enviormentDataRows)
        {
            int newSequenceValue = 0;

            int[] lastValues = data.Sequences
                .Select(s => s.Last())
                .Reverse()
                .ToArray();

            foreach (var lastValue in lastValues)
            {
                newSequenceValue += lastValue;
            }

            total += newSequenceValue;
        }

        return total.ToString();
    }

    public override string GetSecondSolution2()
    {
        int[][] initalRows = GetHistoryRows();

        EnviormentData[] enviormentDataRows = initalRows
            .Select(GetEnviormentDataForHistory)
            .ToArray();


        int total = 0;
        foreach (var data in enviormentDataRows)
        {
            int newSequenceValue = 0;

            int[] firstValues = data.Sequences
                .Select(s => s.First())
                .Reverse()
                .ToArray();

            foreach (var firstValue in firstValues)
            {
                newSequenceValue = firstValue - newSequenceValue;
            }

            total += newSequenceValue;
        }

        return total.ToString();
    }
}
