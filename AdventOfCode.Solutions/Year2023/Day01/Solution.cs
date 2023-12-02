namespace AdventOfCode.Solutions.Year2023.Day01;

internal class Solution : SolutionBase
{
    private string[] _parsedByNewline = null!;
    private readonly Dictionary<string, int> _replacements = new()
    {
        { "one", 1 },
        { "two", 2 },
        { "three", 3 },
        { "four", 4 },
        { "five", 5 },
        { "six", 6 },
        { "seven", 7 },
        { "eight", 8 },
        { "nine", 9 }
    };

    public Solution() : base(01, 2023, "Trebuchet?!") { }

    /// <summary>
    /// Reverse input strategy, get the first occurring digits using FirstOrDefault.
    /// </summary>
    protected override string SolvePartOne()
    {
        var result = new List<int>();

        this._parsedByNewline = this.Input.SplitByNewline();

        foreach (string line in this._parsedByNewline)
        {
            if (!line.Any(char.IsNumber))
                continue;

            int firstDigit = int.Parse(line.FirstOrDefault(char.IsNumber).ToString());
            string reversedLine = new(line.Reverse());
            int lastDigit = int.Parse(reversedLine.FirstOrDefault(char.IsNumber).ToString());

            result.Add(int.Parse(firstDigit + lastDigit.ToString()));
        }
        return result.Sum().ToString();
    }


    /// <summary>
    /// For each line in the input, keep track of all digits
    /// Go through each character in order. Is it a digit "normal" digit? Add it to the list
    /// Is it any of the spelled out digits? Add that to the list too. This way, you have all combinations in-order of occurrence.
    /// Finally, get the first and last digit from the list and add it to the result list, in order to sum it eventually.
    /// </summary>
    protected override string SolvePartTwo()
    {
        var result = new List<int>();

        foreach(string line in  this._parsedByNewline)
        {
            var digitsPerLine = new List<int>();

            for (int i = 0; i < line.Length; i++)
            {
                if (char.IsNumber(line[i]))
                {
                    digitsPerLine.Add(int.Parse(line[i].ToString()));
                    continue;
                }

                foreach ((string spelled, int digit) in this._replacements)
                {
                    if (i + spelled.Length - 1 >= line.Length || line[i..(i + spelled.Length)] != spelled) 
                        continue;

                    digitsPerLine.Add(digit);
                    break;
                }
            }
            result.Add(digitsPerLine.First() * 10 + digitsPerLine.Last());
        }
        return result.Sum().ToString();
    }
} 