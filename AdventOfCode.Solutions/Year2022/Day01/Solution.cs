namespace AdventOfCode.Solutions.Year2022.Day01;

class Solution : SolutionBase
{
    private readonly Dictionary<int, int> calorieDic;

    public Solution() : base(01, 2022, "Calorie Counting") 
    {
        this.calorieDic = new Dictionary<int, int>();

        string[] splitByParagraphInput = Input.SplitByParagraph(true);

        for (int i = 0; i < splitByParagraphInput.Length; i++)
        {
            string? calorieLines = splitByParagraphInput[i];
            string[] caloriesForElf = calorieLines.SplitByNewline();

            foreach (var calorieForElf in caloriesForElf)
            {
                int calorieAsInt = int.Parse(calorieForElf);

                if (!calorieDic.ContainsKey(i))
                    this.calorieDic.Add(i, calorieAsInt);
                else 
                    this.calorieDic[i] += calorieAsInt;
            }
        }
    }

    protected override string SolvePartOne()
    {
        return calorieDic.Values.Max().ToString();
    }

    protected override string SolvePartTwo()
    {
        return calorieDic.OrderByDescending(x => x.Value).Select(x => x.Value).Take(3).Sum().ToString();
    }
}
