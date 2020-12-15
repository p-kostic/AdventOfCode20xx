using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2020
{
    internal class Day15 : ASolution
    {
        private readonly int[] _parsedInput;

        public Day15() : base(15, 2020, "Rambunctious Recitation")
        {
            _parsedInput = Input.Split(',')
                                .Select(int.Parse)
                                .ToArray();
        }

        protected override string SolvePartOne() => SolveGame(2020).ToString();
        protected override string SolvePartTwo() => SolveGame(30000000).ToString();

        private int SolveGame(int target)
        {
            Dictionary<int, (int num, int index)> spoken = _parsedInput.Select((number, index) => (number, index))
                                                                       .ToDictionary(x => x.number, x => (-1, x.index));
            var last = spoken.Last().Key;

            for (var i = _parsedInput.Length; i < target; i++)
            {
                if (spoken.TryGetValue(last, out var lastIndex))
                    last = lastIndex.num != -1 ? lastIndex.index - lastIndex.num : 0;

                spoken[last] = spoken.ContainsKey(last) ? (spoken[last].index, i) : (-1, i);
            }
            return last;
        }
    }
}
