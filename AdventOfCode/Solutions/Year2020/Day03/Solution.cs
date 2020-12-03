namespace AdventOfCode.Solutions.Year2020
{

    class Day03 : ASolution
    {
        private readonly string[] inputArray;

        public Day03() : base(03, 2020, "Toboggan Trajectory")
        {
            inputArray = Input.SplitByNewline();
        }

        /// <summary>
        /// O(N)
        /// </summary>
        protected override string SolvePartOne()
        {
            int treeCounter = 0;
            int x = 0;
            for (int y = 1; y < inputArray.Length; y++)
            {
                x += 3;
                if (x >= inputArray[y].Length)
                    x -= inputArray[y].Length;
                if (inputArray[y][x] == '#')
                    treeCounter++;
            }
            return treeCounter.ToString();
        }

        /// <summary>
        /// O(5N) == O(N)
        /// </summary>
        protected override string SolvePartTwo()
        {
            return (CountTrees(1, 1) * CountTrees(3,1) * CountTrees(5,1) * CountTrees(7,1) * CountTrees(1, 2, 2)).ToString();
        }

        private long CountTrees(int dX, int dY, int yStep = 1)
        {
            int treeCounter = 0;
            int x = 0;

            for (int y = yStep; y < inputArray.Length; y += dY)
            {
                x += dX;
                if (x >= inputArray[y].Length)
                    x -= inputArray[y].Length;
                if (inputArray[y][x] == '#')
                    treeCounter++;
            }
            return treeCounter;
        }
    }
}
