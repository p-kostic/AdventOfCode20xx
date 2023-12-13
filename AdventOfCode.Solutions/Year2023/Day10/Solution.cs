namespace AdventOfCode.Solutions.Year2023.Day10;

internal class Solution : SolutionBase
{
    private sealed record Point(int X, int Y)
    {
        public static Point operator +(Point a, Point b) => new(a.X + b.X, a.Y + b.Y);
    }

    private sealed record Pipe(Point From, Point To)
    {
        public Pipe((int x1, int y1) from, (int x2, int y2) to) : this(new Point(from.x1, from.y1), new Point(to.x2, to.y2)) { }

        public Point[] ToArray => new[] { this.From, this.To };
    }

    private readonly Dictionary<char, Pipe> _pipes;

    // Members
    private readonly int _width;
    private readonly int _height;
    private readonly Point _start;
    private readonly char[][] _map;
    private readonly char[][] _mapCopy;
    private readonly HashSet<Point> _visited;

    public Solution() : base(10, 2023, "Pipe Maze")
    {
        this._map = this.Input.SplitByNewline().Select(s => s.ToCharArray()).ToArray();
        this._mapCopy = this._map.Select(s => s.ToArray()).ToArray();
        this._width = this._map[0].Length;
        this._height = this._map.Length;
        this._visited = new HashSet<Point>();
        this._pipes = new Dictionary<char, Pipe>
        {
            { '|', new Pipe((0,  1), ( 0, -1)) },
            { '-', new Pipe((1,  0), (-1,  0)) },
            { 'L', new Pipe((0, -1), ( 1,  0)) },
            { 'J', new Pipe((0, -1), (-1,  0)) },
            { '7', new Pipe((0,  1), (-1,  0)) },
            { 'F', new Pipe((0,  1), ( 1,  0)) }
        };

        for (int i = 0; i < this._height; i++)
            for (int j = 0; j < this._width; j++)
                if (this._map[i][j] == 'S')
                {
                    this._start = new Point(j, i);
                    break;
                }
    }

    protected override string SolvePartOne()
    {
        var currentPosition = this._start;
        int steps = 0;
        while (true)
        {
            char currentChar = this._map[currentPosition.Y][currentPosition.X];
            this._visited.Add(currentPosition);

            var surroundings = this._pipes.GetValueOrDefault(currentChar)?.ToArray.Select(c => currentPosition + c) ?? new[]
            {
                currentPosition with { X = currentPosition.X - 1 },
                currentPosition with { X = currentPosition.X + 1 },
                currentPosition with { Y = currentPosition.Y - 1 },
                currentPosition with { Y = currentPosition.Y + 1 }
            };

            currentPosition = surroundings.Where(IsNotOutOfBounds)
                                          .FirstOrDefault(c => IsValidNext(currentPosition, this._map[c.Y][c.X], c));
            steps++;

            if (currentPosition == null)
                break;
        }

        return (steps / 2).ToString();
    }

    protected override string SolvePartTwo()
    {
        int result = 0;

        for (int i = 0; i < this._height; i++)
        {
            for (int j = 0; j < this._width; j++)
            {
                if (this._visited.Contains(new Point(j, i)))
                    continue;

                // Uses https://en.wikipedia.org/wiki/Even%E2%80%93odd_rule
                if (CountLeftConnections(i, j) % 2 == 1)
                    result++;
            }
        }

        return result.ToString();
    }

    private int CountLeftConnections(int row, int col)
    {
        if (col == 0)
            return 0;

        var allowedValues = new HashSet<char> { '|', 'J', 'L' };
        return Enumerable.Range(0, col).Count(k => allowedValues.Contains(this._mapCopy[row][k]) && this._visited.Contains(new Point(k, row)));
    }

    private bool IsValidNext(Point currentLocation, char c, Point to)
    {
        if (c is '.' or 'S')
            return false;

        if (this._visited.Contains(to))
            return false;

        var connection = this._pipes[c];

        return to + connection.From == currentLocation || to + connection.To == currentLocation;
    }

    private bool IsNotOutOfBounds(Point c)
    {
        return c is { X: >= 0, Y: >= 0 } && c.X < this._width && c.Y < this._height;
    }
}