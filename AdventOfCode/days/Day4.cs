using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.days;

public class Day4 : Day
{
    public Day4() : base(4)
    {
    }


/*
Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53
Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19
Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1
Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83
Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36
Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11
*/

    protected override void Run(bool isPart2 = false)
    {
        var total = 0;

        var wonAmountList = new List<int>();

        // parse cards
        foreach (var line in Input)
        {
            var wonAmount = 0;

            var values = line.Split(':')[1].Replace("  ", " ").Split('|');
            var winning = values[0].Split(' ').ToList();
            var have = values[1].Split(' ').ToList();
            winning.RemoveAt(0);


            // check if we have the winning numbers
            foreach (var num in have)
            {
                if (!winning.Contains(num))
                    continue;

                wonAmount++;
            }

            wonAmount--;

            if (isPart2)
            {
                wonAmountList.Add(wonAmount);
                continue;
            }

            // calculate score
            var score = 0;
            for (var i = 0; i < wonAmount; i++)
            {
                if (score == 0)
                {
                    score = 1;
                    continue;
                }

                score *= 2;
            }

            // add score to total
            total += score;
        }

        if (!isPart2) Console.WriteLine(total);

        var listWithAmounts = new List<(int, int)>();

        wonAmountList.ForEach(x => listWithAmounts.Add((x, 1)));

        // go through list
        for (var i = 0; i < listWithAmounts.Count; i++)
        {
            var (nextAmount, amount) = listWithAmounts[i];

            // add amount to next
            for (var j = 0; j < nextAmount; j++)
            {
                var nextIndex = i + j + 1;

                if (nextIndex >= listWithAmounts.Count)
                    continue;

                var (nextNextAmount, nextAmountAmount) = listWithAmounts[nextIndex];
                listWithAmounts[nextIndex] = (nextNextAmount, nextAmountAmount + amount);
            }
        }

        // Console.WriteLine(string.Join(", ", listWithAmounts));
        Console.WriteLine(listWithAmounts.Sum(x => x.Item2));
    }
}