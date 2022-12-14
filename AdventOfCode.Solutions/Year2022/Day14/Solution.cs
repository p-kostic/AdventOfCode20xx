namespace AdventOfCode.Solutions.Year2022.Day14;

internal class Solution : SolutionBase
{
    private readonly (int, int)[][] _walls;

    public Solution() : base(14, 2022, "Regolith Reservoir")
    {
        this._walls = this.Input.SplitByNewline()
                                .Select(line => line.Split(" -> "))
                                .Select(pos => pos.Select(xy => xy.Split(',').Select(int.Parse).ToArray()))
                                .Select(xys => xys.Select(xy => (xy[0], xy[1])).ToArray())
                                .ToArray();
    }

    protected override string SolvePartOne()
    {
        return GetSandArea(false).ToString();
    }

    protected override string SolvePartTwo()
    {
        return GetSandArea(true).ToString();
    }

    private int GetSandArea(bool p2)
    {
        List<(int x, int y)> wallPoss = new();
        foreach (var wall in this._walls)
        {
            for (int i = 0; i < wall.Length - 1; i++)
            {
                (int x, int y) a = wall[i];
                (int x, int y) b = wall[i + 1];
                int dx = b.x - a.x;
                int dy = b.y - a.y;

                wallPoss.Add(a);

                while (a != b)
                {
                    a = dx == 0 ? (a.x, a.y + Math.Sign(dy)) : (a.x + Math.Sign(dx), a.y);
                    wallPoss.Add(a);
                }
            }
        }
        int maxY = wallPoss.Max(p => p.y);

        // WriteCave(wallPoss);
        
        (int x, int y) cur = (500, 0);
        List<(int x, int y)> path = new() { cur };
        HashSet<(int x, int y)> sand = new();
        while (true)
        {
            (int x, int y) next = new[] { (0, 1), (-1, 1), (1, 1) }.Select(((int x, int y) d) => (cur.x + d.x, cur.y + d.y))
                                                                   .FirstOrDefault(((int x, int y) t) => !sand.Contains(t) &&
                                                                                                         !wallPoss.Contains(t) &&
                                                                                                         !(p2 && t.y >= maxY + 2));
            if (next == (0, 0)) // Default if not found.
            {
                sand.Add(cur);
                if (path.Count == 0)
                    break;

                cur = path[^1];
                path.RemoveAt(path.Count - 1);
                continue;
            }

            if (!p2 && next.y >= maxY)
                break;

            path.Add(cur);
            cur = next;
        }

        return sand.Count;
    }

    private static void WriteCave(List<(int x, int y)> wall)
    {
        for (int y = wall.Min(p => p.y); y <= wall.Max(p => p.y); y++)
        {
            for (int x = wall.Min(p => p.x); x <= wall.Max(p => p.x); x++)
                Console.Write(wall.Contains((x, y)) ? '#' : '.');
            Console.WriteLine();
        }
    }
}
