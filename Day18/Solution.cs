using System.Collections;
using System.Text.RegularExpressions;
using Shared;
using static Shared.Direction;

namespace Day18;

public class Solution
{
    private readonly IEnumerable<(Direction Direction, int Steps)> commands;
    private HashSet<Coordinate2D> boundaryCoordinates;
    private readonly List<Coordinate2D> interiorCoordinates = [];


    public Solution(List<string> input)
    {
        commands = input
            .Select(line => Regex.Match(line, @"(?<Direction>R|L|U|D) (?<Steps>\d+)"))
            .Select(m => (Direction: GetDirection(m.Groups["Direction"].Value),
                Steps: int.Parse(m.Groups["Steps"].Value)));
    }

    private static Direction GetDirection(string d)
    {
        return d switch
        {
            "R" => East,
            "L" => West,
            "U" => North,
            "D" => South,
            _ => throw new ProgrammerMistake()
        };
    }

    public object PartOne()
    {
        var start = Coordinate2D.Origin;
        boundaryCoordinates = commands.Aggregate(new HashSet<Coordinate2D> { start },
            (acc, c) => acc.Concat(ProcessCommand(acc.Last(), c.Direction, c.Steps)).ToHashSet());

        var yMax = boundaryCoordinates.Max(it => it.Y);
        var topLeftInsideBorder = boundaryCoordinates
            .Where(it => it.Y == yMax - 1)
            .MinBy(it => it.X)?
            .Move(East) ?? throw new ProgrammerMistake();
        
        Fill(topLeftInsideBorder);

        PrintMap();

        return boundaryCoordinates.Count + interiorCoordinates.Count;
    }

    private void Fill(Coordinate2D start)
    {
        var s = new Queue<(int, int, int, int)>();
        s.Enqueue((start.X, start.X, start.Y, 1));
        s.Enqueue((start.X, start.X, start.Y - 1, -1));
        while (s.Count > 0)
        {
            var (x1, x2, y, dy) = s.Dequeue();
            var x = x1;
            if (Inside(x, y))
            {
                while (Inside(x - 1, y))
                {
                    interiorCoordinates.Add(new(x - 1, y));
                    x = x - 1;
                }

                if (x < x1)
                {
                    s.Enqueue((x, x1 - 1, y - dy, -dy));
                }
            }

            while (x1 <= x2)
            {
                while (Inside(x1, y))
                {
                    interiorCoordinates.Add(new(x1, y));
                    x1++;
                }

                if (x1 > x)
                {
                    s.Enqueue((x, x1 - 1, y + dy, dy));
                }

                if (x1 - 1 > x2)
                {
                    s.Enqueue((x2 + 1, x1 - 1, y - dy, -dy));
                }

                x1++;
                while (x1 < x2 && !Inside(x1, y))
                {
                    x1++;
                }

                x = x1;
            }
        }
    }

    private bool Inside(int x, int y)
    {
        return !boundaryCoordinates.Contains(new Coordinate2D(x, y));
    }

    private void PrintMap()
    {
        var xMax = boundaryCoordinates.Max(it => it.X);
        var xMin = boundaryCoordinates.Min(it => it.X);
        var yMax = boundaryCoordinates.Max(it => it.Y);
        var yMin = boundaryCoordinates.Min(it => it.Y);

        for (int y = yMax; y >= yMin; y--)
        {
            for (int x = xMin; x <= xMax; x++)
            {
                var c = new Coordinate2D(x, y);
                if (boundaryCoordinates.Contains(c))
                {
                    Console.Write('#');
                    continue;
                }
                if (interiorCoordinates.Contains(c))
                {
                    Console.Write('X');
                    continue;
                }

                Console.Write('.');
            }

            Console.WriteLine();
        }
    }

    private static List<Coordinate2D> ProcessCommand(Coordinate2D start, Direction direction, int steps)
    {
        var cs = new List<Coordinate2D>();
        for (int i = 1; i <= steps; i++)
        {
            cs.Add(start.Add(Coordinate2D.FromDirection(direction) * i));
        }

        return cs;
    }

    public object PartTwo()
    {
        return "Not implemented yet";
    }
}