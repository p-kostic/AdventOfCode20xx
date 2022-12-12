namespace AdventOfCode.Solutions.Year2022.Day12;

internal class Solution : SolutionBase
{
    private readonly string[] _map;

    public Solution() : base(12, 2022, "Hill Climbing Algorithm")
    {
        this._map = this.Input.SplitByNewline();
    }

    protected override string SolvePartOne()
    {
        var queue = new Queue<((int x, int y), int cost)>();

        for (int x = 0; x < this._map.Length; x++)
            for (int y = 0; y < this._map[0].Length; y++)
                if (this._map[x][y] == 'S')
                    queue.Enqueue(((x, y), 0));

        var visited = new HashSet<(int x, int y)>();
        while (queue.TryDequeue(out var item)) // ReSharper's suggestion (again)
        {
            if (!visited.Add((item.Item1.x, item.Item1.y))) continue;

            if (this._map[item.Item1.x][item.Item1.y] == 'E')
                return item.cost.ToString();

            foreach ((int dx, int dy) dPos in new List<(int x, int y)> { (-1, 0), (1, 0), (0, -1), (0, 1) })
            {
                int dxPos = item.Item1.x + dPos.dx;
                int dyPos = item.Item1.y + dPos.dy;

                if (IsOutOfBounds(dxPos, dyPos)) continue;

                if (this._map[item.Item1.x][item.Item1.y] == 'S' ? this._map[dxPos][dyPos] - 'a' <= 1 : this._map[dxPos][dyPos] - this._map[item.Item1.x][item.Item1.y] <= 1) // ReSharper's suggestion
                    queue.Enqueue(((dxPos, dyPos), item.cost + 1));
            }
        }
        return "No path found";
    }

    protected override string SolvePartTwo()
    {
        int[,] costs = new int[this._map.Length, this._map[0].Length];

        for (int x = 0; x < this._map.Length; x++)
            for (int y = 0; y < this._map[0].Length; y++)
            {
                costs[x, y] = this._map[x][y] switch // ReSharper's suggestion
                {
                    'S' => 1,
                    'E' => 26,
                    _ => (this._map[x][y] - 'a') + 1
                };
            }

        var queue = new Queue<((int x, int y), int cost)>();

        for (int x = 0; x < this._map.Length; x++)
            for (int y = 0; y < this._map[0].Length; y++)
                if (costs[x, y] == 1)
                    queue.Enqueue(((x, y), 0));

        var seen = new HashSet<(int x, int y)>();

        while (queue.TryDequeue(out var item)) // ReSharper's suggestion (again)
        {
            if (!seen.Add((item.Item1.x, item.Item1.y))) continue;

            if (this._map[item.Item1.x][item.Item1.y] == 'E')
                return item.cost.ToString();

            foreach ((int dx, int dy) dPos in new List<(int x, int y)> { (-1, 0), (1, 0), (0, -1), (0, 1) })
            {
                int dxPos = item.Item1.x + dPos.dx;
                int dyPos = item.Item1.y + dPos.dy;

                if (IsOutOfBounds(dxPos, dyPos)) continue;

                if (costs[dxPos, dyPos] <= 1 + costs[item.Item1.x, item.Item1.y])
                    queue.Enqueue(((dxPos, dyPos), item.cost + 1));
            }
        }
        return "No path found";
    }
    private bool IsOutOfBounds(int dxPos, int dyPos)
    {
        return dxPos < 0 || dxPos >= this._map.Length || dyPos < 0 || dyPos >= this._map[0].Length;
    }
}
