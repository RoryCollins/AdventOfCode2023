namespace Day13;

public class Solution
{
    private readonly List<List<string>> patterns;
    private readonly Dictionary<List<string>, int> patternScores;

    public Solution(string input)
    {
        this.patterns = input.Split("\r\n\r\n")
            .Select(it => it.Split("\r\n")
                .ToList())
            .ToList();
        patternScores = patterns.ToDictionary(i => i, GetReflectionScore);
    }

    public object PartOne()
    {
        return patternScores.Sum(it => it.Value);
    }

    public object PartTwo()
    {
        return patterns.Sum(GetNonSmudgedScore);
    }

    private int GetNonSmudgedScore(List<string> pattern)
    {
        for (int y = 0; y < pattern.Count; y++)
        {
            for (int x = 0; x < pattern[0].Length; x++)
            {
                var line = pattern[y].ToArray();
                var current = pattern[y][x];
                line[x] = current == '#' ? '.' : '#';

                var newPattern = pattern.Take(y)
                    .Append(string.Join("", line))
                    .Concat(pattern.Skip(y + 1))
                    .ToList();

                var alternateScores = GetReflectionScores(newPattern)
                    .Except(new[] { patternScores[pattern] }).ToList();
                if (alternateScores.Any()) return alternateScores.First();
            }
        }

        return -1;
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

    private IEnumerable<int> GetReflectionScores(List<string> pattern)
    {
        var reflectionScores = GetReflectionPoints(pattern)
            .Select(reflectionPoint => 100 * reflectionPoint);

        var xs = new List<string>();
        for (var index = 0; index < pattern[0].Length; index++)
        {
            xs.Add(string.Join("", pattern.Select(it => it[index])));
        }

        return reflectionScores.Concat(GetReflectionPoints(xs));
    }

    private List<int> GetReflectionPoints(List<string> xs)
    {
        var reflectionPoints = new List<int>();
        for (int i = 0; i < xs.Count - 1; i++)
        {
            if (xs[i] != xs[i + 1]) continue;
            if (CheckReflectionPoint(xs, i)) reflectionPoints.Add(i+1);
        }

        return reflectionPoints;
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
        for (int i = start; i < r; i++)
        {
            if (xs[i] != xs[((2 * r) + 1) - i]) return false;
        }

        return true;
    }
}