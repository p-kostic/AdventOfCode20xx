namespace AdventOfCode.Solutions.Year2022.Day06;

class Solution : SolutionBase
{
    public Solution() : base(06, 2022, "Tuning Trouble") { }

    protected override string SolvePartOne()
    {
        return SubsequentDistinct(4);
    }

    protected override string SolvePartTwo()
    {
        return SubsequentDistinct(14);
    }

    private string SubsequentDistinct(int length)
    {
        for (int i = 0; i < this.Input.Length; i++)
            if (this.Input.Skip(i).Take(length).Distinct().Count() == length)
                return (i + length).ToString();
        return "no answer";
    }
}
