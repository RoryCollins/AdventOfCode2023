namespace Shared;

public record Coordinate2D(int X, int Y)
{
    public Coordinate2D Add(Coordinate2D other)
    {
        return new Coordinate2D(X + other.X, Y + other.Y);
    }

    public static Coordinate2D Origin => new(0, 0);
}

public record Grid(int XSize, int YSize)
{
    public IEnumerable<Coordinate2D> GetNeighbours(Coordinate2D centre, bool includeDiagonals)
    {
        var directions = new[]
        {
            new Coordinate2D(-1, 0),
            new Coordinate2D(0, 1),
            new Coordinate2D(1, 0),
            new Coordinate2D(0, -1),
        };

        if (includeDiagonals)
        {
            directions = directions.Concat(new[]
                {
                    new Coordinate2D(-1, 1),
                    new Coordinate2D(1, 1),
                    new Coordinate2D(1, -1),
                    new Coordinate2D(-1, -1),
                }
            ).ToArray();
        }

        return directions.Select(centre.Add)
            .Where(it => (it.X >= 0 && it.X < XSize)
                         && (it.Y >= 0 && it.Y < YSize));
    }
}