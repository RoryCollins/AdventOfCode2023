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