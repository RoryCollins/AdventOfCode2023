namespace Shared;

public class Grid
{
    public int Width;
    public int Height;
    private readonly List<string> contents;

    public Grid(List<string> input)
    {
        Height = input.Count;
        Width = input[0].Length;
        contents = input;
        contents.Reverse();
    }

    public Coordinate2D TopLeft() => new(0, Height - 1);
    public Coordinate2D BottomRight() => new(Width-1, 0);

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

        return directions
            .Select(centre.Add)
            .Where(IsOnGrid);
    }

    public bool IsOnGrid(Coordinate2D coordinate)
    {
        return (coordinate.X >= 0 && coordinate.X < Width)
               && (coordinate.Y >= 0 && coordinate.Y < Height);
    }

    public char At(Coordinate2D coordinate)
    {
        return contents[coordinate.Y][coordinate.X];
    }
}