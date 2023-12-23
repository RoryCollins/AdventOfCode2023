namespace Shared;

using System.ComponentModel;

public enum Direction
{
    North,
    West,
    South,
    East
}

public record Coordinate2D(int X, int Y)
{
    public Coordinate2D Add(Coordinate2D other)
    {
        return new Coordinate2D(X + other.X, Y + other.Y);
    }

    public static Coordinate2D operator *(Coordinate2D c, int x) => new(c.X * x, c.Y * x);

    public Coordinate2D Move(Direction direction)
    {
        var d =  direction switch
        {
            Direction.North => North,
            Direction.West => West,
            Direction.South => South,
            Direction.East => East,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
        return Add(d);
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

    public static Coordinate2D FromDirection(Direction direction)
    {
        return direction switch
        {
            Direction.North => North,
            Direction.South => South,
            Direction.West => West,
            Direction.East => East,
            _ => throw new ProgrammerMistake()
        };
    }
}