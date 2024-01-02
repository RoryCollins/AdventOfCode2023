using System.Text.RegularExpressions;

namespace Day19;

internal abstract class Process
{
    public abstract string? Execute(Part p);
    public static Process From(string definition)
    {
        var m = Regex.Match(definition,
            @"(?<component>x|m|a|s)(?<comparator>\<|\>)(?<value>\d+):(?<destination>\w+)");
        if (!m.Success)
        {
            return new UnconditionalProcess(definition);
        }

        return new ConditionalProcess(
            m.Groups["component"].Value,
            m.Groups["comparator"].Value,
            long.Parse(m.Groups["value"].Value),
            m.Groups["destination"].Value);
    }

    public abstract List<(Part min, Part max, string? destination)> GetBoundaryDestinations(Part min, Part max);
}