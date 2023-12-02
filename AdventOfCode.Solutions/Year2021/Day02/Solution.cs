namespace AdventOfCode.Solutions.Year2021.Day02
{
    internal class Solution : SolutionBase
    {
        private readonly string[] _input;

        public Solution() : base(02, 2021, "Dive!")
        {
            this._input = this.Input.SplitByNewline();
        }

        protected override string SolvePartOne()
        {
            int horizontalPosition = 0;
            int depth = 0;

            foreach (string line in this._input)
            {
                string[] commandAndValue = line.Split(' ');
                switch (commandAndValue[0])
                {
                    case "forward":
                        horizontalPosition += int.Parse(commandAndValue[1]);
                        break;
                    case "down":
                        depth += int.Parse(commandAndValue[1]);
                        break;
                    case "up":
                        depth -= int.Parse(commandAndValue[1]);
                        break;
                }
            }

            return (horizontalPosition * depth).ToString();
        }

        protected override string SolvePartTwo()
        {
            int horizontalPosition = 0;
            int depth = 0;
            int aim = 0;

            foreach (string line in this._input)
            {
                string[] commandAndValue = line.Split(' ');
                switch (commandAndValue[0])
                {
                    case "forward":
                        depth += aim * int.Parse(commandAndValue[1]);
                        horizontalPosition += int.Parse(commandAndValue[1]);
                        break;
                    case "down":
                        aim += int.Parse(commandAndValue[1]);
                        break;
                    case "up":
                        aim -= int.Parse(commandAndValue[1]);
                        break;
                }
            }

            return (horizontalPosition * depth).ToString();
        }
    }
}
