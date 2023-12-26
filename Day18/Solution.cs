using System.Text.RegularExpressions;
using Shared;
using static Shared.Direction;

namespace Day18;

public class Solution
{
    private readonly IEnumerable<(Direction Direction, int Steps)> commands;
    private HashSet<Coordinate2D> boundaryCoordinates;
    private HashSet<Coordinate2D> corners = new();
    private Dictionary<int, HashSet<int>> xs;
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
            (acc, c) =>
            {
                corners.Add(acc.Last());
                return acc.Concat(ProcessCommand(acc.Last(), c.Direction, c.Steps)).ToHashSet();
            });

        var yMax = boundaryCoordinates.Max(it => it.Y);
        var topLeftInsideBorder = boundaryCoordinates
            .Where(it => it.Y == yMax - 1)
            .MinBy(it => it.X)?
            .Move(East) ?? throw new ProgrammerMistake();

        Fill2(topLeftInsideBorder);

        PrintMap();

        return boundaryCoordinates.Count;
    }

    private int GetAreaByShoelaceFormula()
    {
        var cs = new HashSet<Coordinate2D>
        {
            new(1, 6),
            new(3, 1),
            new(7, 2),
            new(4, 4),
            new(8, 5),
        };
        var windows = corners.ToList()
            .Append(corners.First())
            .Windowed(2)
            .Select(it => (it.First().X * it.Last().Y) - (it.First().Y * it.Last().X));
        return Math.Abs(windows.Sum() / 2);
    }

    private void Fill2(Coordinate2D start)
    {
        var yMin = boundaryCoordinates.Min(it => it.Y);
        var yMax = boundaryCoordinates.Max(it => it.Y);
        var xsByY = boundaryCoordinates
            .GroupBy(it => it.Y)
            .ToDictionary(c => c.Key, c => c.Select(it => it.X).Order());
        
        var s = new Queue<(int LX, int RX, int Y, int dY)>();
        s.Enqueue((start.X, start.X, start.Y, -1));
        while (s.Count > 0)
        {
            var (lx, rx, y, dy) = s.Dequeue();
            if (y == yMin) continue;
            if (y == yMax) continue;
            if (Inside(lx, y))
            {
                var newLx = xsByY[y].LastOrDefault(it => it < lx, lx)+1;
                var newRx = xsByY[y].FirstOrDefault(it => it > rx, rx);
                if(newLx < lx) s.Enqueue((newLx, lx, y-dy, -dy));
                if(newRx >= rx) s.Enqueue((rx, newRx, y-dy, -dy));
                lx = newLx;
                rx = newRx;
                
                for (int i = lx; i < rx; i++)
                {
                    interiorCoordinates.Add(new (i, y));
                }
                
            }


            for(int i = lx; i < rx-1; i++)
            {
                if (xsByY[y+dy].Contains(i))
                {
                    continue;
                }

                var rangeStart = i;
                var rangeEnd = xsByY[y+dy].FirstOrDefault(it => it > i, rx+1)-1;
                s.Enqueue((rangeStart, rangeEnd, y+dy, dy));
                i = rangeEnd;
            }
        }
    }
    private void Fill(Coordinate2D start)
    {
        var s = new Queue<(int, int, int, int)>();
        s.Enqueue((start.X, start.X, start.Y, 1));
        s.Enqueue((start.X, start.X, start.Y - 1, -1));
        var xsByY = boundaryCoordinates
            .GroupBy(it => it.Y)
            .ToDictionary(c => c.Key, c => c.Select(it => it.X).Order());
        while (s.Count > 0)
        {
            var (x1, x2, y, dy) = s.Dequeue();
            var x = x1;
            if (Inside(x, y))
            {
                x = xsByY[y].Where(it => it < x).Max();
                for (int i = x; i < x1; i++)
                {
                    interiorCoordinates.Add(new(i, y));
                }

                if (x < x1)
                {
                    // go back up
                    s.Enqueue((x, x1 - 1, y - dy, -dy));
                }
            }

            while (x1 <= x2)
            {
                var newX1 = xsByY[y].First(it => it >= x1);
                for (int i = x1; i < newX1; i++)
                {
                    interiorCoordinates.Add(new(i, y));
                }

                x1 = newX1;

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