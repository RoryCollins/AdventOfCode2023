using System.Text.RegularExpressions;
using Shared;
using static Shared.Direction;

namespace Day18;

public class Solution
{
    private readonly IEnumerable<(Direction Direction, int Steps)> commands;

    public Solution(List<string> input)
    {
        commands = input
            .Select(line => Regex.Match(line, @"(?<Direction>R|L|U|D) (?<Steps>\d+)"))
            .Select(m => (Direction:GetDirection(m.Groups["Direction"].Value), Steps:int.Parse(m.Groups["Steps"].Value)));
    }

    private static Direction GetDirection(string d)
    {
        return d switch
        {
            "R" => East,
            "L" => West,
            "U" => North,
            "D" => South,
            _ => throw new ProgrammerMistake()
        };
    }

    public object PartOne()
    {
        var start = Coordinate2D.Origin;
        var cs = commands.Aggregate(new HashSet<Coordinate2D>{start}, (acc, c) => acc.Concat(ProcessCommand(acc.Last(), c.Direction, c.Steps)).ToHashSet());

        return cs.Count;
    }

    private static List<Coordinate2D> ProcessCommand(Coordinate2D start, Direction direction, int steps)
    {
        var cs = new List<Coordinate2D>();
        for (int i = 1; i <= steps; i++)
        {
            cs.Add(start.Add(Coordinate2D.FromDirection(direction) * i));
        }
        return cs;
    }

    public object PartTwo()
    {
        return "Not implemented yet";
    }
}