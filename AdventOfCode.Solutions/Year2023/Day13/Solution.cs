namespace AdventOfCode.Solutions.Year2023.Day13;

internal class Solution : SolutionBase
{
    private readonly List<char[][]> _grids;

    public Solution() : base(13, 2023, "Point of Incidence")
    {
        this._grids = this.Input.SplitByParagraph(true)
            .Select(x => x.SplitByNewline()
                .Select(y => y.ToCharArray())
                .ToArray())
            .ToList();
    }

    protected override string SolvePartOne()
    {
        var result = new List<int>();

        foreach (char[][] grid in this._grids)
        {
            (int horizontal, int vertical) = GetSummary(grid, (-1, -1));
            result.Add(vertical + 1 + (horizontal + 1) * 100);
        }

        return result.Sum().ToString();
    }

    protected override string SolvePartTwo() => this._grids.Sum(GetSummaryPartTwo).ToString();

    private static int GetSummaryPartTwo(IReadOnlyList<char[]> grid)
    {
        (int horizontal, int vertical) originalSummary = GetSummary(grid, (-1, -1));

        for (int i = 0; i < grid.Count; i++)
        {
            for (int j = 0; j < grid[0].Length; j++)
            {
                char[][] gridCopy = grid.Select(x => x.ToArray()).ToArray();
                gridCopy[i][j] = grid[i][j] == '#' ? '.' : '#';

                (int horizontal, int vertical) newSummary = GetSummary(gridCopy, originalSummary);

                if (IsInArray(newSummary, originalSummary, (-1, -1)))
                    continue;

                if (newSummary.horizontal != -1)
                    return (newSummary.horizontal + 1) * 100;

                if (newSummary.vertical != -1)
                    return newSummary.vertical + 1;
            }
        }
        throw new Exception("Apparently doesn't happen, there's always a smudge!");
    }

    private static (int, int) GetSummary(IReadOnlyList<char[]> grid, (int x, int y) avoid)
    {
        int horizontal = -1;
        for (int i = 0; i < grid.Count - 1; i++)
        {
            if (i == avoid.x || !IsHorizontal(grid, i)) 
                continue;

            horizontal = i;
            break;
        }

        int vertical = -1;
        char[][] transposedGrid = Transpose(grid);
        for (int j = 0; j < grid[0].Length - 1; j++)
        {
            if (j == avoid.y || !IsHorizontal(transposedGrid, j))
                continue;

            vertical = j;
            break;
        }

        // (Horizontal line of reflection, vertical line of reflection)
        return (horizontal, vertical);
    }

    private static bool IsInArray((int, int) target, params (int, int)[] array) => Array.Exists(array, item => item == target);

    private static bool IsHorizontal(IReadOnlyList<char[]> grid, int i)
    {
        for (int j = 0; j < grid[0].Length; j++)
        {
            for (int r = 0; r < grid.Count; r++)
            {
                int rn = i * 2 + 1 - r;

                if (!(rn >= 0 && rn < grid.Count)) // Within bounds
                    continue;

                if (grid[r][j] != grid[rn][j])
                    return false;
            }
        }

        return true;
    }

    private static char[][] Transpose(IReadOnlyList<char[]> grid) =>
        Enumerable.Range(0, grid[0].Length)
            .Select(columnIndex => grid.Select(row => row[columnIndex]).ToArray())
            .ToArray();
}