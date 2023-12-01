using System;
using System.Collections.Generic;

namespace AdventOfCode.days;

public class Day1 : Day
{
    private readonly Dictionary<string, int> _dictionary = new()
    {
        { "one", 1 },
        { "two", 2 },
        { "three", 3 },
        { "four", 4 },
        { "five", 5 },
        { "six", 6 },
        { "seven", 7 },
        { "eight", 8 },
        { "nine", 9 }
    };

    public Day1() : base(1)
    {
    }


    protected override void Run(bool isPart2 = false)
    {
        var sum = 0;
        var result = new string[Input.Length];
        for (var i = 0; i < Input.Length; i++)
        {
            var s = Input[i];

            var charArray = s.ToCharArray();
            var array = new List<char>();
            for (var i1 = 0; i1 < charArray.Length; i1++)
            {
                var c = charArray[i1];
                if (char.IsDigit(c))
                    array.Add(c);

                if (isPart2)
                {
                    var numString = "";
                    var numInt = 0;
                    var currentChar = charArray[i1 + numInt];
                    while (char.IsLetter(currentChar) && i1 + numInt < charArray.Length && numInt < 5)
                    {
                        numString += currentChar;
                        numInt++;
                        if (i1 + numInt < charArray.Length)
                            currentChar = charArray[i1 + numInt];

                        if (_dictionary.TryGetValue(numString, out var value))
                            array.Add(value.ToString()[0]);
                    }
                }
            }

            var first = array.Count > 0 ? array[0] : '0';
            var last = array.Count > 0 ? array[array.Count - 1] : '0';
            var num = first + "" + last;
            result[i] = num + " " + s;
            sum += int.Parse(num);
        }

        Console.WriteLine(sum);
        // Console.WriteLine(string.Join(", ", result));
    }
}