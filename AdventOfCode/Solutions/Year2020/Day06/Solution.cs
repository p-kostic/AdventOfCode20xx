using System;
using System.Linq;

namespace AdventOfCode.Solutions.Year2020
{
	class Day06 : ASolution
    {
	    private readonly string[][] _parsedInput;

        public Day06() : base(06, 2020, "Custom Customs")
        {
	        _parsedInput = Input.Split(new[] { "\n\n" }, StringSplitOptions.RemoveEmptyEntries)
									  .Select(x => x.Split('\n'))
									  .ToArray();
        }

        /// <summary>
        /// For each group, count the number of questions to which anyone answered "yes"
        /// flatten jagged array from input and count the distincts, sum those
        /// </summary>
        protected override string SolvePartOne()
        {
	        return _parsedInput.Sum(x => x.SelectMany(y => y).Distinct().Count()).ToString();
        }

        /// <summary>
        /// For each group, count the number of questions to which everyone answered "yes". What is the sum of those counts?
        /// Count the number of elements for all that occur multiple times in the group, sum those
        /// </summary>
        protected override string SolvePartTwo()
        {
	        return _parsedInput.Sum(x => x.First().Count(y => x.All(z => z.Contains(y)))).ToString();
        }
    }
}
