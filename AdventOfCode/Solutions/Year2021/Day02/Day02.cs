using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Solutions.Year2021
{

    class Day02 : ASolution
    {

        private readonly string[] _input;

        public Day02() : base(02, 2021, "Dive!")
        {
            _input = Input.SplitByNewline();
        }

        protected override string SolvePartOne()
        {
            int horizontalPosition = 0;
            int depth = 0;

            foreach (var line in _input)
            {
                var commandAndValue = line.Split(' ');
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

            foreach (var line in _input)
            {
                var commandAndValue = line.Split(' ');
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
