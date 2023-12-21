namespace Day16;

using Shared;
using static Shared.Direction;

record Beam(Coordinate2D Location, Direction Direction);

public class Solution
{
    private readonly Grid grid;
    private HashSet<Coordinate2D> visitedCoordinates = new();
    private HashSet<Beam> uniqueBeams = new();

    public Solution(List<string> input)
    {
        grid = new Grid(input);
    }

    public object PartOne()
    {
        return ProcessBeam(new Beam(grid.TopLeft(), East));
    }

    private int ProcessBeam(Beam origin)
    {
        var beams = new List<Beam> { origin };
        while (beams.Any())
        {
            beams = beams.SelectMany(Process)
                .ToList();
        }

        return visitedCoordinates.Count;
    }

    private List<Beam> Process(Beam beam)
    {
        var beams = new List<Beam>();
        visitedCoordinates.Add(beam.Location);
        var newLocation = beam.Location.Move(beam.Direction);
        Direction newDirection;
        switch (grid.At(beam.Location))
        {
            case '.':
                beams.Add(beam with { Location = newLocation });
                break;
            case '-':
                if (new[] { North, South }.Contains(beam.Direction))
                    beams.AddRange(new[] { East, West }.Select(it => beam with {Direction = it}));
                else beams.Add(beam with { Location = newLocation });
                break;
            case '|':
                if (new[] { East, West }.Contains(beam.Direction))
                    beams.AddRange(new[] { North, South }.Select(it => beam with {Direction = it}));
                else beams.Add(beam with { Location = newLocation });
                break;
            case '/':
                newDirection = ForwardSlash(beam.Direction);
                beams.Add(new Beam(beam.Location.Move(newDirection), newDirection));
                break;
            case '\\':
                newDirection = BackwardSlash(beam.Direction);
                beams.Add(new Beam(beam.Location.Move(newDirection), newDirection));
                break;
        }

        return beams
            .Where(it => grid.IsOnGrid(it.Location))
            .Where(it => uniqueBeams.Add(it))
            .ToList();
    }

    private static Direction ForwardSlash(Direction direction) => direction switch
    {
        North => East,
        East => North,
        South => West,
        West => South
    };

    private static Direction BackwardSlash(Direction direction) => direction switch
    {
        North => West,
        West => North,
        South => East,
        East => South
    };

    public object PartTwo()
    {
        var m = 0;
        var xEdges = Enumerable.Range(0, grid.Width)
            .SelectMany(it => new[] { new Beam(new Coordinate2D(it, 0), North), new Beam(new Coordinate2D(it, grid.Height-1), South) });
        var yEdges = Enumerable.Range(0, grid.Height)
            .SelectMany(it => new[] { new Beam(new Coordinate2D(0, it), East), new Beam(new Coordinate2D(grid.Width-1, it), West)});
        var allEdges = xEdges.Concat(yEdges);
        foreach (var b in allEdges)
        {
            visitedCoordinates = new();
            uniqueBeams = new();
            m = new[] { m, ProcessBeam(b) }.Max();
        }

        return m;
    }
}