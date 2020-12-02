using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions.Year2020
{

    class Day02 : ASolution
    {
        struct ParsedLine
        {
            public int min { get; set; }
            public int max { get; set; }
            public char letter { get; set; }
            public string password { get; set; }
        }

        List<ParsedLine> parsedInput;

        public Day02() : base(02, 2020, "Password Philosophy")
        {
            string[] lines = Input.SplitByNewline();
            parsedInput = new List<ParsedLine>();

            foreach (string line in lines)
            {
                // 2-9 c: ccccccccc
                var fsttSplit = line.Split(':'); // [0] 2-9 c, [1] _ccccccccc
                var sndSplit = fsttSplit[0].Split(' '); // [0] 2-9, [1] c
                var trdSplit = sndSplit[0].Split('-'); // [0] 2, [1] 9

                parsedInput.Add(new ParsedLine()
                {
                    min = int.Parse(trdSplit[0]),
                    max = int.Parse(trdSplit[1]),
                    letter = char.Parse(sndSplit[1]),
                    password = fsttSplit[1][1..]
                });
            }
        }

        /// <summary>
        /// O(n * l) where n is input length and l is string length
        /// </summary>
        protected override string SolvePartOne()
        {
            int validCounter = 0;
            foreach (var parsedLine in parsedInput)
            {
                var localCount = parsedLine.password.Count(x => x == parsedLine.letter);
                if (localCount >= parsedLine.min && localCount <= parsedLine.max)
                    validCounter++;
            }
            return validCounter.ToString();
        }

        /// <summary>
        /// O(n) where n is input length
        /// </summary>
        protected override string SolvePartTwo()
        {
            int validCounter = 0;
            foreach (var parsedLine in parsedInput)
            {
                bool firstValid = parsedLine.password[parsedLine.min - 1] == parsedLine.letter;
                bool secondValid = parsedLine.password[parsedLine.max - 1] == parsedLine.letter;
                if (firstValid ^ secondValid)
                    validCounter++;
            }
            return validCounter.ToString();
        }
    }
}
