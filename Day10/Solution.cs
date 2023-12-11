namespace Day10;

using Shared;
using static Shared.Coordinate2D;

public class Solution
{
    private readonly List<string> input;
    private readonly Coordinate2D start;
    private readonly HashSet<Coordinate2D> loopParts = new();

    public Solution(List<string> input)
    {
        this.input = input;
        for (int i = 0; i < input.Count(); i++)
        {
            var s = input[i]
                .IndexOf('S');
            if (s == -1) continue;
            start = new Coordinate2D(s, -i);
            loopParts.Add(start);
            break;
        }
        ConstructLoop();
    }

    public object PartOne()
    {
        return (loopParts.Count + 1) / 2;
    }

    private void ConstructLoop()
    {
        char instruction;
        var previous = start;
        var current = start.Add(East);
        while (true)
        {
            loopParts.Add(current);
            instruction = input[-current.Y][current.X];
            if (instruction == 'S') return;

            var candidates = instruction switch
            {
                'J' => new[] { West, North },
                'F' => new[] { South, East },
                'L' => new[] { North, East },
                '7' => new[] { South, West },
                '-' => new[] { East, West },
                '|' => new[] { South, North },
                _ => throw new ArgumentOutOfRangeException()
            };
            var next = candidates
                .Select(it => it.Add(current))
                .Single(it => it != previous);
            previous = current;
            current = next;
        }
    }

    public object PartTwo()
    {
        var insideSquares = 0;
        for (int y = 0; y < input.Count; y++)
        {
            var line = input[y].ToArray();
            for (int x = 0; x < input[y].Length; x++)
            {
                if (loopParts.Contains(new Coordinate2D(x, -y))) continue;
                line[x] = '.';
            }

            var cleanLine = string.Join("", line)
                .Replace("-", "")
                .Replace("F7", "")
                .Replace("LJ", "")
                .Replace("L7", "|")
                .Replace("FJ", "|");
            var inside = false;
            foreach (var instruction in cleanLine)
            {
                if (instruction == '|')
                {
                    inside = !inside;
                    continue;
                }
                if (inside)
                {
                    insideSquares++;
                }
            }
        }

        return insideSquares;
    }
}