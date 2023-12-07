namespace Day07;

public class Solution
{
    private readonly IEnumerable<Game> games;

    public Solution(IEnumerable<string> input)
    {
        games = input.Select(it => it.Split(" "))
            .Select(it => new Game(new Hand(it[0]), int.Parse(it[1])));
    }

    public object PartOne()
    {
        return games
            .OrderBy(game => game.Hand)
            .Select((game, index) => game.Bid * (index + 1))
            .Sum();
    }


    public object PartTwo()
    {
        return "modified Part One";
    }
}

internal record Game(Hand Hand, int Bid);