using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.days;

public class Day3 : Day
{
    public Day3() : base(3)
    {
    }


    protected override void Run(bool isPart2 = false)
    {
        Dictionary<(int, int), (char, int)> chars = new();

        List<((int, int), int)> numList = new();
        var currentCoords = (X: -1, Y: -1);
        var currentNumber = 0;

        for (var i = 0; i < Input.Length; i++)
        {
            var line = Input[i];
            for (var j = 0; j < line.Length; j++)
            {
                var c = line[j];

                // convert the char to a number
                if (char.IsNumber(c))
                {
                    currentNumber = currentNumber * 10 + (int)char.GetNumericValue(c);
                    currentCoords = (j, i);
                    continue;
                }

                if (c != '.' && (!isPart2 || c == '*'))
                    chars[(j, i)] = (c, 0);

                // if the current number is not 0, add it to the list
                if (currentNumber != 0)
                {
                    currentCoords.X -= currentNumber.ToString().Length - 1;
                    numList.Add((currentCoords, currentNumber));
                    // Console.WriteLine(currentNumber + " at " + currentCoords);
                    currentCoords = (-1, -1);
                    currentNumber = 0;
                }
            }
        }


        var maxLen = Input[0].Length;
        var maxHei = Input.Length;

        var total = 0;
        var gears = false;

        Dictionary<(int, int), int[]> overlaps = new();

        CheckList:

        foreach (var num in numList)
        {
            var coords = num.Item1;
            var number = num.Item2;

            // check the coords around the number
            for (var i = -1; i < number.ToString().Length + 1; i++)
            for (var j = -1; j < 2; j++)
            {
                var x = coords.Item1 + i;
                var y = coords.Item2 + j;

                // check if the coords are out of bounds
                if (x < 0 || y < 0 || x > maxLen || y > maxHei) continue;

                // use the dictionary to check if the coords are in the dictionary
                if (chars.ContainsKey((x, y)))
                {
                    // calculate the total
                    total += number;

                    if (!gears)
                        chars[(x, y)] = (chars[(x, y)].Item1, chars[(x, y)].Item2 + 1);

                    if (chars[(x, y)].Item2 == 2 && gears)
                    {
                        if (overlaps.ContainsKey((x, y)))
                            overlaps[(x, y)] = overlaps[(x, y)].Append(number).ToArray();
                        else
                            overlaps[(x, y)] = new[] { number };
                    }

                    // Console.WriteLine(number + " at " + coords + " overlaps with " + chars[(x, y)] + " at " + (x, y));

                    // exit out of both loops
                    i = number.ToString().Length + 1;
                    j = 3;
                }
            }
        }

        // return for part two to check the list again
        if (!gears && isPart2)
        {
            gears = true;
            goto CheckList;
        }

        // print the total
        if (!isPart2)
        {
            Console.WriteLine(total);
            return;
        }

        // part two
        var finalValue = 0;
        foreach (var overlap in overlaps)
        {
            finalValue += overlap.Value[0] * overlap.Value[1];
        }

        Console.WriteLine(finalValue);
    }
}