using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions.Year2020
{

    class Day10 : ASolution
    {
        private readonly List<int> _input;
        private readonly int _maxJolt;

        public Day10() : base(10, 2020, "Adapter Array")
        {
            string[] splitInput = Input.SplitByNewline();
            _input = Array.ConvertAll(splitInput, s => int.Parse(s)).ToList();
            _input.Sort();

            // Joltage of 3 higher than the max)
            _maxJolt = _input.Last() + 3;
            _input.Add(_maxJolt);

            _input.Insert(0, 0);
        }

        protected override string SolvePartOne()
        {
            int oneJolt = 0;
            int threeJolts = 0;

            for (int i = 0; i < _input.Count - 1; i++)
            {
                if (_input[i] == _input[i + 1] - 1)
                    oneJolt++;
                else if (_input[i] == _input[i + 1] - 3)
                    threeJolts++;
            }
            return (threeJolts * oneJolt).ToString();
        }

        protected override string SolvePartTwo()
        {
            long[] dpArray = new long[_input.Count];
            Array.Fill(dpArray, -1);
            return CountValid(0, dpArray).ToString();
        }

        private long CountValid(int index, long[] dpArray)
        {
            if (index == _input.Count - 1)
                return 1;

            if (dpArray[index] != -1)
                return dpArray[index];
            else
            {
                long count = 0;
                for (int i = index + 1; i <= Math.Min(index + 3, _input.Count - 1); i++)
                    if (_input[index] + 3 >= _input[i])
                        count += CountValid(i, dpArray);

                dpArray[index] = count;
                return count;
            }
        }
    }
}
