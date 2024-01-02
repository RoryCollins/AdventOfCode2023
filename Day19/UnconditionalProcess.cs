namespace Day19;

class UnconditionalProcess(string destination) : Process
{
    public override string? Execute(Part p)
    {
        return destination;
    }

    public override List<(Part min, Part max, string destination)> GetBoundaryDestinations(Part min, Part max)
    {
        return new List<(Part, Part, string)> { (min, max, destination) };
    }
}