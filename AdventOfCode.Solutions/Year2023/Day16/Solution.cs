namespace AdventOfCode.Solutions.Year2023.Day16;

internal class Solution : SolutionBase
{
    private enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    private sealed record Position(int X, int Y);

    private sealed record Beam
    {
        public Beam(int x, int y, Direction dir)
        {
            this.Position = new Position(x, y);
            this.Direction = dir;
        }

        public Position Position { get; private set; }
        public Direction Direction { get; private set; }

        public void MoveToDirection(Direction direction, int amount = 1)
        {
            this.Direction = direction;
            this.Position = direction switch
            {
                Direction.Right => this.Position with { X = this.Position.X + amount },
                Direction.Left => this.Position with { X = this.Position.X - amount },
                Direction.Up => this.Position with { Y = this.Position.Y - amount },
                Direction.Down => this.Position with { Y = this.Position.Y + amount },
                _ => this.Position
            };
        }

        public bool WithinBounds(int maxX, int maxY) => this.Position.X >= 0 && this.Position.X <= maxX &&
                                                         this.Position.Y >= 0 && this.Position.Y <= maxY;
    }

    private readonly Dictionary<Position, char> _grid;
    private readonly int _width;
    private readonly int _height;

    public Solution() : base(16, 2023, "The Floor Will Be Lava")
    {
        this._grid = this.Input.SplitByNewline(true).SelectMany((row, y) => row.Select((c, x) => new { x, y, c })).ToDictionary(x => new Position(x.x, x.y), x => x.c);
        this._width = this._grid.Keys.Max(x => x.X);
        this._height = this._grid.Keys.Max(x => x.Y);
    }

    protected override string SolvePartOne() => Simulate(new Beam(0, 0, Direction.Right)).ToString();

    protected override string SolvePartTwo()
    {
        var results = new List<int>();

        for (int x = 0; x <= this._width; x++)
            results.Add(Simulate(new Beam(x, 0, Direction.Down)));

        for (int x = 0; x <= this._width; x++)
            results.Add(Simulate(new Beam(x, this._height, Direction.Up)));

        for (int y = 0; y <= this._height; y++)
            results.Add(Simulate(new Beam(0, y, Direction.Right)));

        for (int y = 0; y <= this._height; y++)
            results.Add(Simulate(new Beam(this._width, y, Direction.Left)));

        return results.Max().ToString();
    }

    private int Simulate(Beam start)
    {
        var beams = new List<Beam> { start };
        var visited = new HashSet<Position>();
        var cache = new HashSet<(Position, Direction)>();

        while (beams.Count != 0)
        {
            var beamsCurrentIteration = new List<Beam>();

            foreach (var beam in beams)
                visited.Add(beam.Position);

            foreach (var beam in beams)
                ProcessCharacter(this._grid[beam.Position], beam, beamsCurrentIteration);

            // Clear the beams list for the next iteration, and prevent revisiting the same beam
            beams.Clear();
            foreach (var beam in beamsCurrentIteration.Where(beam => !cache.Contains((beam.Position, beam.Direction))))
            {
                beams.Add(beam);
                cache.Add((beam.Position, beam.Direction));
            }
        }

        return visited.Count;
    }

    private void ProcessCharacter(char characterToCheck, Beam beam, ICollection<Beam> beamsCurrentIteration)
    {
        switch (characterToCheck)
        {
            case '.':
            case '-' when beam.Direction is Direction.Left or Direction.Right:
            case '|' when beam.Direction is Direction.Up or Direction.Down:
                beam.MoveToDirection(beam.Direction);
                break;
            case '\\':
                beam.MoveToDirection(beam.Direction switch
                {
                    Direction.Right => Direction.Down,
                    Direction.Up => Direction.Left,
                    Direction.Left => Direction.Up,
                    Direction.Down => Direction.Right,
                });
                break;
            case '/':
                beam.MoveToDirection(beam.Direction switch
                {
                    Direction.Right => Direction.Up,
                    Direction.Up => Direction.Right,
                    Direction.Left => Direction.Down,
                    Direction.Down => Direction.Left,
                });
                break;
            case '-':
                var newBeam = new Beam(beam.Position.X, beam.Position.Y, Direction.Left);
                beam.MoveToDirection(Direction.Right);
                newBeam.MoveToDirection(Direction.Left);

                if (newBeam.WithinBounds(this._width, this._height))
                    beamsCurrentIteration.Add(newBeam);
                break;
            case '|':
                newBeam = new Beam(beam.Position.X, beam.Position.Y, Direction.Down);
                beam.MoveToDirection(Direction.Up);
                newBeam.MoveToDirection(Direction.Down);

                if (newBeam.WithinBounds(this._width, this._height))
                    beamsCurrentIteration.Add(newBeam);
                break;
        }

        // Also consider the current beam if it's within bounds, similar to the potential new beams
        if (beam.WithinBounds(this._width, this._height))
            beamsCurrentIteration.Add(beam);
    }
}