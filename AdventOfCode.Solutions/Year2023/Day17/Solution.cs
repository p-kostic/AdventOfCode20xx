namespace AdventOfCode.Solutions.Year2023.Day17;

internal class Solution : SolutionBase
{
    private sealed record Constraints(
        Func<State, bool> StraightMovesToChangeDirection,
        Func<State, bool> StraightMoveAmount
    );

    private enum Direction
    {
        North,
        East,
        South,
        West
    }

    private sealed record Position(int X, int Y);
    private sealed record State(Position Position, Direction Direction, int NStraightMoves);
    private readonly Dictionary<Position, int> _grid;
    private readonly Position _goal;

    public Solution() : base(17, 2023, "Clumsy Crucible")
    {
        string[] inputByLine = this.Input.SplitByNewline(true);

        this._grid = new Dictionary<Position, int>();

        int height = inputByLine.Length;
        int width = inputByLine[0].Length;

        this._goal = new Position(width - 1, height - 1);

        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                this._grid.Add(new Position(x, y), int.Parse(inputByLine[y][x].ToString()));
    }

    protected override string SolvePartOne() => FindMinimumHeatLoss(new Constraints(_ => true, state => state.NStraightMoves < 3)).ToString();

    // A possible improvement noted by a colleague: State could do without `int NStraightMoves` if you could enforce your direction to always be a turn.
    // You then only have to explore all "jumps" from a state instead of just its immediate neighbors.
    protected override string SolvePartTwo() => FindMinimumHeatLoss(new Constraints(state => state.NStraightMoves >= 4, state => state.NStraightMoves < 10)).ToString();

    private int FindMinimumHeatLoss(Constraints constraints)
    {
        var q = new PriorityQueue<State, int>(Comparer<int>.Default);

        q.Enqueue(new State(new Position(0, 0), Direction.East, 0), 0);
        q.Enqueue(new State(new Position(0, 0), Direction.South, 0), 0);

        var seen = new HashSet<State>();
        while (q.TryDequeue(out var state, out int heatLoss))
        {
            if (state.Position == this._goal && constraints.StraightMovesToChangeDirection(state))
                return heatLoss;
            
            foreach (var next in GetMoves(state, constraints))
            {
                if (!this._grid.ContainsKey(next.Position) || seen.Contains(next)) 
                    continue;

                seen.Add(next);
                q.Enqueue(next, heatLoss + this._grid[next.Position]);
            }
        }
        return -1;
    }

    private static IEnumerable<State> GetMoves(State state, Constraints constraints)
    {
        if (constraints.StraightMoveAmount(state))
        {
            yield return state with
            {
                Position = GetNextPosition(state.Position, state.Direction),
                NStraightMoves = state.NStraightMoves + 1
            };
        }

        if (!constraints.StraightMovesToChangeDirection(state)) 
            yield break;

        foreach (var dir in GetBoth90Directions(state.Direction))
            yield return new State(GetNextPosition(state.Position, dir), dir, 1);
    }

    private static Position GetNextPosition(Position position, Direction direction)
    {
        return direction switch
        {
            Direction.North => position with { Y = position.Y - 1 },
            Direction.East => position with { X = position.X + 1 },
            Direction.South => position with { Y = position.Y + 1 },
            Direction.West => position with { X = position.X - 1 },
        };
    }

    private static Direction[] GetBoth90Directions(Direction currentDirection)
    {
        return currentDirection switch
        {
            Direction.North => new[] { Direction.West, Direction.East },
            Direction.East => new[] { Direction.North, Direction.South },
            Direction.South => new[] { Direction.East, Direction.West },
            Direction.West => new[] { Direction.South, Direction.North },
            _ => throw new ArgumentOutOfRangeException(nameof(currentDirection))
        };
    }

}