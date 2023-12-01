namespace Day01;

using System.Text.RegularExpressions;

public partial class Solution
{
    private readonly IEnumerable<string> input;

    public Solution(IEnumerable<string> input)
    {
        this.input = input;

    }

    public object PartOne()
    {
        return input.Sum(GetNumberFromString);
    }

    public object PartTwo()
    {
        var sum = 0;
        foreach (var s in input)
        {
            var matches = MyRegex().Matches(s);
            var first = Parse(matches.First().Groups[1].ToString());
            var last = Parse(matches.Last().Groups[1].ToString());
            sum += int.Parse($"{first}{last}");
        }
        return sum;
    }

    private static int Parse(string s)
    {
        return s switch
        {
            "one" => 1,
            "two" => 2,
            "three" => 3,
            "four" => 4,
            "five" => 5,
            "six" => 6,
            "seven" => 7,
            "eight" => 8,
            "nine" => 9,
            _ => int.Parse(s)
        };
    }

    private int GetNumberFromString(string s)
    {
        var first = s.First(c => c >= '0' && c <= '9');
        var last = s.Last(c => c >= '0' && c <= '9');
        return int.Parse($"{first}{last}");
    }

    [GeneratedRegex("(?=(one|two|three|four|five|six|seven|eight|nine|[0-9]))")]
    private static partial Regex MyRegex();
}