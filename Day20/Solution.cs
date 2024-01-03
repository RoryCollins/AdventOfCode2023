using static Day20.Pulse;

namespace Day20;

internal enum Pulse
{
    Low,
    High
}

public class Solution
{
    private readonly Dictionary<string, Module> modules = new();

    public Solution(List<string> input)
    {
        foreach (var line in input)
        {
            var module = Module.From(line, out var name);
            modules.Add(name, module);
        }

        foreach (var module in modules)
        {
            if (module.Value is Conjunction conjunction)
            {
                conjunction.SetInputs(
                    modules
                        .Where(it => it.Value.ContainsListener(module.Key))
                        .Select(it => it.Key));
            }
        }
    }

    public object PartOne()
    {
        var s = new Queue<(string, Pulse, string)>();
        var b = new Button();
        for (int i = 0; i < 1000; i++)
        {
            b.Click();
            s.Enqueue(("button", Low, "broadcaster"));
            while (s.Count > 0)
            {
                var (source, pulse, destination) = s.Dequeue();
                var module = modules.GetValueOrDefault(destination, new TestModule());
                var results = module.HandlePulse(pulse, source);
                foreach (var (nextDestination, nextPulse) in results)
                {
                    s.Enqueue((destination, nextPulse, nextDestination));
                }
            }
        }

        var lowPulseCount = Module.LowPulseCount;
        var highPulseCount = Module.HighPulseCount;
        return lowPulseCount * highPulseCount;
    }

    public object PartTwo()
    {
        return "Not implemented yet";
    }
}