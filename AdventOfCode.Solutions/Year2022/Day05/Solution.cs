namespace AdventOfCode.Solutions.Year2022.Day05;

class Solution : SolutionBase
{
    private Dictionary<int, List<char>> _state;
    private readonly string[] _stateAsLines;
    private readonly List<int[]> _moveSet;

    public Solution() : base(05, 2022, "Supply Stacks")
    {
        string[] stateAndInstructions = this.Input.SplitByParagraph();
        
        // Parse State, supports variable input.
        this._stateAsLines = stateAndInstructions[0].SplitByNewline();
        InitOrResetState(this._stateAsLines);

        // Parse moves.
        this._moveSet = new List<int[]>();
        foreach (string line in stateAndInstructions[1].SplitByNewline())
            this._moveSet.Add(GetMoves(line));
    }

    protected override string SolvePartOne()
    {
        foreach (int[] moves in this._moveSet)
        {
            for (int i = 0; i < moves[0]; i++)
                this._state[moves[2]].Insert(0, this._state[moves[1]][i]);
            this._state[moves[1]].RemoveRange(0, moves[0]);
        }
        return GetTopBoxesFromState();
    }

    protected override string SolvePartTwo()
    {
        InitOrResetState(this._stateAsLines);

        foreach (int[] moves in this._moveSet)
        {
            for (int i = moves[0] - 1; i >= 0; i--)
                this._state[moves[2]].Insert(0, this._state[moves[1]][i]);
            this._state[moves[1]].RemoveRange(0, moves[0]);
        }
        return GetTopBoxesFromState();
    }

    private string GetTopBoxesFromState()
    {
        char[] returnString = new char[9];
        foreach (var kvp in this._state)
            returnString[kvp.Key - 1] = kvp.Value[0];
        return new string(returnString);
    }

    private static int[] GetMoves(string instruction)
    {
        return instruction.Split(new[] { "move ", " from ", " to " }, StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToArray();
    }

    private void InitOrResetState(string[] stateAsLines)
    {
        if (stateAsLines == null) throw new ArgumentNullException(nameof(stateAsLines));

        this._state = new Dictionary<int, List<char>>();

        // Parse the input states and allocate the lists.
        int max = (int)stateAsLines[^1].Where(char.IsDigit).Select(char.GetNumericValue).Max();
        for (int i = 1; i <= max; i++)
            this._state.Add(i, new List<char>());

        // Fill all stacks with boxes.
        for (int index = 0; index < stateAsLines.Length - 1; index++)
        {
            string s = stateAsLines[index];
            int col = 1;

            for (int i = 0; i < s.Length; i += 4)
            {
                if (s[i + 1] != ' ')
                    this._state[col].Add(s[i + 1]);
                col++;
            }
        }
    }
}
