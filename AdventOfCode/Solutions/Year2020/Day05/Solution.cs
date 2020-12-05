using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2020
{
    class Day05 : ASolution
    {
        HashSet<int> parsedInput;

        /// <summary>
        /// The question's example for getting the seatID is a convoluted example of binary numbers
        /// To get the seat ID, translate each line of input to binary
        /// If we convert B to 1, F to 0, R to 1 and L to 0, and convert it to integers
        /// Example BFFFBBFRRR from the explanation (Ctrl+F it in the question description)
        /// = 1000110111
        /// = 1*2^9 + 0*2^8 + 0*2^7 + 0*2^6 + 1*2^5 + 1*2^4 + 0*2^3 + 1*2^2 + 1*2^1 + 1*2^0 
        /// =  512  +  0    +   0   +   0   +   32   +  16  +   0   +   4   +   2   +   1 
        /// = 567
        /// </summary>
        public Day05() : base(05, 2020, "Binary Boarding")
        {
            parsedInput = Input.Replace("F", "0")
                               .Replace("B", "1")
                               .Replace("L", "0")
                               .Replace("R", "1")
                               .Split("\n")
                               .Select(row => Convert.ToInt32(row, 2)) // base 2
                               .ToHashSet();
        }

        /// <summary>
        /// Refer to parsing summary: What is the highest seat ID on a boarding pass? = MAX, 
        /// </summary>
        protected override string SolvePartOne()
        {
            return parsedInput.Max().ToString();
        }

        /// <summary>
        /// The question asks for a seat ID that is not in the list of seat numbers, 
        /// but should have a value between two other included IDs: in the "middle" and not in the front or back. 
        /// We can create a number range of this and grab the 'Single' one that is not within the HashSet
        /// </summary>
        protected override string SolvePartTwo()
        {
            var min = parsedInput.Min();
            var max = parsedInput.Max();
            return Enumerable.Range(min, max - min + 1).Single(id => !parsedInput.Contains(id)).ToString();
        }
    }
}
