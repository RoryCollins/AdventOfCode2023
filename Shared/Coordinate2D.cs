namespace Shared;

public record Coordinate2D(int X, int Y)
{
    public Coordinate2D Add(Coordinate2D other)
    {
        return new Coordinate2D(X + other.X, Y + other.Y);
    }

    public static Coordinate2D Origin => new(0, 0);
}