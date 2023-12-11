namespace Shared;

public record Coordinate2D(int X, int Y)
{
    public Coordinate2D Add(Coordinate2D other)
    {
        return new Coordinate2D(X + other.X, Y + other.Y);
    }

    public int ManhattanDistanceTo(Coordinate2D other)
    {
        return Math.Abs(X - other.X) + Math.Abs(Y - other.Y);
    }

    public static Coordinate2D Origin => new(0, 0);
    public static Coordinate2D North => new(0, 1);
    public static Coordinate2D South => new(0, -1);
    public static Coordinate2D East => new(1, 0);
    public static Coordinate2D West => new(-1, 0);
    public static Coordinate2D SouthEast => new(1, -1);
    public static Coordinate2D SouthWest => new(-1, -1);
    public static Coordinate2D NorthEast => new(1, 1);
    public static Coordinate2D NorthWest => new(-1, 1);
}

public record Grid(int XSize, int YSize)
{
    public IEnumerable<Coordinate2D> GetNeighbours(Coordinate2D centre, bool includeDiagonals)
    {
        var directions = new[]
        {
            Coordinate2D.North,
            Coordinate2D.South,
            Coordinate2D.East,
            Coordinate2D.West,

        };

        if (includeDiagonals)
        {
            directions = directions.Concat(new[]
                {
                    Coordinate2D.NorthEast,
                    Coordinate2D.NorthWest,
                    Coordinate2D.SouthEast,
                    Coordinate2D.SouthWest,
                }
            ).ToArray();
        }

        return directions.Select(centre.Add)
            .Where(it => (it.X >= 0 && it.X < XSize)
                         && (it.Y >= 0 && it.Y < YSize));
    }
}