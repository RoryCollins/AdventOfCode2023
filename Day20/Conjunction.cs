using static Day20.Pulse;

namespace Day20;

internal class Conjunction : Module
{
    private Dictionary<string, Pulse> memory;
    public Conjunction(List<string> listeners) : base(listeners)
    {
    }

    public override List<(string, Pulse)> HandlePulse(Pulse pulse, string source)
    {
        memory[source] = pulse;
        return memory.Any(it => it.Value == Low)
            ? Emit(High)
            : Emit(Low);
    }

    public void SetInputs(IEnumerable<string> inputs)
    {
        memory = inputs.ToDictionary(it => it, _ => Low);
    }
}