namespace AdventOfCode.Solutions.Year2022.Day09;

internal class Solution : SolutionBase
{
    private readonly string[] _parsedInput;
    private readonly Dictionary<string, (int x, int y)> _directions;

    public Solution() : base(09, 2022, "Rope Bridge")
    {
        this._parsedInput = this.Input.SplitByNewline();
        this._directions = new Dictionary<string, (int x, int y)>
        {
            ["U"] = (0, -1),
            ["R"] = (1, 0),
            ["D"] = (0, 1),
            ["L"] = (-1, 0)
        };
    }

    protected override string SolvePartOne()
    {
        return VisitedTails(2).ToString();
    }

    protected override string SolvePartTwo()
    {
        return VisitedTails(10, true).ToString();
    }

    private int VisitedTails(int amountOfKnots, bool p2 = false)
    {
        var visited = new Dictionary<(int x, int y), bool>();
        var tails = new (int x, int y)[amountOfKnots];

        foreach (string move in this._parsedInput)
        {
            string[] splitMove = move.Split(' ');
            for (int j = 0; j < int.Parse(splitMove[1]); j++)
            {
                (int dx, int dy) = this._directions[splitMove[0]];
                tails[0].x += dx;
                tails[0].y += dy;

                for (int k = 1; k < amountOfKnots; k++)
                {
                    // Distance
                    (int x1, int y1) = tails[k - 1];
                    (int x2, int y2) = tails[k];
                    double distanceSquared = Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2);
                    if (distanceSquared <= 1)
                        continue;

                    // clamp to a range of -1 to 1
                    tails[k].x += Math.Sign(x1 - x2);
                    tails[k].y += Math.Sign(y1 - y2);
                }
                
                if (!p2) 
                    visited[tails[1]] = true;
                else
                    visited[tails[amountOfKnots - 1]] = true;
            }
        }

        return visited.Count;
    }
}
