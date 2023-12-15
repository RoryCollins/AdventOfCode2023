namespace Day14;

public class Solution
{
    private readonly List<string> platform;
    private Dictionary<List<string>, int> cycles = new(comparer: new PlatformComparer());
    private readonly int xSize;

    public Solution(List<string> input)
    {
        this.platform = input;
        xSize = input[0].Length;
    }

    public object PartOne()
    {
        // var foo = Enumerable.Range(1, 20);
        // var p = platform;
        // foreach (var i in foo)
        // {
        //     p = p.Cycle()
        //         .ToList();
        //     var f = p
        //         .Select((row, index) => row.Count(c => c == 'O') * (xSize - index))
        //         .Sum();
        //     Console.WriteLine($"{i}: {f}");
        // }

        // return "fooo";
        return platform.TiltNorth()
        .Select((row, index) => row.Count(c => c == 'O') * (xSize-index))
        .Sum();
    }

    public object PartTwo()
    {
        var p = platform;
        int count = 0;
        int cycleStart;
        int cycleLength;

        while (true)
        {
            if (cycles.TryGetValue(p, out var indexedCycle))
            {
                cycleStart = indexedCycle;
                cycleLength = count - cycleStart;
                break;
            }

            var cycle = p.Cycle()
                .ToList();
            cycles.Add(p, count);
            p = cycle;
            count++;
        }

        var foo = (1_000_000_000-cycleStart) % cycleLength + cycleStart;
        var q = platform;
        for (int i = 1; i <= foo; i++)
        {
            q = q.Cycle()
                .ToList();
        }

        return q
            .Select((row, index) => row.Count(c => c == 'O') * (xSize - index))
            .Sum();
    }
}

class PlatformComparer : IEqualityComparer<List<string>>
{
    public bool Equals(List<string>? x, List<string>? y)
    {
        return y != null && (x?.SequenceEqual(y) ?? false);
    }

    public int GetHashCode(List<string> obj)
    {
        return obj.Aggregate(0, (current, t) => current ^ t.GetHashCode());
    }
}