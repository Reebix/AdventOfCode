using System;
using System.IO;
using AdventOfCode.days;

namespace AdventOfCode;

internal class Program
{
    public static void Main(string[] args)
    {
        //CreateDays();
        new Day1();
        Console.WriteLine();
    }

    public static void CreateDays()
    {
        var time = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
        for (var i = 25; i > 0; i--)
        {
            var path = $"../../days/Day{i}.txt";
            if (!File.Exists(path)) File.Create(path);
            if (!File.Exists($"../../days/Day{i}.cs"))
                File.WriteAllText($"../../days/Day{i}.cs",
                    File.ReadAllText("../../days/TemplateDay.cs")
                        .Replace("TemplateDay", $"Day{i}")
                        .Replace("base(1)", $"base({i})"));
        }
    }
}