namespace Day03;

using System.Text.RegularExpressions;
using Shared;

public class Solution
{
    private readonly List<string> input;
    private Dictionary<Coordinate2D, Dictionary<Coordinate2D, int>> starLookups = new();
    private Grid grid;

    public Solution(List<string> input)
    {
        this.input = input;
        grid = new Grid(input);
    }

    public object PartOne()
    {
        var sum = 0;
        for (int i = 0; i < input.Count; i++)
        {
            var numbers = new Regex(@"(\d+)").Matches(input[i]);
            foreach (Match number in numbers)
            {
                bool valid = false;
                for (int j = number.Index; j < number.Index + number.Length; j++)
                {
                    var neighbours = grid.GetNeighbours(new Coordinate2D(i, j), true).ToList();
                    var stars = neighbours.Where(it => input[it.X][it.Y] == '*');
                    foreach (var starCoordinate in stars)
                    {
                        var d = starLookups.TryGetValue(starCoordinate, out var current) ? current : new();
                        d.Add(new Coordinate2D(i, number.Index), int.Parse(number.Value));
                        starLookups[starCoordinate] = d;
                    }
                    if (neighbours.Any(SymbolAtCoordinate))
                    {
                        valid = true;
                        break;
                    }
                }

                if (valid)
                {
                    // Console.WriteLine(number.Value);
                    sum+= int.Parse(number.Value);
                }
            }
        }

        return sum;
    }

    private bool SymbolAtCoordinate(Coordinate2D arg)
    {
        return Regex.IsMatch(input[arg.X][arg.Y].ToString(), @"[^\.\d]");
    }

    public object PartTwo()
    {
        var gears = starLookups.Where(s => s.Value.Count == 2)
            .Select(i => i.Value)
            .Select(it => it.Aggregate(1, (i, pair) => i * pair.Value));

        return gears.Sum();
    }
}