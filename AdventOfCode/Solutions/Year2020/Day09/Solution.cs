using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2020
{
    class Day09 : ASolution
    {
	    private readonly List<long> _input;
	    private long _noPropertyNumber = long.MinValue;

        public Day09() : base(09, 2020, "Encoding Error")
        {
	        _input = Input.Split('\n')
		        .Select(x => Convert.ToInt64(x))
		        .ToList();
        }

		protected override string SolvePartOne()
        {
	        for (var i = 25; i < _input.Count; i++)
	        {
		        var previous25 = _input.GetRange(i - 25, 25);
		        var found = previous25.DifferentCombinations(2).Any(combination => combination.Sum() == _input[i]);

		        if (found) 
					continue;

		        _noPropertyNumber = _input[i];
		        return _noPropertyNumber.ToString();
	        }
	        return null;
        }

		/// <summary>
		/// find a contiguous set of at least two numbers in your list which
		/// sum to the invalid number from step 1 == _noPropertyNumber
		///
		/// Go over an ever-increasing range and if the sum equals the propertryNumber, add min + max of that range
		/// </summary>
		protected override string SolvePartTwo()
        {
	        var low = 0;
	        var high = 1;

	        for(;;)
	        {
		        var range = _input.GetRange(low, (high - low) + 1);

		        if (range.Sum() == _noPropertyNumber)
			        return (range.Min() + range.Max()).ToString();
		        if (range.Sum() < _noPropertyNumber)
			        high++;
		        else
		        {
			        low++;
			        high = low + 1;
		        }
	        }
        }
    }
}
