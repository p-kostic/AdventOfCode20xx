namespace AdventOfCode.Solutions.Year2022.Day03;

internal class Solution : SolutionBase
{
    private readonly List<(string, string)> _parsedInputPart1;
    private readonly string[] _parsedInputPart2;

    public Solution() : base(03, 2022, "Rucksack Reorganization")
    {
        _parsedInputPart2 = Input.SplitByNewline();
        _parsedInputPart1 = _parsedInputPart2.Select(x => (x[..(x.Length / 2)], x.Substring(x.Length / 2, x.Length / 2))).ToList();
    }

    // Not abusing LINQ for once
    protected override string SolvePartOne()
    {
        var prioritySum = 0;
        foreach (var line in _parsedInputPart1)
        {
            List<char> seen = new();
            foreach (var c in line.Item1.Where(c => line.Item2.Contains(c) && !seen.Contains(c)))
            {
                prioritySum += char.IsUpper(c) ? c - 38 : c - 96;
                seen.Add(c);
            }
        }

        return prioritySum.ToString();
    }

    protected override string SolvePartTwo()
    {
        return _parsedInputPart2.Chunk(3)
                                .Select(x => x[0].Intersect(x[1]).Intersect(x[2]).First())
                                .Select(c => char.IsUpper(c) ? c - 38 : c - 96)
                                .Sum()
                                .ToString();
    }
}
