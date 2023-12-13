using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Puzzels2023.Solutions;
internal class Solution12(string path) : SolutionBase(path)
{
    private record Line(string Raw, List<int> GroupsOfSprings)
    {
        public static Line FromRawString(string input)
        {
            var groupsOfSprings = input
                .Split(' ')[1]
                .Split(',')
                .Select(int.Parse)
                .ToList();

            return new Line(input.Split(' ')[0], groupsOfSprings);
        }
    }

    private Line[] GetLines()
    {
        return _lines
            .Select(Line.FromRawString)
            .ToArray();
    }

    private class Field()
    {
        public int StartIndex { get; set; }

        public int Length { get; set; }

        public int EndIndex => StartIndex + Length - 1;
    }

    private Field[] GetFieldsForLine(ReadOnlySpan<char> input)
    {
        List<Field> fields = [];

        bool isTrackingField = false;
        int lineLength = input.Length;
        for (int i = 0; i < lineLength; i++)
        {
            char c = input[i];

            if (c == '.')
            {
                isTrackingField = false;
                continue;
            }

            if (c == '#')
            {
                if (isTrackingField)
                {
                    Field lastField = fields.Last();
                    lastField.Length -= 1;

                    if (lastField.Length == 0)
                    {
                        fields.Remove(lastField);
                    }
                }

                isTrackingField = false;
                continue;
            }

            if (isTrackingField)
            {
                fields.Last().Length += 1;
                continue;
            }


            if (c == '?')
            {
                if (i - 1 >= 0 && input[i - 1] == '#')
                {
                    continue;
                }

                Field newField = new()
                {
                    Length = 1,
                    StartIndex = i,
                };

                fields.Add(newField);

                isTrackingField = true;

                continue;
            }
        }

        return [.. fields];
    }


    private class GroupPointer
    {
        public int StartIndex { get; set; }

        public int Length { get; set; }

        public int EndIndex => StartIndex + Length - 1;
    }

    private static bool IsInValidLocation(GroupPointer pointer, ReadOnlySpan<char> line)
    {
        if (pointer.StartIndex - 1 >= 0 && line[pointer.StartIndex - 1] == '#')
        {
            return false;
        }

        if (pointer.EndIndex + 1 < line.Length && line[pointer.EndIndex + 1] == '#')
        {
            return false;
        }

        for (int i = pointer.StartIndex; i <= pointer.EndIndex; i++)
        {
            if (line[i] == '.')
            {
                return false;
            }
        }

        return true;
    }

    private GroupPointer[] GetDefaultGroupPositionsForLine(Line line)
    {
        ReadOnlySpan<char> rawLine = line.Raw;

        List<GroupPointer> groupPointers = new(line.GroupsOfSprings.Count);

        int currentGroupIndex = 0;
        GroupPointer currentPointer = new()
        {
            StartIndex = 0,
            Length = line.GroupsOfSprings.First()
        };

        for (int i = 0; i < rawLine.Length; i++)
        {
            currentPointer.StartIndex = i;

            if (IsInValidLocation(currentPointer, rawLine))
            {
                groupPointers.Add(currentPointer);
                currentGroupIndex++;

                if (currentGroupIndex == line.GroupsOfSprings.Count)
                {
                    break;
                }

                i += currentPointer.Length;

                currentPointer = new()
                {
                    StartIndex = i,
                    Length = line.GroupsOfSprings[currentGroupIndex]
                };
            }
        }

        return [.. groupPointers];
    }

    private int GetArrangementsForLine(Line line)
    {
        int possibleCombinations = 0;

        Field[] fieldsForLine = GetFieldsForLine(line.Raw);
        GroupPointer[] groupPointers = GetDefaultGroupPositionsForLine(line);

        foreach (var field in fieldsForLine)
        {
            GroupPointer[] groupPointersInAField = groupPointers
                .Where(g => g.StartIndex >= field.StartIndex && g.EndIndex <= field.EndIndex)
                .ToArray();

            int amountOfGroupsInField = groupPointersInAField.Length;

            if (amountOfGroupsInField == 0) { continue; }

            int leftOverColumnsInField = field.Length - groupPointersInAField.Select(g => g.Length).Sum();

            possibleCombinations += (amountOfGroupsInField + (amountOfGroupsInField - 1)) * (leftOverColumnsInField - (amountOfGroupsInField - 1)) + 1;
        }

        if (possibleCombinations == 0)
        {
            return 1;
        }

        return possibleCombinations;
    }

    public override string GetFirstSolution()
    {
        var lines = GetLines(); 

        return lines
            .Select(GetArrangementsForLine)
            .Sum()
            .ToString();
    }

    public override string GetSecondSolution2()
    {
        throw new NotImplementedException();
    }
}
