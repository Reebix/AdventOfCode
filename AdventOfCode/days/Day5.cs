using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AdventOfCode.days;

public class Day5 : Day
{
    public Day5() : base(5)
    {
    }


    protected override void Run(bool isPart2 = false)
    {
        // init maps
        var seedToSoilMap = new Map();
        var soilToFertilizerMap = new Map();
        var fertilizerToWaterMap = new Map();
        var waterToLightMap = new Map();
        var lightToTemperatureMap = new Map();
        var temperatureToHumidityMap = new Map();
        var humidityToLocationMap = new Map();

        // parse input
        List<long> seeds = new();
        List<(long, long)> seedRanges = new();
        var seedLine = Input[0].Split(':')[1].Split(' ').ToList();
        seedLine.RemoveAt(0);
        seedLine.ForEach(s => seeds.Add(long.Parse(s)));

        // parse seed ranges for part 2
        for (var i = 0; i < seedLine.Count; i += 2)
        {
            var rangeStart = long.Parse(seedLine[i]);
            var rangeLength = long.Parse(seedLine[i + 1]);

            seedRanges.Add((rangeStart, rangeLength));
        }

        var currentMap = seedToSoilMap;
        for (var index = 2; index < Input.Length; index++)
        {
            var line = Input[index];

            switch (line)
            {
                case "soil-to-fertilizer map:":
                    currentMap = soilToFertilizerMap;
                    break;
                case "fertilizer-to-water map:":
                    currentMap = fertilizerToWaterMap;
                    break;
                case "water-to-light map:":
                    currentMap = waterToLightMap;
                    break;
                case "light-to-temperature map:":
                    currentMap = lightToTemperatureMap;
                    break;
                case "temperature-to-humidity map:":
                    currentMap = temperatureToHumidityMap;
                    break;
                case "humidity-to-location map:":
                    currentMap = humidityToLocationMap;
                    break;
            }

            // if the line starts with a number, it's a mapping
            if (line.Length != 0 && char.IsDigit(line.ToCharArray()[0]))
            {
                var split = line.Split(' ');
                var destinationRangeStart = long.Parse(split[0]);
                var sourceRangeStart = long.Parse(split[1]);
                var destinationRangeEnd = long.Parse(split[2]);

                currentMap.AddMapping(destinationRangeStart, sourceRangeStart, destinationRangeEnd);
            }
        }


        var lowestLocation = long.MaxValue;

        // part 2
        if (isPart2) goto Part2;

        // convert all seeds down to location
        foreach (var seed in seeds)
        {
            var soil = seedToSoilMap.MapSource(seed);
            var fertilizer = soilToFertilizerMap.MapSource(soil);
            var water = fertilizerToWaterMap.MapSource(fertilizer);
            var light = waterToLightMap.MapSource(water);
            var temperature = lightToTemperatureMap.MapSource(light);
            var humidity = temperatureToHumidityMap.MapSource(temperature);
            var location = humidityToLocationMap.MapSource(humidity);

            if (lowestLocation > location) lowestLocation = location;
        }

        Console.WriteLine(lowestLocation);

        return;

        // part 2
        Part2:
        lowestLocation = long.MaxValue;

        List<(long, long)> dividedSeedRanges = new();

        
        // divide seed ranges into smaller ranges
         var divisionCount = 1024;
        
        foreach (var (rangeStart, rangeLength) in seedRanges)
        {
            var rangeEnd = rangeStart + rangeLength;
            var rangeSize = rangeLength / divisionCount;

            for (var i = 0; i < divisionCount; i++)
            {
                var start = rangeStart + i * rangeSize;
                var length = i == divisionCount - 1 ? rangeEnd - start : rangeSize;

                dividedSeedRanges.Add((start, length));
            }
        }
        
        Console.WriteLine(dividedSeedRanges.Count);
        Console.WriteLine(dividedSeedRanges.Sum(r => r.Item2));
        
        Console.WriteLine(seedRanges.Count);
        Console.WriteLine(seedRanges.Sum(r => r.Item2));
       

        var finishedThreads = 0;
        /*
       foreach (var (start, length) in dividedSeedRanges)
       {
           var thread = new Thread(o =>
           {
               for (var i = 0; i < length; i++)
               {
                   var seed = start + i;
                   var soil = seedToSoilMap.MapSource(seed);
                   var fertilizer = soilToFertilizerMap.MapSource(soil);
                   var water = fertilizerToWaterMap.MapSource(fertilizer);
                   var light = waterToLightMap.MapSource(water);
                   var temperature = lightToTemperatureMap.MapSource(light);
                   var humidity = temperatureToHumidityMap.MapSource(temperature);
                   var location = humidityToLocationMap.MapSource(humidity);

                   if (lowestLocation > location) lowestLocation = location;
               }

               Console.WriteLine($"Thread finished: {finishedThreads++} / {dividedSeedRanges.Count}");
           });

           thread.Start();
           thread.Join();
       }
       */

        Parallel.For(0, seedRanges.Count, i =>
        {
            var (start, length) = seedRanges[i];
            for (var j = 0; j < length; j++)
            {
                var seed = start + j;
                var soil = seedToSoilMap.MapSource(seed);
                var fertilizer = soilToFertilizerMap.MapSource(soil);
                var water = fertilizerToWaterMap.MapSource(fertilizer);
                var light = waterToLightMap.MapSource(water);
                var temperature = lightToTemperatureMap.MapSource(light);
                var humidity = temperatureToHumidityMap.MapSource(temperature);
                var location = humidityToLocationMap.MapSource(humidity);

                if (lowestLocation > location) lowestLocation = location;
            }

            Console.WriteLine($"Thread finished: {finishedThreads++} / {dividedSeedRanges.Count}");
        });
        
        Console.WriteLine(lowestLocation);
    }

    private class Map
    {
        // destinationRangeStart, sourceRangeStart, destinationRangeEnd
        private readonly List<(long, long, long)> _mappings = new();

        public void AddMapping(long destinationRangeStart, long sourceRangeStart, long destinationRangeEnd)
        {
            _mappings.Add((destinationRangeStart, sourceRangeStart, destinationRangeEnd));
        }

        public long MapSource(long source)
        {
            foreach (var (start, sourceStart, length) in _mappings)
                if (source >= sourceStart && source <= sourceStart + length)
                    return start - sourceStart + source;

            return source;
        }
    }
}