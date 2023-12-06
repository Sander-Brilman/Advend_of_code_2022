using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Puzzels2023.Solutions;
public class Solution6(string path) : SolutionBase(path)
{
    private record Race(long Miliseconds, long RecordMilimeters);

    private static long CalcuateDistance(long maxTime, long timeSpendButtonPressing)
    {
        if (maxTime == timeSpendButtonPressing || timeSpendButtonPressing == 0) { return 0; }

        long milimeterPerMilisecond = timeSpendButtonPressing;
        long miliSecondsLeftRacing = maxTime - timeSpendButtonPressing;

        return milimeterPerMilisecond * miliSecondsLeftRacing;
    }

    private Race[] GetRaces()
    {
        long[] miliseconds = _lines[0]
            .Split(':')[1]
            .Trim()
            .Split(" ")
            .Where(x => x.Length > 0)
            .Select(long.Parse)
            .ToArray();

        long[] milimeters = _lines[1]
            .Split(':')[1]
            .Trim()
            .Split(" ")
            .Where(x => x.Length > 0)
            .Select(long.Parse)
            .ToArray();

        List<Race> races = new(miliseconds.Length);

        for (long i = 0; i < miliseconds.Length; i++)
        {
            races.Add(new Race(miliseconds[i], milimeters[i]));
        }

        return [.. races];
    }

    private Race GetSingleRace()
    {
        string miliSecondsString = _lines[0].Split(':')[1];
        long miliseconds = long.Parse(Regex.Replace(miliSecondsString, " ", ""));

        string milimetersString = _lines[1].Split(':')[1];
        long milimeters = long.Parse(Regex.Replace(milimetersString, " ", ""));

        return new Race(miliseconds, milimeters);
    }

    private static (long frist, long last) GetTotalWaysOfWinningForRace(Race race)
    {
        long? firstWayOfWinning = null;
        long lastWayOfWinning = 0;
        long totalWaysOfWinningForCurrentRace = 0;
        for (long timeSpendPressingButton = 0; timeSpendPressingButton < race.Miliseconds; timeSpendPressingButton++)
        {
            long distance = CalcuateDistance(race.Miliseconds, timeSpendPressingButton);

            if (distance > race.RecordMilimeters)
            {
                firstWayOfWinning ??= timeSpendPressingButton;

                totalWaysOfWinningForCurrentRace++;
                lastWayOfWinning = timeSpendPressingButton;
            }
        }

        return (firstWayOfWinning!.Value, lastWayOfWinning);
    }


    public override string GetFirstSolution()
    {
        var races = GetRaces();

        long marginOfError = 0;

        foreach (var race in races)
        {
            (long first, long last) = GetTotalWaysOfWinningForRace(race);

            long totalWaysOfWinningForCurrentRace = (last - first) + 1;

            if (marginOfError == 0) {
                marginOfError = totalWaysOfWinningForCurrentRace;
                continue;
            }

            marginOfError *= totalWaysOfWinningForCurrentRace;
        }

        return marginOfError.ToString();
    }

    public override string GetSecondSolution2()
    {
        var race = GetSingleRace();

        (long first, long last) = GetTotalWaysOfWinningForRace(race);

        return ((last - first) + 1).ToString();
    }
}
