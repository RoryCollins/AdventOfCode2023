namespace Day11;

using System.Text.RegularExpressions;
using Shared;

public class Solution
{
    private readonly List<string> cosmos;
    private HashSet<Coordinate2D> galaxies = new();

    public Solution(List<string> input)
    {
        cosmos = input;
        ExpandCosmos();
        galaxies = FindGalaxies();
    }

    private HashSet<Coordinate2D> FindGalaxies()
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

    private void ExpandCosmos()
    {
        for (int y = 0; y < cosmos.Count; y++)
        {
            if (!cosmos[y]
                    .Contains('#'))
            {
                cosmos.Insert(y, cosmos[y]);
                y++;
            }
        }

        for (int x = 0; x < cosmos[0].Length; x++)
        {
            if (cosmos.All(it => it[x] == '.'))
            {
                for (int y = 0; y < cosmos.Count; y++)
                {
                    cosmos[y] = cosmos[y]
                        .Insert(x, ".");
                }

                x++;
            }
        }
    }

    public object PartOne()
    {
        return galaxies.ChooseTwo().Sum(it => it.Item1.ManhattanDistanceTo(it.Item2));
    }

    public object PartTwo()
    {
        return "Not yet implemented";
    }
}