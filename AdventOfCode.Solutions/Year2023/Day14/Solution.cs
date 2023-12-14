namespace AdventOfCode.Solutions.Year2023.Day14;

internal class Solution : SolutionBase
{
    private sealed record Position(int X, int Y)
    {
        public Position Move(Dir dir) => dir switch
        {
            Dir.Up => new Position(this.X, this.Y - 1),
            Dir.Down => new Position(this.X, this.Y + 1),
            Dir.Left => new Position(this.X - 1, this.Y),
            Dir.Right => new Position(this.X + 1, this.Y),
            _ => throw new NotImplementedException(),
        };
    }

    private enum Dir { Up, Down, Left, Right }

    private readonly string[] _grid;
    private HashSet<Position> _rocks;
    private HashSet<Position> _walls;
    private readonly Position _bounds;

    public Solution() : base(14, 2023, "Parabolic Reflector Dish")
    {
        this._grid = this.Input.SplitByNewline();
        this._bounds = new Position(this._grid[0].Length - 1, this._grid.Length - 1);
    }

    private void Initialize()
    {
        this._rocks = new HashSet<Position>();
        this._walls = new HashSet<Position>();

        for (int row = 0; row < this._grid.Length; row++)
        for (int column = 0; column < this._grid[row].Length; column++)
        {
            switch (this._grid[row][column])
            {
                case 'O':
                    this._rocks.Add(new Position(column, row));
                    break;
                case '#':
                    this._walls.Add(new Position(column, row));
                    break;
            }
        }
    }

    protected override string SolvePartOne()
    {
        Initialize();
        var bounds = new Position(this._grid[0].Length - 1, this._grid.Length - 1);

        for (int column = 0; column < this._grid[0].Length; column++)
        {
            for (int row = this._grid.Length - 1; row >= 0; row--)
            {
                var pos = new Position(column, row);
                if (this._rocks.Contains(pos))
                    ProcessRock(pos, Dir.Up);
            }
        }

        return this._rocks.Sum(rock => this._grid.Length - rock.Y).ToString();
    }

    protected override string SolvePartTwo()
    {
        Initialize();
        var visited = new Dictionary<string, int>();
        bool toEnd = false;

        for (int i = 0; i < 1000000000; i++)
        {
            ProcessRocksInDirection(Dir.Up);
            ProcessRocksInDirection(Dir.Left);
            ProcessRocksInDirection(Dir.Down);
            ProcessRocksInDirection(Dir.Right);

            if (toEnd)
                continue;

            string result = string.Join(',', this._rocks.OrderBy(r => r.Y).ThenBy(r => r.X));
            if (visited.TryGetValue(result, out int value))
            {
                int cycleLength = i - value;
                int remaining = 1000000000 - i;
                int cycles = remaining / cycleLength;
                i += cycles * cycleLength;
                toEnd = true;
            }
            else
                visited.Add(result, i);
        }

        return this._rocks.Sum(rock => this._grid.Length - rock.Y).ToString();
    }

    private void ProcessRocksInDirection(Dir dir)
    {
        int startX = 0;
        int endX = this._bounds.X;
        int stepX = 1;

        int startY = 0;
        int endY = this._bounds.Y;
        int stepY = 1;

        if (dir is Dir.Up or Dir.Left)
        {
            startX = this._bounds.X;
            endX = 0;
            stepX = -1;
        }

        if (dir is Dir.Up or Dir.Right)
        {
            startY = this._bounds.Y;
            endY = 0;
            stepY = -1;
        }

        for (int x = startX; x != endX + stepX; x += stepX)
        {
            for (int y = startY; y != endY + stepY; y += stepY)
            {
                var pos = new Position(x, y);
                if (this._rocks.Contains(pos))
                    ProcessRock(pos, dir);
            }
        }
    }

    private bool ProcessRock(Position pos, Dir dir)
    {
        
        var target = pos.Move(dir);
        if (target.X < 0 || target.X > this._bounds.X || target.Y < 0 || target.Y > this._bounds.Y)
            return false;
        if (this._walls.Contains(target))
            return false;
        if (this._rocks.Contains(target) && !ProcessRock(target, dir))
            return false;

        this._rocks.Remove(pos);
        this._rocks.Add(target);
        return true;
    }

}