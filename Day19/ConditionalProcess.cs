using Shared;

namespace Day19;

class ConditionalProcess : Process
{
    private readonly string component;
    private readonly Func<Part, long> partComponent;
    private readonly string comparator;
    private readonly long value;
    private readonly string destination;

    public ConditionalProcess(string component, string comparator, long value, string destination)
    {
        this.component = component;
        partComponent = p => component switch
        {
            "x" => p.X,
            "m" => p.M,
            "a" => p.A,
            "s" => p.S,
            _ => throw new ProgrammerMistake()
        };
        this.comparator = comparator;
        this.value = value;
        this.destination = destination;
    }

    private bool Pass(Part part)
    {
        return comparator switch
        {
            ">" => partComponent(part) > value,
            "<" => partComponent(part) < value,
            _ => throw new ProgrammerMistake()
        };
    }


    public override string? Execute(Part p)
    {
        return Pass(p) ? destination : null;
    }

    public override List<(Part min, Part max, string? destination)> GetBoundaryDestinations(Part min, Part max)
    {
        if (comparator == "<")
        {
            var newMax = component switch
            {
                "x" => max with { X = value - 1 },
                "m" => max with { M = value - 1 },
                "a" => max with { A = value - 1 },
                "s" => max with { S = value - 1 },
                _ => throw new ProgrammerMistake()
            };
            var newMin = component switch
            {
                "x" => min with { X = value },
                "m" => min with { M = value },
                "a" => min with { A = value },
                "s" => min with { S = value },
                _ => throw new ProgrammerMistake()
            };
            return [
                (min, newMax, destination),
                (newMin, max, null)
            ];
        }
        else
        { 
            var newMax = component switch
            {
                "x" => max with { X = value },
                "m" => max with { M = value },
                "a" => max with { A = value },
                "s" => max with { S = value },
                _ => throw new ProgrammerMistake()
            };
            var newMin = component switch
            {
                "x" => min with { X = value + 1},
                "m" => min with { M = value + 1},
                "a" => min with { A = value + 1},
                "s" => min with { S = value + 1},
                _ => throw new ProgrammerMistake()
            };
            return [
                (min, newMax, null),
                (newMin, max, destination)
            ];
        }
    }
}