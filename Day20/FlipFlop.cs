using static Day20.Pulse;

namespace Day20;

internal class FlipFlop : Module
{
    private bool state;
    public FlipFlop(List<string> listeners) : base(listeners)
    {
        state = false;
    }

    public override List<(string, Pulse)> HandlePulse(Pulse pulse, string source)
    {
        if (pulse is High) return [];
        state = !state;
        return state switch
        {
            true => Emit(High),
            false => Emit(Low),
        };
    }
}