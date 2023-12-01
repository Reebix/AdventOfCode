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
        Run();
        Run(true);
        Console.WriteLine();
        // ReSharper restore VirtualMemberCallInConstructor
    }

    protected abstract void Run(bool isPart2 = false);
}