namespace AdventOfCode.Solutions.Year2022.Day04;

internal class Solution : SolutionBase
{
    private readonly List<int[][]> _parsedInput;

    public Solution() : base(04, 2022, "Camp Cleanup")
    {
        _parsedInput = Input.SplitByNewline()
                            .Select(x => x.Split(",")
                                                .Select(y => y.Split("-")
                                                                    .Select(int.Parse)
                                                                    .ToArray()
                                                       )
                                                .ToArray()
                                   )
                            .ToList();
    }

    protected override string SolvePartOne()
    {
        var count = 0;
        foreach (var pairs in _parsedInput)
            if ((pairs[0][0] >= pairs[1][0] && pairs[0][1] <= pairs[1][1]) || (pairs[0][0] <= pairs[1][0] && pairs[0][1] >= pairs[1][1])) 
                count++;

        return count.ToString();
    }

    protected override string SolvePartTwo()
    {
        var count = 0;
        foreach (var pairs in _parsedInput)
            if (pairs[0][0] <= pairs[1][1] && pairs[0][1] >= pairs[1][0]) 
                count++;
        
        return count.ToString();
    }
}
