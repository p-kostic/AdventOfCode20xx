namespace AdventOfCode.Solutions.Year2022.Day07;

class Solution : SolutionBase
{
    private readonly Dictionary<string, int> _directorySizes;

    /// <summary>
    /// Since the input is relative and 'in order', we can keep track of the current directory we're in by holding
    /// the current path we're "in" in a stack. We push when we encounter a folder, we pop when we traverse back with 'cd ..'.
    /// When encountering a file, we reverse this path step by step by taking (i + 1) in a for loop for every path segment.
    /// For each segment of the parent folder, the fileSize is added to every parent path for that folder in a dictionary.
    /// The result is a dictionary containing the correct size for all possible paths that were encountered when parsing the input.
    /// </summary>
    public Solution() : base(07, 2022, "No Space Left On Device")
    {
        this._directorySizes = new Dictionary<string, int>();

        Stack<string> dir = new();
        foreach (string line in this.Input.SplitByNewline())
        {
            string[] splitLine = line.Split(" ");

            if (line.StartsWith("$ cd "))
            {
                if (splitLine[2] == "..")
                    dir.Pop();
                else
                    dir.Push(splitLine[2]);
            }
            else if (int.TryParse(splitLine[0], out int fileSize))
            {
                for (int i = 0; i < dir.Count; i++)
                {
                    string associatedDir = string.Join("/", dir.Reverse().Take(i + 1));
                    if (!this._directorySizes.ContainsKey(associatedDir))
                        this._directorySizes.Add(associatedDir, fileSize);
                    else
                        this._directorySizes[associatedDir] += fileSize;
                }
            }
        }

    }

    protected override string SolvePartOne()
    {
        return this._directorySizes.Where(x => x.Value < 100000)
                                   .Sum(x => x.Value)
                                   .ToString();
    }

    /// <summary>
    /// Total directory size (for my input)    => 48690120
    /// Total disk available                   => 70000000
    /// Unused space (for me) before deleting  => 70000000 - 48690120 = 21309880
    /// Needed space                           => 30000000
    /// To free up (for me)                    => 30000000 - 21309880 = 8690120
    /// Order by, ascending, and take the first we encounter bigger than 8690120
    /// </summary>
    protected override string SolvePartTwo()
    {
        return this._directorySizes.OrderBy(x => x.Value)
                                   .First(x => x.Value > 30000000 - (70000000 - this._directorySizes["/"]))
                                   .Value
                                   .ToString();
    }
}