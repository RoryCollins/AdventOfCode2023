namespace Day11;

using System.Text.RegularExpressions;
using Shared;

public class Solution
{
    private readonly List<string> input;
    private List<string> expandedCosmos = new();
    private HashSet<Coordinate2D> galaxies = new();

    public Solution(List<string> input)
    {
        this.input = input;
    }

    public object PartOne()
    {
        expandedCosmos = ExpandCosmos(input);
        galaxies = FindGalaxies(expandedCosmos);
        return galaxies.ChooseTwo().Sum(it => it.Item1.ManhattanDistanceTo(it.Item2));
    }

    public object PartTwo()
    {
        return "Not yet implemented";
    }

    private List<string> ExpandCosmos(List<string> cosmos)
    {
        for (int y = 0; y < cosmos.Count; y++)
        {
            if (cosmos[y].Contains('#')) continue;

            cosmos.Insert(y, cosmos[y]);
            y++;
        }

        for (int x = 0; x < cosmos[0].Length; x++)
        {
            if (cosmos.Any(it => it[x] == '#')) continue;

            for (int y = 0; y < cosmos.Count; y++)
            {
                cosmos[y] = cosmos[y]
                    .Insert(x, ".");
            }

            x++;
        }

        return cosmos;
    }

    private HashSet<Coordinate2D> FindGalaxies(List<string> cosmos)
    {
        var gs = new HashSet<Coordinate2D>();
        for (int y = 0; y < cosmos.Count; y++)
        {
            var index = y;
            var coords = Regex.Matches(cosmos[y], @"#")
                .Select(it => new Coordinate2D(it.Index, index));
            gs = gs.Concat(coords)
                .ToHashSet();
        }

        return gs;
    }
}