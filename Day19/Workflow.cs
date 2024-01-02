using Shared;

namespace Day19;

internal class Workflow
{
    private IEnumerable<Process> processes;

    public Workflow(string definition)
    {
        processes = definition.Split(',')
            .Select(
                condition => Day19.Process.From(condition)
            );
    }

    public string Process(Part part)
    {
        foreach (var process in processes)
        {
            var result = process.Execute(part);
            if (result is null) continue;
            return result;
        }

        throw new ProgrammerMistake();
    }

    public List<(Part, Part, string)> GetPossibilities(Part min, Part max)
    {
        var parts = new List<(Part, Part, string)>();

        var currentMin = min;
        var currentMax = max;
        
        foreach (var process in processes)
        {
            var results = process.GetBoundaryDestinations(currentMin, currentMax);
            parts.Add(results.Single(it => it.destination is not null)!);
            if (results.Count == 1) break;

            (currentMin, currentMax, _) = results.First(it => it.destination is null);
        }

        return parts;
    }
}