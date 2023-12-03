using System;
using System.IO;

namespace AdventOfCode;

internal class Program
{
    public static void Main(string[] args)
    {
        var time = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
        if (time.Month != 12)
        {
            Console.WriteLine("It's not December yet!");
            return;
        }

        // TODO: remove when whole month is done
        var i = 1;
        if (args.Length == 0 && time is { Month: 12, Day: <= 25 }) i = time.Day;

        for (; i <= time.Day && i <= 25; i++)
        {
            var type = Type.GetType($"AdventOfCode.days.Day{i}");
            if (type == null)
            {
                Console.WriteLine($"Day {i} not found");
                continue;
            }

            Activator.CreateInstance(type);
        }
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