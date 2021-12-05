using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Solutions.Year2021
{

    class Day01 : ASolution
    {
        private readonly int[] parsedInput;

        public Day01() : base(01, 2021, "Sonar Sweep")
        {
            string[] lines = Input.SplitByNewline();
            parsedInput = new int[lines.Length];
            for (int i = 0; i < lines.Length; i++)
            {
                parsedInput[i] = int.Parse(lines[i]);
            }
        }

        protected override string SolvePartOne()
        {
            int amount = 0;
            for (int i = 1; i < parsedInput.Length; i++)
            {
                if (parsedInput[i] > parsedInput[i - 1])
                    amount++;
            }
            return amount.ToString();
        }

        protected override string SolvePartTwo()
        {
            int amount = 0;

            for (int i = 1; i < parsedInput.Length - 2; i++)
            {
                int first = parsedInput[i - 1] + parsedInput[i] + parsedInput[i + 1];
                int second = parsedInput[i] + parsedInput[i + 1] + parsedInput[i + 2];

                if (second > first)
                {
                    amount++;
                }
            }

            return amount.ToString();
        }
    }
}
