namespace AdventOfCode.Solutions.Year2020.Day09
{
    internal class Solution : SolutionBase
    {
	    private readonly List<long> _input;
	    private long _noPropertyNumber = long.MinValue;

        public Solution() : base(09, 2020, "Encoding Error")
        {
            this._input = this.Input.SplitByNewline(true)
		        .Select(x => Convert.ToInt64(x))
		        .ToList();
        }

		protected override string SolvePartOne()
        {
	        for (int i = 25; i < this._input.Count; i++)
	        {
		        var previous25 = this._input.GetRange(i - 25, 25);
		        bool found = previous25.DifferentCombinations(2).Any(combination => combination.Sum() == this._input[i]);

		        if (found) 
					continue;

		        this._noPropertyNumber = this._input[i];
		        return this._noPropertyNumber.ToString();
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
	        int low = 0;
	        int high = 1;

	        for(;;)
	        {
		        var range = this._input.GetRange(low, (high - low) + 1);

		        if (range.Sum() == this._noPropertyNumber)
			        return (range.Min() + range.Max()).ToString();
		        if (range.Sum() < this._noPropertyNumber)
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
