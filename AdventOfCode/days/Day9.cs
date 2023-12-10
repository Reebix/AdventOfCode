using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.days;

public class Day9 : Day
{
    public Day9() : base(9)
    {
    }


    protected override void Run(bool isPart2 = false)
    {
        List<List<long>> numbers = new();

        foreach (var line in Input)
        {
            var list = new List<long>();
            foreach (var number in line.Split(' ')) list.Add(long.Parse(number));
            numbers.Add(list);
        }


        long total = 0;

        foreach (var num in numbers)
        {
            var sequenceList = CreateSequenceList(num);
            sequenceList.Reverse();
            sequenceList[0].Add(0);
            for (var i = 0; i < sequenceList.Count - 1; i++)
            {
                var sequence = sequenceList[i];
                var nextSequence = sequenceList[i + 1];

                if (!isPart2)
                {
                    nextSequence.Add(sequence.Last() + nextSequence.Last());
                    continue;
                }

                nextSequence.Insert(0, nextSequence.First() - sequence.First());
            }


            // Get Last number in sequenceList
            if (!isPart2)
            {
                total += sequenceList.Last().Last();
                continue;
            }
            total += sequenceList.Last().First();
            
        }

        Console.WriteLine(total);
    }

    private List<List<long>> CreateSequenceList(List<long> numbers)
    {
        var sequenceList = new List<List<long>>();
        sequenceList.Add(numbers);
        while (!IsWholeZero(numbers))
        {
            numbers = ToSequence(numbers);
            sequenceList.Add(numbers);
        }

        return sequenceList;
    }

    private List<long> ToSequence(List<long> numbers)
    {
        var sequence = new List<long>();
        for (var i = 0; i < numbers.Count - 1; i++) sequence.Add(numbers[i + 1] - numbers[i]);
        return sequence;
    }

    private bool IsWholeZero(List<long> numbers)
    {
        foreach (var number in numbers)
            if (number != 0)
                return false;
        return true;
    }
}