namespace Day20;

internal class Broadcaster : Module
{
    public Broadcaster(List<string> listeners) : base(listeners)
    {
    }

    public override List<(string, Pulse)> HandlePulse(Pulse pulse, string source)
    {
        return Emit(pulse);
    }
}