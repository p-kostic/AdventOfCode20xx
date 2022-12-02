namespace AdventOfCode.Solutions.Year2022.Day02;

class Solution : SolutionBase
{
    string[] parsedInput;

    public Solution() : base(02, 2022, "Rock Paper Scissors")
    {
        parsedInput = Input.SplitByNewline();
    }

    protected override string SolvePartOne()
    {
        int totalSum = 0;

        foreach (var line in parsedInput)
        {
            string[] splitLine = line.Split(' ');

            switch (splitLine[0])
            {
                case "A": // Rock = 1
                    if (splitLine[1] == "X") totalSum += 4;      // Reponose => Rock     = Draw  = 1 + 3 
                    else if (splitLine[1] == "Y") totalSum += 8; // Response => Paper    = Win   = 2 + 6
                    else totalSum += 3;                          // Response => Scissors = Lose  = 3 + 0
                    break;
                case "B": // Paper = 2
                    if (splitLine[1] == "X") totalSum += 1;      // Response => Rock     = Lose  = 1 + 0
                    else if (splitLine[1] == "Y") totalSum += 5; // Response => Paper    = Draw  = 2 + 3  
                    else totalSum += 9;                          // Response => Scissors = Win   = 3 + 6
                    break;
                case "C": // Scissors = 3
                    if (splitLine[1] == "X") totalSum += 7;      // Response => Rock     = Win   = 1 + 6
                    else if (splitLine[1] == "Y") totalSum += 2; // Response => Paper    = Lose  = 2 + 0
                    else totalSum += 6;                          // Response => Scissors = Draw  = 3 + 3
                    break;
            }
        }

        return totalSum.ToString();
    }

    protected override string SolvePartTwo()
    {
        int totalSum = 0;
        foreach (var line in parsedInput)
        {
            string[] splitLine = line.Split(' ');

            switch (splitLine[0])
            {
                case "A": // Rock = 1
                    if (splitLine[1] == "X") totalSum += 3;       // Need to lose => Scissors = 3 + 0
                    else if (splitLine[1] == "Y") totalSum += 4;  // Need to draw => Rock     = 1 + 3
                    else totalSum += 8;                           // Need to win  => Paper    = 2 + 6
                    break;
                case "B": // Paper = 2
                    if (splitLine[1] == "X") totalSum += 1;       // Need to lose => Rock     = 1 + 0
                    else if (splitLine[1] == "Y") totalSum += 5;  // Need to draw => Paper    = 2 + 3
                    else totalSum += 9;                           // Need to win  => Scissors = 3 + 6
                    break;
                case "C": // Scissors = 3
                    if (splitLine[1] == "X") totalSum += 2;       // Need to lose => Paper    = 2 + 0
                    else if (splitLine[1] == "Y") totalSum += 6;  // Need to draw => Scissors = 3 + 3
                    else totalSum += 7;                           // Need to win  => Rock     = 1 + 6
                    break;
            }
        }

        return totalSum.ToString();
    }
}
