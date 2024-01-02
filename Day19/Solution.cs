using System.Text.RegularExpressions;

namespace Day19;

public class Solution
{
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
        var acceptedPile = new List<(Part, Part)>();
        var min = new Part(1, 1, 1, 1);
        var max = new Part(4000, 4000, 4000, 4000);
        var s = new Queue<(Part, Part, string)>();
        s.Enqueue((min, max, "in"));
        while (s.Count > 0)
        {
            var (currentMin, currentMax, dest) = s.Dequeue();
            var results = workflows[dest].GetPossibilities(currentMin, currentMax);
            foreach (var (newMin, newMax, newDest) in results)
            {
                if(newDest is "A") acceptedPile.Add((newMin, newMax));
                if (newDest is "A" or "R") continue;
                s.Enqueue((newMin, newMax, newDest));
            }
        }

        return acceptedPile.Sum(it =>
            (it.Item2.X - it.Item1.X + 1) 
            * (it.Item2.M - it.Item1.M + 1) 
            * (it.Item2.A - it.Item1.A + 1) 
            * (it.Item2.S - it.Item1.S + 1));
    }

    private static long ScorePart(Part part)
    {
        return part.X + part.M + part.A + part.S;
    }
}