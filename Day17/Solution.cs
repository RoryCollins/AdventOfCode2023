namespace Day17;

using Shared;
using static Shared.Direction;

public class Solution
{
    private readonly record struct State(Coordinate2D Coordinate, Direction Direction, int Count);

    private readonly Grid grid;

    public Solution(List<string> input)
    {
        grid = new Grid(input);
    }

    public object PartOne()
    {
        return Solve(0, 3);
    }

    public object PartTwo()
    {
        return Solve(4, 10);
    }

    private int Solve(int min, int max)
    {
        PriorityQueue<State, int> nodes = new();

        var start = grid.TopLeft();
        var initialEastState = new State(start, East, 0);
        var initialSouthState = new State(start, South, 0);

        var costs = new Dictionary<State, int> { { initialEastState, 0 }, { initialSouthState, 0 } };
        nodes.Enqueue(initialEastState, 0);
        nodes.Enqueue(initialSouthState, 0);
        while (nodes.Count > 0)
        {
            var node = nodes.Dequeue();
            if (node.Coordinate == grid.BottomRight())
            {
                return costs[node];
            }

            var neighbours = GetNeighbours(node, min, max);
            var currentCost = costs.GetValueOrDefault(node, int.MaxValue / 2);
            foreach (var neighbour in neighbours)
            {
                var newCost = currentCost + grid.At(neighbour.Coordinate) - '0';

                if (newCost < costs.GetValueOrDefault(neighbour, int.MaxValue / 2))
                {
                    costs[neighbour] = newCost;
                    nodes.Enqueue(neighbour, newCost);
                }
            }
        }

        throw new NoSolutionFoundException();
    }

    private IEnumerable<State> GetNeighbours(State current, int min, int max)
    {
        var neighbours = new List<State>();
        if (current.Count >= min)
        {
            neighbours.AddRange(new[] { TurnRight, TurnLeft }
                .Select(f => f(current)));
        }

        if (current.Count < max)
        {
            neighbours.Add(GoStraight(current));
        }

        return neighbours.Where(it => grid.IsOnGrid(it.Coordinate));
    }

    private static State GoStraight(State current) => current with { Coordinate = current.Coordinate.Move(current.Direction), Count = current.Count + 1 };

    private static State TurnRight(State current)
    {
        var newDirection = current.Direction switch
        {
            North => East,
            East => South,
            South => West,
            West => North,
        };

        return new State(current.Coordinate.Move(newDirection), newDirection, 1);
    }

    private static State TurnLeft(State current)
    {
        var newDirection = current.Direction switch
        {
            North => West,
            West => South,
            South => East,
            East => North,
        };

        return new State(current.Coordinate.Move(newDirection), newDirection, 1);
    }
}