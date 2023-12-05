namespace Day05;

using System.Text.RegularExpressions;
using Shared;

public partial class Solution
{
    private readonly List<long> seeds;
    private IEnumerable<List<(long from, long to, long adjustment)>> maps;

    public Solution(string input)
    {
        var lines = input.Split("\r\n\r\n")
            .ToList();

        seeds = UnsignedNumberRegex().Matches(lines[0]
                .Split(':')[1])
            .Select(it => long.Parse(it.Value)).ToList();

        maps = lines.Skip(1)
            .Select(it => it.Split("\r\n")
                .Skip(1)
                .Select(d => UnsignedNumberRegex()
                    .Matches(d)
                    .Select(m => long.Parse(m.Value))
                    .ToList())
                .Select(m => (m[1], m[1]+m[2]-1, m[0]-m[1])).ToList());
    }

    public object PartOne()
    {
        var seedValues = seeds.Select(Process);
        return seedValues.Min();
    }

    public object PartTwo()
    {
        var seedRanges = seeds
            .Windowed(2, 2)
            .Select(it => (from: it[0], to: it[0] + it[1] - 1))
            .ToList();

        foreach (var map in maps)
        {
            var newRanges = new List<(long from, long to)>();
            foreach (var r in seedRanges)
            {
                var range = r;
                foreach (var mapping in map.OrderBy(x => x.from).ToList())
                {
                    if (range.from < mapping.from)
                    {
                        newRanges.Add((range.from, Math.Min(range.to, mapping.from - 1)));
                        range.from = mapping.from;
                        if (range.from > range.to)
                            break;
                    }

                    if (range.from <= mapping.to)
                    {
                        newRanges.Add((range.from + mapping.adjustment, Math.Min(range.to, mapping.to) + mapping.adjustment));
                        range.from = mapping.to + 1;
                        if (range.from > range.to)
                            break;
                    }
                }
                if (range.from <= range.to)
                    newRanges.Add(range);
            }
            seedRanges = newRanges;
        }
        return seedRanges.Min(r => r.from);
    }

    private long Process(long seed)
    {
        return maps.Aggregate(seed, (l, tuples) =>
        {
            var v = tuples.FirstOrDefault(it => l >= it.from && l <= it.to);
            return l + v.adjustment;
        });
    }

    [GeneratedRegex(@"\d+")]
    private static partial Regex UnsignedNumberRegex();
}