using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.days;

public class Day10 : Day
{
    private char[][] grid;

    public Day10() : base(10)
    {
    }

    protected override void Run(bool isPart2 = false)
    {
        grid = new char[Input.Length][];

        for (var i = 0; i < Input.Length; i++) grid[i] = Input[i].ToCharArray();

        // find the location of the S
        var (x, y) = (0, 0);
        for (var i = 0; i < grid.Length; i++)
        for (var j = 0; j < grid[0].Length; j++)
            if (grid[i][j] == 'S')
            {
                (x, y) = (i, j);
                break;
            }


        List<(int x, int y, char option)> toVisit = new();
        foreach (var possibleChar in GetPossibleChars((x, y))) toVisit.Add((x, y, possibleChar));

        foreach (var original in toVisit)
        {
            var notVisited = new List<(int x, int y)>();
            var visited = new List<(int x, int y)>();

            var (oX, oY) = (original.x, original.y);
            notVisited.Add((oX, oY));
            int steps = 0;
            while (notVisited.Count > 0)
            {
                steps++;
                var (x1, y1) = notVisited[0];
                Console.WriteLine($"Checking {x1}, {y1} with option {original.option}");
                if (x1 < 0 || y1 < 0 || y1 >= grid.Length || x1 >= grid[0].Length) continue;
                if (grid[x1][y1] == 'S') Console.WriteLine($"Found S at {x1}, {y1} with option {original.option} in {steps} steps");
                notVisited.RemoveAt(0);
                var possibleChars = GetPossibleChars((x1, y1));
                foreach (var possibleChar in possibleChars)
                {
                    var (c1, c2) = CharToRelativeConnections(possibleChar);
                    var (x2, y2) = ((c1.x + x1, c1.y + y1), (c2.x + x1, c2.y + y1));
                    notVisited.Add((x2.Item1, x2.Item2));
                    notVisited.Add((y2.Item1, y2.Item2));
                }
                
                visited.Add((x1, y1));
            }
        }
        
    }

    private void PrintGrid()
    {
        foreach (var row in grid) Console.WriteLine(row);
    }

    private ((int x, int y) c1, (int x, int y)c2) CharToRelativeConnections(char character)
    {
        return character switch
        {
            '|' => ((0, 1), (0, -1)), // vertical pipe connecting north and south
            '-' => ((-1, 0), (1, 0)), // horizontal pipe connecting east and west
            'L' => ((0, 1), (1, 0)), // 90-degree bend connecting north and east
            'J' => ((0, 1), (-1, 0)), // 90-degree bend connecting north and west
            '7' => ((-1, 0), (0, -1)), // 90-degree bend connecting south and west
            'F' => ((1, 0), (0, -1)), // 90-degree bend connecting south and east
            'S' => ((0, 0), (0, 0)), // start
            _ => throw new Exception($"Invalid character: {character}")
        };
    }

    private List<char> GetPossibleChars((int x, int y) loc)
    {
        var adjacentChars = new List<(int, int)>();
        var height = grid.Length;
        var width = grid[0].Length;


        // check cardinal directions
        for (var x = -1; x < 2; x++)
        for (var y = -1; y < 2; y++)
        {
            var (x1, y1) = (loc.x + x, loc.y + y);
            if (x1 < 0 || y1 < 0 || y1 >= height || x1 >= width) continue;
            var charAt = grid[y1][x1];
            if (charAt == '.') continue;
            if (x == 0 && y == 0) continue;
            var (c1, c2) = CharToRelativeConnections(charAt);

            var (eC1, eC2) = ((c1.x + x1, c1.y + y1), (c2.x + x1, c2.y + y1));
            if (eC1 == loc || eC2 == loc) adjacentChars.Add((x1, y1));
        }

        var possibleChars = new List<char>();

        var allChars = new List<char> { '|', '-', 'L', 'J', '7', 'F' };
        allChars.ForEach(c =>
        {
            adjacentChars.ForEach(a =>
            {
                var (c1, c2) = CharToRelativeConnections(c);
                var (x1, y1) = ((c1.x + a.Item1, c1.y + a.Item2), (c2.x + a.Item1, c2.y + a.Item2));
                if (x1 == loc || y1 == loc) possibleChars.Add(c);
            });
        });

        // make unique
        possibleChars = possibleChars.Distinct().ToList();

        return possibleChars;
    }
}