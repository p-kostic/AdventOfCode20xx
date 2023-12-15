using System.Buffers;

namespace AdventOfCode.Solutions.Year2023.Day15;

internal class Solution : SolutionBase
{
    private record Lens(string? Label, int? Power);
    private record Box(List<Lens> Lenses);

    private readonly string[] _input;
    private readonly SearchValues<char> _operationCodeBuffer;

    public Solution() : base(15, 2023, "Lens Library")
    {
        // For some reason there's a \n at the end within this.Input using SolutionBase.LoadInput
        this._input = this.Input.TrimEnd('\n').Split(',');
        this._operationCodeBuffer = SearchValues.Create("-=");
    }

    protected override string SolvePartOne() => this._input.Sum(Hash).ToString();
    
    private static int Hash(string toHash)
    {
        int currentValue = 0;
        foreach (char c in toHash)
        {
            currentValue += c;
            currentValue *= 17;
            currentValue %= 256;
        }

        return currentValue;
    }

    protected override string SolvePartTwo()
    {
        var boxes = new List<Box>(256);
        for (int i = 0; i < 256; i++)
            boxes.Add(new Box(new List<Lens>()));

        foreach (string s in this._input)
        {
            string lensLabel = s[..s.AsSpan().IndexOfAny(this._operationCodeBuffer)];  // ReSharper, original: string lensLabel = s.Substring(0, s.IndexOfAny("-=".ToCharArray()));
                                                                                       // + System.Buffers for increased performance.
            char operation = s.Contains('=') ? '=' : '-';             
            int lensPower = operation == '=' ? s[^1] - '0' : -1;                       // ReSharper, original: s[s.Length -1]

            int hashIndex = Hash(lensLabel);
            int index = boxes[hashIndex].Lenses.FindIndex(x => x.Label == lensLabel ); // Returns -1 if not found
            switch (operation)
            {
                case '-' when index != -1:
                    boxes[hashIndex].Lenses.RemoveAt(index);
                    break;
                case '=' when index == -1:
                    boxes[hashIndex].Lenses.Add(new Lens(lensLabel , lensPower));
                    break;
                case '=':
                    boxes[hashIndex].Lenses[index] = new Lens(lensLabel, lensPower);
                    break;
            }
        }

        int result = 0;
        for (int i = 0; i < boxes.Count; i++)
        {
            for (int j = 0; j < boxes[i].Lenses.Count; j++)
            {
                var lens = boxes[i].Lenses[j];
                result += (1 + i) * (1 + j) * lens.Power!.Value;
            }
        }

        return result.ToString();
    }
}