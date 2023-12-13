namespace Day13;

public class Solution
{
    private readonly List<List<string>> patterns;

    public Solution(string input)
    {
        this.patterns = input.Split("\r\n\r\n").Select(it => it.Split("\r\n").ToList()).ToList();

    }

    public object PartOne()
    {
        foreach (var pattern in patterns)
        {
            Console.WriteLine(GetReflectionScore(pattern));
        }

        return patterns.Sum(GetReflectionScore);
    }

    private int GetReflectionScore(List<string> pattern)
    {
        var horizontalReflectionPoint = GetReflectionPoint(pattern);
        if (horizontalReflectionPoint > 0) return 100 * horizontalReflectionPoint;
        var xs = new List<string>();
        for (var index = 0; index < pattern[0].Length; index++)
        {
            xs.Add(string.Join("", pattern.Select(it => it[index])));
        }

        return GetReflectionPoint(xs);
    }

    private int GetReflectionPoint(List<string> xs)
    {
        for (int i = 0; i < xs.Count - 1; i++)
        {
            if (xs[i] != xs[i + 1]) continue;
            if (CheckReflectionPoint(xs, i)) return i + 1;
        }

        return -1;
    }

    private bool CheckReflectionPoint(List<string> xs, int r)
    {
        var start = Math.Max(0, (2 * (r + 1)) - xs.Count);
        var end = Math.Min(xs.Count-1, 2 * r + 1);
        for (int i = start; i < r; i++)
        {
            var s1 = xs[i];
            var s2 = xs[((2*r)+1) - i];
            if (s1 != s2) return false;
        }

        return true;
    }


    public object PartTwo()
    {
        return "Not yet implemented";
    }
}