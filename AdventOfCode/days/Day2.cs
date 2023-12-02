using System;

namespace AdventOfCode.days;

public class Day2 : Day
{
    public Day2() : base(2)
    {
    }

//Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
    protected override void Run(bool isPart2 = false)
    {
        var validGames = 0;
        var sumOfPowers = 0;

        const int maxRed = 12;
        const int maxGreen = 13;
        const int maxBlue = 14;

        foreach (var line in Input)
        {
            // Init Cubes
            Cube red = new("red");
            Cube green = new("green");
            Cube blue = new("blue");

            // Get Game
            var game = line.Split(':');
            var gameNumber = int.Parse(game[0].Split(' ')[1]);

            foreach (var round in game[1].Split(';'))
            {
                var cubes = round.Split(',');
                foreach (var cube in cubes)
                {
                    var color = cube.Split(' ')[2];
                    var amount = int.Parse(cube.Split(' ')[1]);
                    switch (color)
                    {
                        case "red":
                            red.Amount = isPart2 ? red.Amount < amount ? amount : red.Amount : amount;
                            break;
                        case "blue":
                            blue.Amount = isPart2 ? blue.Amount < amount ? amount : blue.Amount : amount;
                            break;
                        case "green":
                            green.Amount = isPart2 ? green.Amount < amount ? amount : green.Amount : amount;
                            break;
                    }

                    // Check if game is valid
                    if ((red.Amount > maxRed || blue.Amount > maxBlue || green.Amount > maxGreen) && !isPart2)
                        goto Invalid;
                }
            }

            // Get Power
            sumOfPowers += red.Amount * blue.Amount * green.Amount;


            validGames += gameNumber;
            // Console.WriteLine($"Game {gameNumber} is valid");

            Invalid: ;
            // Console.WriteLine($"Game {gameNumber} is invalid");
        }

        if (!isPart2)
            Console.WriteLine($"Valid: {validGames}");
        else
            Console.WriteLine($"Power: {sumOfPowers}");
    }


    private struct Cube
    {
        public Cube(string color, int amount = 0)
        {
            Amount = amount;
            Color = color;
        }

        private string Color { get; }
        public int Amount { get; set; }


        public override string ToString()
        {
            return $"{Amount} {Color}";
        }
    }
}