namespace AdventOfCode.Solutions.Year2021.Day01
{
    internal class Solution : SolutionBase
    {
        private readonly int[] parsedInput;

        public Solution() : base(01, 2021, "Sonar Sweep")
        {
            string[] lines = this.Input.SplitByNewline();
            this.parsedInput = new int[lines.Length];
            for (int i = 0; i < lines.Length; i++)
            {
                this.parsedInput[i] = int.Parse(lines[i]);
            }
        }

        protected override string SolvePartOne()
        {
            int amount = 0;
            for (int i = 1; i < this.parsedInput.Length; i++)
            {
                if (this.parsedInput[i] > this.parsedInput[i - 1])
                    amount++;
            }
            return amount.ToString();
        }

        protected override string SolvePartTwo()
        {
            int amount = 0;

            for (int i = 1; i < this.parsedInput.Length - 2; i++)
            {
                int first = this.parsedInput[i - 1] + this.parsedInput[i] + this.parsedInput[i + 1];
                int second = this.parsedInput[i] + this.parsedInput[i + 1] + this.parsedInput[i + 2];

                if (second > first)
                    amount++;
            }

            return amount.ToString();
        }
    }
}
