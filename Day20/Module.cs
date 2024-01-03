using static Day20.Pulse;

namespace Day20;

internal abstract class Module
{
    protected readonly List<string> listeners;
    public static int LowPulseCount { get; private set; } = 0;
    public static int HighPulseCount { get; private set; } = 0;

    public static Module From(string definition, out string name)
    {
        var parts = definition.Split("->", StringSplitOptions.TrimEntries);
        var listeners = parts[1].Split(",", StringSplitOptions.TrimEntries).ToList();
        name = parts[0].Replace("%", "").Replace("&", "");
        if (parts[0] == "broadcaster")
        {
            return new Broadcaster(listeners);
        }

        if (parts[0].StartsWith("%"))
        {
            return new FlipFlop(listeners);
        }

        return new Conjunction(listeners);
    }

    public abstract List<(string, Pulse)> HandlePulse(Pulse pulse, string source);

    protected Module(List<string> listeners)
    {
        this.listeners = listeners;
    }

    public bool ContainsListener(string module)
    {
        return listeners.Contains(module);
    }

    protected List<(string, Pulse)> Emit(Pulse pulse)
    {
        if (pulse == Low) LowPulseCount += listeners.Count;
        else HighPulseCount += listeners.Count;

        return listeners.Select(it => (it, pulse)).ToList();
    }
}

internal class TestModule() : Module([])
{
    public override List<(string, Pulse)> HandlePulse(Pulse pulse, string source)
    {
        return [];
    }
}

class Button : Module
{
    public Button() : base(["broadcaster"])
    { }

    public override List<(string, Pulse)> HandlePulse(Pulse pulse, string source)
    {
        throw new NotImplementedException();
    }

    public void Click()
    {
        Emit(Low);
    }
}