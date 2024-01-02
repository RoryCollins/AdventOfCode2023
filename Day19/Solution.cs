using System.Text.RegularExpressions;
using Shared;

namespace Day19;

public class Solution
{
    private record Part(int X, int M, int A, int S);

    private class Workflow
    {
        private IEnumerable<Func<Part, string?>> processes;

        public Workflow(string definition)
        {
            processes = definition.Split(',')
                .Select(
                    condition =>
                    {
                        var m = Regex.Match(condition,
                            @"(?<component>x|m|a|s)(?<comparator>\<|\>)(?<value>\d+):(?<destination>\w+)");
                        if (!m.Success)
                        {
                            return new Func<Part, string?>(_ => condition);
                        }

                        return p =>
                        {
                            var component = m.Groups["component"].Value switch
                            {
                                "x" => p.X,
                                "m" => p.M,
                                "a" => p.A,
                                "s" => p.S,
                                _ => throw new ProgrammerMistake()
                            };
                            bool pass = m.Groups["comparator"].Value switch
                            {
                                ">" => component > int.Parse(m.Groups["value"].Value),
                                "<" => component < int.Parse(m.Groups["value"].Value),
                                _ => throw new ProgrammerMistake()
                            };
                            return pass ? m.Groups["destination"].Value : null;
                        };
                    });
        }

        public string Process(Part part)
        {
            foreach (var condition in processes)
            {
                var result = condition(part);
                if (result is null) continue;
                return result;
            }

            throw new ProgrammerMistake();
        }
    }

    private readonly Dictionary<string, Workflow> workflows;
    private readonly List<Part> parts;

    public Solution(string input)
    {
        var b = input.Split("\n\n");
        workflows = b[0].Split("\n")
            .Select(line => (
                Name: line[..line.IndexOf('{')],
                Workflow: new Workflow(line[(line.IndexOf('{') + 1)..line.IndexOf('}')])
            )).ToDictionary(m => m.Name, m => m.Workflow);

        parts = b[1].Split("\n")
            .Select(it => Regex.Matches(it, @"\d+").Select(m => int.Parse(m.Value)).ToList())
            .Select(ms => new Part(ms[0], ms[1], ms[2], ms[3]))
            .ToList();
    }

    public object PartOne()
    {
        var acceptedPile = new List<Part>();
        Console.WriteLine("PARTS");
        foreach (var part in parts)
        {
            var flow = "in";
            while (true)
            {
                flow = workflows[flow].Process(part);

                if (flow is "A") acceptedPile.Add(part);
                if (flow is "A" or "R") break;
            }
        }

        return acceptedPile.Sum(ScorePart);
    }

    public object PartTwo()
    {
        return "Not implemented yet";
    }

    private static int ScorePart(Part part)
    {
        return part.X + part.M + part.A + part.S;
    }
}