namespace Day05;

using System.Text.RegularExpressions;
using Shared;

public class Solution
{
    private readonly List<string> input;
    private readonly Dictionary<string, IEnumerable<Map>> mappers = new();
    private readonly IEnumerable<long> seeds;

    public Solution(string input)
    {
        this.input = input.Split("\r\n\r\n")
            .ToList();
        seeds = Regex.Matches(this.input[0].Split(':')[1], @"\d+")
            .Select(it => long.Parse(it.Value));
        foreach (var block in this.input.Skip(1))
        {
            var lines = block.Split("\r\n");
            var title = lines[0]
                .Split(" ")
                .First();
            var maps = lines.Skip(1)
                .Select(it =>
                {
                    var ms = Regex.Matches(it, @"\d+")
                        .Select(m => long.Parse(m.Value))
                        .ToList();
                    return new Map(ms[0], ms[1], ms[2]);
                });

            mappers.Add(title, maps);
        }
    }

    public object PartOne()
    {
        return seeds.Min(it => new Seed(this.mappers, it)
            .ToSoil()
            .ToFertilizer()
            .ToWater()
            .ToLight()
            .ToTemperature()
            .ToHumidity()
            .ToLocation().Value);
    }

    public object PartTwo()
    {
        var seedsWithRanges = seeds
            .Windowed(2, 2)
            .SelectMany(it => Enumerable.Range(0, (int)it[1])
                .Select(i => it[0] + i));

        return seedsWithRanges.Min(it => new Seed(this.mappers, it)
            .ToSoil()
            .ToFertilizer()
            .ToWater()
            .ToLight()
            .ToTemperature()
            .ToHumidity()
            .ToLocation().Value);
    }
}

record Map(long DestinationStart, long SourceStart, long Length);

abstract record Element(Dictionary<string, IEnumerable<Map>> maps, long Value)
{
    protected abstract string mapper { get; }

    protected long ConvertValue()
    {
        var map = maps[mapper]
            .Where(it => Value >= it.SourceStart && Value < it.SourceStart + it.Length)
            .ToList();
        if (!map.Any())
        {
            return Value;
        }

        var c = map.Single();
        return (Value - c.SourceStart) + c.DestinationStart;
    }
}

record Seed(Dictionary<string, IEnumerable<Map>> Maps, long Value) : Element(Maps, Value)
{
    protected override string mapper => "seed-to-soil";
    public Soil ToSoil()
    {
        return new Soil(Maps, ConvertValue());
    }
}

record Soil(Dictionary<string, IEnumerable<Map>> Maps, long Value) : Element(Maps, Value)
{
    protected override string mapper => "soil-to-fertilizer";
    public Fertilizer ToFertilizer()
    {
        return new Fertilizer(Maps, ConvertValue());
    }
}

record Fertilizer(Dictionary<string, IEnumerable<Map>> Maps, long Value) : Element(Maps, Value)
{
    protected override string mapper => "fertilizer-to-water";
    public Water ToWater()
    {
        return new Water(Maps, ConvertValue());
    }
}

record Water(Dictionary<string, IEnumerable<Map>> Maps, long Value) : Element(Maps, Value)
{
    protected override string mapper => "water-to-light";
    public Light ToLight()
    {
        return new Light(Maps, ConvertValue());
    }
}
record Light(Dictionary<string, IEnumerable<Map>> Maps, long Value) : Element(Maps, Value)
{
    protected override string mapper => "light-to-temperature";
    public Temperature ToTemperature()
    {
        return new Temperature(Maps, ConvertValue());
    }
}
record Temperature(Dictionary<string, IEnumerable<Map>> Maps, long Value) : Element(Maps, Value)
{
    protected override string mapper => "temperature-to-humidity";
    public Humidity ToHumidity()
    {
        return new Humidity(Maps, ConvertValue());
    }
}
record Humidity(Dictionary<string, IEnumerable<Map>> Maps, long Value) : Element(Maps, Value)
{
    protected override string mapper => "humidity-to-location";
    public Location ToLocation()
    {
        return new Location(ConvertValue());
    }
}

record Location(long Value);