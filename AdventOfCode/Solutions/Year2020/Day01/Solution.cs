namespace AdventOfCode.Solutions.Year2020
{
    class Day01 : ASolution
    {
        private readonly int[] parsedInput;

        public Day01() : base(01, 2020, "Report Repair")
        {
            string[] lines = Input.SplitByNewline();

            parsedInput = new int[lines.Length];

            for (int i = 0; i < lines.Length; i++)
                parsedInput[i] = int.Parse(lines[i]);
        }

        /// <summary>
        /// First naive solution O(N^2)
        /// </summary>
        protected override string SolvePartOne()
        {
            int part1Result = 0;

            for (int i = 0; i < parsedInput.Length - 1; i++)
                for (int j = i + 1; j < parsedInput.Length; j++)
                {
                    if (parsedInput[i] + parsedInput[j] == 2020)
                        part1Result = parsedInput[i] * parsedInput[j];
                }
            return part1Result.ToString();
        }

        /// <summary>
        /// First naive solution O(N^3)
        /// </summary>
        protected override string SolvePartTwo()
        {
            int part2Result = 0;

            for (int i = 0; i < parsedInput.Length - 2; i++)
                for (int j = i + 1; j < parsedInput.Length - 1; j++)
                    for (int z = j + 1; z < parsedInput.Length; z++)
                    {
                        if (parsedInput[i] + parsedInput[j] + parsedInput[z] == 2020)
                            part2Result = parsedInput[i] * parsedInput[j] * parsedInput[z];
                    }
                   
            return part2Result.ToString();
        }
    }
}
