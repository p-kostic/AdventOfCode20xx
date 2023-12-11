namespace AdventOfCode.Solutions.Year2023.Day11;

internal class Solution : SolutionBase
{
    private sealed record Galaxy(int X, int Y)
    {
        public long Distance(Galaxy other, IEnumerable<int> dX, IEnumerable<int> dY, bool partTwo = false)
        {
            (int xHigh, int yHigh) = (Math.Max(this.X, other.X), Math.Max(this.Y, other.Y));
            (int xLow, int yLow) = (Math.Min(this.X, other.X), Math.Min(this.Y, other.Y));
            return xHigh - xLow + yHigh - yLow
                + dX.Count(x => x > xLow && x < xHigh) * (partTwo ? 999999L : 1)
                + dY.Count(y => y > yLow && y < yHigh) * (partTwo ? 999999L : 1);
        }
    }

    private readonly List<Galaxy> _galaxies;
    private readonly List<int> _dX;
    private readonly List<int> _dY;

    public Solution() : base(11, 2023, "Cosmic Expansion")
    {
        string[] inputLines = this.Input.SplitByNewline(true);

        this._dX = new List<int>();
        this._dY = new List<int>();
        this._galaxies = new List<Galaxy>();

        // Any rows or columns that contain no galaxies should all actually be twice as big.
        for (int i = 0; i < inputLines.Length; i++)
        {
            if (!inputLines[i].Contains('#'))
            {
                this._dY.Add(i);
                if (i != 0)
                    continue;
            }

            for (int j = 0; j < inputLines[i].Length; j++)
            {
                if (i == 0)
                {
                    var col = Enumerable.Range(0, inputLines.Length).Select(x => inputLines[x][j]).ToList();
                    if (!col.Contains('#'))
                    {
                        this._dX.Add(j);
                        continue;
                    }
                        
                }
                if (inputLines[i][j] == '#')
                    this._galaxies.Add(new Galaxy(j, i));
            }
        }
    }

    protected override string SolvePartOne() => this._galaxies.SelectMany((g1, i) => this._galaxies.Skip(i + 1).Select(g2 => g1.Distance(g2, this._dX, this._dY)))
                                                              .Sum()
                                                              .ToString();

    protected override string SolvePartTwo() => this._galaxies.SelectMany((g1, i) => this._galaxies.Skip(i + 1).Select(g2 => g1.Distance(g2, this._dX, this._dY, true)))
                                                              .Sum()
                                                              .ToString();

}