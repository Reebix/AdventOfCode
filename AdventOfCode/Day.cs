using System;
using System.IO;

namespace AdventOfCode;

public abstract class Day
{
    protected readonly string[] Input;

    protected Day(int day)
    {
        Input = File.ReadAllLines($"../../days/Day{day}.txt");
        // ReSharper disable VirtualMemberCallInConstructor
        var start = DateTime.Now;
        Console.WriteLine($"-------------------- Day {day}: --------------------");
        Run();
        Console.WriteLine();
        Run(true);
        Console.WriteLine($"this day took {(DateTime.Now - start).TotalMilliseconds}ms");

        // ReSharper restore VirtualMemberCallInConstructor
    }

    protected abstract void Run(bool isPart2 = false);
}