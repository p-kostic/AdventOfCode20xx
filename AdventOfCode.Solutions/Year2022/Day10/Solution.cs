namespace AdventOfCode.Solutions.Year2022.Day10;

internal class Solution : SolutionBase
{
    private readonly (string, string) _answers;

    public Solution() : base(10, 2022, "Cathode-Ray Tube")
    {
        this._answers = CalculateSignal(this.Input.SplitByNewline(), 20);
    }

    protected override string SolvePartOne()
    {
        return this._answers.Item1;
    }

    protected override string SolvePartTwo()
    {
        return this._answers.Item2;
    }

    private static (string, string) CalculateSignal(IEnumerable<string> instructions, int thCycle)
    {
        int x = 1;
        int currentCycle = 0;
        int signalStrengthSum = 0;

        StringBuilder builder = new("\n");
        foreach (string line in instructions)
        {
            int originalX = x;
            string[] splitLine = line.Split(' ');
            x += line.StartsWith("addx") ? int.Parse(splitLine[1]) : 0;

            foreach (string _ in splitLine) // Convenient cycle of 2 when relevant
            {
                builder.Append(new[] { originalX, originalX + 1, originalX - 1 }.Contains(currentCycle % 40) ? '#' : '.'); // ReSharper's suggestion

                currentCycle++;

                if (currentCycle % 40 == 0)
                    builder.Append('\n');

                if ((currentCycle - thCycle) % 40 == 0 && currentCycle <= 220)
                    signalStrengthSum += currentCycle * originalX;
            }
        }

        return (signalStrengthSum.ToString(), builder.ToString());
    }


}
