using System;
using System.Collections.Generic;

namespace AdventOfCode.days;

public class Day6 : Day
{
    public Day6() : base(6)
    {
    }


    protected override void Run(bool isPart2 = false)
    {
        List<(long, long)> raceList = new();
        var timeIn = ParseStringToIntList(Input[0], isPart2);
        var distanceIn = ParseStringToIntList(Input[1], isPart2);

        if (isPart2)
            goto Part2;
        for (var i = 0; i < timeIn.Count; i++) raceList.Add((timeIn[i], distanceIn[i]));

        var total = 1;
        foreach (var (time, distance) in raceList)
        {
            var waysToWin = 0;
            for (var i = 0; i < time; i++)
            {
                var remaining = time - i;
                var distanceTraveled = i * remaining;
                if (distanceTraveled > distance) waysToWin++;
            }

            total *= waysToWin;
        }

        Console.WriteLine(total);
        return;

        Part2:
        var waysToWinPt2 = 0;
        for (var i = 0; i < timeIn[0]; i++)
        {
            var remaining = timeIn[0] - i;
            var distanceTraveled = i * remaining;
            if (distanceTraveled > distanceIn[0]) waysToWinPt2++;
        }

        Console.WriteLine(waysToWinPt2);
    }

    protected virtual List<long> ParseStringToIntList(string input, bool isPart2 = false)
    {
        if (input == null) throw new ArgumentNullException(nameof(input));
        var split = input.ToCharArray();
        var result = new List<long>();

        var current = "";
        foreach (var c in split)
            if (!char.IsDigit(c))
            {
                if (current.Length > 0)
                    if (!isPart2)
                    {
                        result.Add(long.Parse(current));
                        current = "";
                    }
            }
            else
            {
                current += c;
            }

        if (current.Length > 0) result.Add(long.Parse(current));

        return result;
    }
}