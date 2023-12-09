namespace Day08;

using System.Text.RegularExpressions;

public class Solution
{
    private readonly string directions;
    private readonly Dictionary<string, (string left, string right)> map;

    public Solution(List<string> input)
    {
        this.directions = input[0];
        map = input.Skip(2)
            .Select(s => Regex.Match(s, @"(?<node>\w{3}) = \((?<left>\w{3}), (?<right>\w{3})\)")
                .Groups)
            .ToDictionary(gs => gs["node"].Value, gs => (gs["left"].Value, gs["right"].Value));
    }

    public object PartOne()
    {
        var currentNode = "AAA";
        return FindEnd(currentNode);
    }

    private int FindEnd(string currentNode)
    {
        int instruction = 0;
        int steps = 0;
        while (true)
        {
            if (currentNode.EndsWith('Z')) break;
            currentNode = directions[instruction] == 'L'
                ? map[currentNode].left
                : map[currentNode].right;
            instruction = (instruction + 1) % directions.Length;
            steps++;
        }

        return steps;
    }

    public object PartTwo()
    {
        return map.Keys.Where(it => it.EndsWith("A"))
            .Select(FindEnd)
            .Select(it => (long)it)
            .Aggregate(1L, LeastCommonMultiple);
    }

    private static long LeastCommonMultiple(long i, long j) => (i * j) / GreatestCommonFactor(i, j);

    private static long GreatestCommonFactor(long i, long j) => j == 0
        ? i
        : GreatestCommonFactor(j, i % j);
}