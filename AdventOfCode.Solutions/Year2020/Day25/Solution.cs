namespace AdventOfCode.Solutions.Year2020.Day25
{
    class Solution : SolutionBase
    {
        private readonly long _subject;
        private readonly long _publicKey;

        public Solution() : base(25, 2020, "Combo Breaker")
        {
            var splitLines = Input.SplitByNewline();
            this._subject = long.Parse(splitLines[0]);
            this._publicKey = long.Parse(splitLines[1]);
        }

        protected override string SolvePartOne()
        {
            var cur = 1L;
            long loopSize;
            for (var i = 1; ; i++)
            {
                cur = cur * 7 % 20201227;
                if (cur == this._publicKey)
                {
                    loopSize = i;
                    break;
                }
            }

            cur = 1;
            for (var i = 1; i <= loopSize; i++)
                cur = cur * this._subject % 20201227;
            return cur.ToString();
        }

        protected override string SolvePartTwo()
        {
            return null;
        }
    }
}
