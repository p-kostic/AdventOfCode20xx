namespace AdventOfCode.Solutions.Year2023.Day09;

internal class Solution : SolutionBase
{
    private readonly List<List<int>> _numbers;

    public Solution(): base(09, 2023, "Mirage Maintenance")
    {
        var splitLines = this.Input.SplitByNewline(true);
        this._numbers = splitLines.Select(line => line.Split(' ', StringSplitOptions.TrimEntries)
                .Select(int.Parse)
                .ToList())
            .ToList();
    }

    protected override string SolvePartOne() => Solve().ToString();
    protected override string SolvePartTwo() => Solve(true).ToString();

    private int Solve(bool partTwo = false)
    {
        var result = 0;

        foreach (var numbers in this._numbers)
        {
            var currentNumbers = numbers;
            var diffs = new List<List<int>>() { numbers };

            while (currentNumbers.Exists(x => x != 0))
            {
                currentNumbers = new List<int>();

                for (var i = 0; i < diffs[^1].Count - 1; i++)
                {
                    var diff = diffs[^1][i + 1] - diffs[^1][i];
                    currentNumbers.Add(diff);
                }

                diffs.Add(currentNumbers);
            }

            // Process differences in reverse order
            // Part 1: Add the sum of the last elements of each list
            // Part 2: Insert the difference of the first elements of each list
            for (var i = diffs.Count - 1; i > 0; i--)
            {
                if (!partTwo)
                    diffs[i - 1].Add(diffs[i - 1][^1] + diffs[i][^1]);
                else
                    diffs[i - 1].Insert(0, diffs[i - 1][0] - diffs[i][0]);
            }

            if (!partTwo)
                result += diffs[0][^1];
            else
                result += diffs[0][0];
        }

        return result;
    }
}
