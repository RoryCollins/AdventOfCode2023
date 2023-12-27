using System.Text.RegularExpressions;
using Shared;
using static Shared.Direction;

namespace Day18;

public class Solution(IEnumerable<string> input)
{
    private static Direction GetDirection(string d)
    {
        return d switch
        {
            "R" or "0" => East,
            "D" or "1" => South,
            "L" or "2" => West,
            "U" or "3" => North,
            _ => throw new ProgrammerMistake()
        };
    }

    public object PartOne()
    {
        var commands = input
            .Select(line => Regex.Match(line, @"(?<Direction>R|L|U|D) (?<Steps>\d+)"))
            .Select(m => (Direction: GetDirection(m.Groups["Direction"].Value),
                Steps: long.Parse(m.Groups["Steps"].Value)));
        return GetLavaVolume(commands);
    }

    public object PartTwo()
    {
        var commands = input
            .Select(line => Regex.Match(line, @"\(#(?<Steps>\w*)(?<Direction>[0-3])\)"))
            .Select(m => (Direction: GetDirection(m.Groups["Direction"].Value),
                Steps: Convert.ToInt64(m.Groups["Steps"].Value, 16)));
        return GetLavaVolume(commands);
    }

    private long GetLavaVolume(IEnumerable<(Direction Direction, long Steps)> commands)
    {
        var b = 0L;
        var corners = commands.Aggregate(new List<(long X, long Y)> { (0L, 0L) }, (acc, cs) =>
        {
            var direction = Coordinate2D.FromDirection(cs.Direction);
            var previousCoordinate = acc.Last();
            
            var dx = direction.X * cs.Steps;
            var dy = direction.Y * cs.Steps;
            
            var x = previousCoordinate.X + dx;
            var y = previousCoordinate.Y + dy;

            b += cs.Steps;
            
            return acc.Append((x, y)).ToList();
        }).ToHashSet();
        
        var area = GetAreaByShoelaceFormula(corners);
        return b + area - (b / 2) + 1;
    }

    private static long GetAreaByShoelaceFormula(IReadOnlyCollection<(long X, long Y)> corners)
    {
        var windows = corners.ToList()
            .Append(corners.First())
            .Windowed(2)
            .Select(it => (it.First().X * it.Last().Y) - (it.First().Y * it.Last().X));
        return Math.Abs(windows.Sum() / 2);
    }
}