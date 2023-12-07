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

        // split ranges into chunks of 1000000
        var chunks = new List<(long, long)>();
        foreach (var (rangeStart, rangeLength) in seedRanges)
        {
            for (var i = rangeStart; i < rangeStart + rangeLength + 1000000; i += 1000000)
            {
                var end = i + 100000;
                if (end > rangeStart + rangeLength)
                {
                    end = rangeStart + rangeLength;
                }

                chunks.Add((i, end));
            }
        }
        
        var chunkCount = chunks.Count;
        var chunkIndex = 0;
        
        
        List<long> chunkLowestLocations = new();
        // convert all seeds down to location
        Parallel.ForEach(chunks, chunk =>
        {
            var lowLoc = long.MaxValue;
            for (var i = 0; i < chunk.Item2; i++)
            {
                
                var soil = seedToSoilMap.MapSource(chunk.Item1 + i);
                var fertilizer = soilToFertilizerMap.MapSource(soil);
                var water = fertilizerToWaterMap.MapSource(fertilizer);
                var light = waterToLightMap.MapSource(water);
                var temperature = lightToTemperatureMap.MapSource(light);
                var humidity = temperatureToHumidityMap.MapSource(temperature);
                var location = humidityToLocationMap.MapSource(humidity);

                if (lowLoc > location) lowLoc = location;
            }
            
            Interlocked.Increment(ref chunkIndex);
            Console.WriteLine($"chunk {chunkIndex}/{chunkCount} done");
            
            chunkLowestLocations.Add(lowLoc);
        });
        
        // not 3128308
        Console.WriteLine(chunkLowestLocations.Min());
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