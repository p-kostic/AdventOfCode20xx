namespace AdventOfCode.Solutions.Year2020.Day01
{
    class Solution : SolutionBase
    {
        private readonly int[] parsedInput;

        public Solution() : base(01, 2020, "Report Repair")
        {
            string[] lines = this.Input.SplitByNewline();

            this.parsedInput = new int[lines.Length];

            for (int i = 0; i < lines.Length; i++)
                this.parsedInput[i] = int.Parse(lines[i]);
        }

        /// <summary>
        /// First naive solution O(N^2)
        /// </summary>
        protected override string SolvePartOne()
        {
            int part1Result = 0;

            for (int i = 0; i < this.parsedInput.Length - 1; i++)
                for (int j = i + 1; j < this.parsedInput.Length; j++)
                {
                    if (this.parsedInput[i] + this.parsedInput[j] == 2020)
                        part1Result = this.parsedInput[i] * this.parsedInput[j];
                }
            return part1Result.ToString();
        }

        /// <summary>
        /// First naive solution O(N^3)
        /// </summary>
        protected override string SolvePartTwo()
        {
            int part2Result = 0;

            for (int i = 0; i < this.parsedInput.Length - 2; i++)
                for (int j = i + 1; j < this.parsedInput.Length - 1; j++)
                    for (int z = j + 1; z < this.parsedInput.Length; z++)
                    {
                        if (this.parsedInput[i] + this.parsedInput[j] + this.parsedInput[z] == 2020)
                            part2Result = this.parsedInput[i] * this.parsedInput[j] * this.parsedInput[z];
                    }
                   
            return part2Result.ToString();
        }
    }
}
