namespace Day04;

using System.Text.RegularExpressions;

public class Solution
{
    private readonly IEnumerable<int> cards;

    public Solution(IEnumerable<string> input)
    {
        cards = input
            .Select(it => it.Split(new[] { ':', '|' })[1..])
            .Select(side => side.Select(FindAllDigits).ToList())
            .Select(f => (f[0], f[1]))
            .Select(it => it.Item1.Count(n => it.Item2.Contains(n)));
    }

    private static IEnumerable<int> FindAllDigits(string s)
    {
        return Regex.Matches(s, @"\d+")
            .Select(it => int.Parse(it.Value));
    }

    public object PartOne()
    {
        return cards
            .Where(f => f > 0)
            .Sum(f => Math.Pow(2, f - 1));
    }

    public object PartTwo()
    {
        var cardsWithCount = cards.Select(it => (it, 1)).ToList();
        return TotalCountOfCardsInList(cardsWithCount);
    }

    private int TotalCountOfCardsInList(List<(int, int)> cs)
    {
        if (!cs.Any()) return 0;

        var (matchingNumbers, count) = cs.First();
        var remainder = cs.Skip(1).ToList();
        if (count == 0) return TotalCountOfCardsInList(remainder);
        return count + TotalCountOfCardsInList(remainder
            .Take(matchingNumbers).Select(i => (i.Item1, i.Item2 + count))
            .Concat(remainder.Skip(matchingNumbers))
            .ToList());
    }
}