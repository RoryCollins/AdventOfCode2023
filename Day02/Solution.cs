namespace Day02;

using System.Text.RegularExpressions;

public partial class Solution
{
    private readonly IEnumerable<string> input;
    private readonly Dictionary<string, int> reference = new()
    {
        {"red", 12},
        {"green", 13},
        {"blue", 14},
    };

    private static readonly string[] Colours = { "red", "green", "blue" };

    public Solution(IEnumerable<string> input)
    {
        this.input = input;
    }

    public object PartOne()
    {
        return input.Select(ParseGame)
            .Where(it => it.isValid)
            .Sum(it => it.id);
    }

    public object PartTwo()
    {
        return input.Sum(ScoreGame);
    }

    private (int id, bool isValid) ParseGame(string game)
    {
        var id = int.Parse(GameIdRegex().Match(game).Groups[1].Value);
        var gameValid = Colours.All(colour => ValidateGameColour(game, colour));
        return (id, gameValid);
    }

    private bool ValidateGameColour(string game, string colour)
    {
        return reference[colour] >= MaxGameColour(game, colour);
    }

    private static int MaxGameColour(string game, string colour)
    {
        return new Regex($"(\\d+) {colour}").Matches(game)
            .Select(it => it.Groups[1].Value)
            .Select(int.Parse)
            .Max();
    }

    private static int ScoreGame(string game)
    {
        return Colours.Select(colour => MaxGameColour(game, colour))
            .Aggregate(1, (x, y) => x * y);
    }

    [GeneratedRegex("Game (\\d+):")]
    private static partial Regex GameIdRegex();
}