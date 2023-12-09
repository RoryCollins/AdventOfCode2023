namespace Day09;

using Shared;

public class Solution
{
    private readonly IEnumerable<List<int>> input;

    public Solution(IEnumerable<string> input)
    {
        this.input = input.Select(line => line.Split(" ")
            .Select(int.Parse).ToList());
    }

    public object PartOne()
    {
        return input.Sum(NextInSequence);
    }

    public object PartTwo()
    {
        return input.Sum(PreviousInSequence);
    }

    private static int NextInSequence(List<int> xs)
    {
        var diffs = xs.Windowed(2)
            .Select(x => x[1] - x[0])
            .ToList();
        if (diffs.Distinct().Count() == 1) return xs.Last() + diffs.Last();
        var nextInSequence = NextInSequence(diffs);
        return xs.Last() + nextInSequence;
    }

    private static int PreviousInSequence(List<int> xs)
    {
        var diffs = xs.Windowed(2)
            .Select(x => x[1] - x[0])
            .ToList();
        if (diffs.Distinct().Count() == 1) return xs.First() - diffs.First();
        var previousInSequence = PreviousInSequence(diffs);
        return xs.First() - previousInSequence;
    }
}