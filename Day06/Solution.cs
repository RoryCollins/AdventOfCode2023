namespace Day06;

using System.Text.RegularExpressions;

public class Solution
{
    private readonly IEnumerable<string> input;
    private IEnumerable<(int Time, int Distance)> races;
    private readonly (long Time, long Distance) bigRace;

    public Solution(List<string> input)
    {
        var lines = input.Select(it => Regex.Matches(it, @"\d+")
            .Select(m => int.Parse(m.Value))).ToList();
        races = lines.First().Zip(lines.Last());

        var bigRaceLines = input
            .Select(it => it.Replace(" ", ""))
            .Select(it => it.Split(":")[1])
            .Select(long.Parse)
            .ToArray();

        bigRace = (bigRaceLines[0], bigRaceLines[1]);
    }

    public object PartOne()
    {
        return races.Aggregate(1, (i, race) => i * SolveQuadratic(race));
    }

    public object PartTwo()
    {
        return SolveQuadratic(bigRace);
    }

    public int SolveQuadratic((long Time, long Distance) race)
    {
        var roots = new[]{1, -1}.Select(it => ((race.Time) + it*Math.Sqrt(Math.Pow(race.Time, 2) - (4 * race.Distance))) / 2).ToArray();
        return (int)(Math.Ceiling(roots.Max()-1) - Math.Floor(roots.Min()));
    }
}