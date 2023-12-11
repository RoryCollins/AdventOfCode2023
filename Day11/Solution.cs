namespace Day11;

using System.Text.RegularExpressions;
using Shared;

public class Solution
{
    private readonly List<string> input;
    private List<string> expandedCosmos = new();
    private HashSet<Coordinate2D> galaxies = new();
    private Dictionary<Coordinate2D, Coordinate2D> galaxyGrids = new();

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
        var ys = new List<int>();
        var xs = new List<int>();

        for (int y = 0; y < input.Count; y++)
        {
            if (input[y].Contains('#')) continue;
            ys.Add(y);
        }
        ys.Add(input.Count);

        for (int x = 0; x < input[0].Length; x++)
        {
            if (input.Any(it => it[x] == '#')) continue;
            xs.Add(x);
        }
        xs.Add(input[0].Length);

        galaxies = FindGalaxies(input);
        foreach (var galaxy in galaxies)
        {
            var gridx = xs.IndexOf(xs.First(it => galaxy.X < it));
            var gridy = ys.IndexOf(ys.First(it => galaxy.Y < it));

            galaxyGrids.Add(galaxy, new Coordinate2D(gridx, gridy));
        }

        return galaxies.ChooseTwo()
            .Sum(pair => GalacticDistance(pair, 1_000_000));
    }

    private long GalacticDistance((Coordinate2D, Coordinate2D) pair, int expansions)
    {
        var c1 = pair.Item1;
        var c2 = pair.Item2;
        if (galaxyGrids[c1] == galaxyGrids[c2]) return c1.ManhattanDistanceTo(c2);
        var expansionDistance = (long)((expansions-1) * galaxyGrids[c1]
            .ManhattanDistanceTo(galaxyGrids[c2]));
        return c1.ManhattanDistanceTo(c2) + expansionDistance;
    }

    private List<string> ExpandCosmos(IEnumerable<string> previousCosmos)
    {

        var nextCosmos = previousCosmos.ToList();
        for (int y = 0; y < nextCosmos.Count; y++)
        {
            if (nextCosmos[y].Contains('#')) continue;

            nextCosmos.Insert(y, nextCosmos[y]);
            y++;
        }

        for (int x = 0; x < nextCosmos[0].Length; x++)
        {
            if (nextCosmos.Any(it => it[x] == '#')) continue;

            for (int y = 0; y < nextCosmos.Count; y++)
            {
                nextCosmos[y] = nextCosmos[y]
                    .Insert(x, ".");
            }

            x++;
        }

        return nextCosmos;
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