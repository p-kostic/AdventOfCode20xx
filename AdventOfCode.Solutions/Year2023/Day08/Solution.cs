using System.Collections.Immutable;

namespace AdventOfCode.Solutions.Year2023.Day08;

internal class Solution : SolutionBase
{
    private readonly string _instructions;
    private readonly ImmutableDictionary<string, (string, string)> _network;
    public Solution() : base(08, 2023, "Haunted Wasteland")
    {
        var parsedLines = this.Input.SplitByParagraph(true);
        this._instructions = parsedLines[0];

        this._network = parsedLines[1].SplitByNewline(true)
            .Select(line => line.Split('=', StringSplitOptions.TrimEntries))
            .Select(parts =>
            {
                var key = parts[0];
                var values = parts[^1].Trim('(', ')').Split(',').Select(s => s.Trim()).ToArray();
                return (key, (values[0], values[^1]));
            })
            .ToImmutableDictionary(entry => entry.key, entry => entry.Item2);
    }
    
    protected override string SolvePartOne()
    {
        var currentKey = "AAA";
        var steps = 0;

        while (currentKey != "ZZZ")
        {
            var instruction = this._instructions[steps % this._instructions.Length];

            switch (instruction)
            {
                case 'L':
                    currentKey = this._network[currentKey].Item1;
                    steps++;
                    break;
                case 'R':
                    currentKey = this._network[currentKey].Item2;
                    steps++;
                    break;
                default:
                    throw new InvalidOperationException("Invalid instruction");
            }
        }

        return steps.ToString();
    }

    protected override string SolvePartTwo()
    {
        var keysThatEndWithA = this._network.Keys.Where(key => key.EndsWith('A'))
            .ToList();

        var paths = new List<long>();

        foreach (var node in keysThatEndWithA)
        {
            var steps = 0;
            var key = node;
            while (!key.EndsWith('Z'))
            {
                var instruction = this._instructions[steps % this._instructions.Length];
                switch (instruction)
                {
                    case 'L':
                        key = this._network[key].Item1;
                        steps++;
                        break;
                    case 'R':
                        key = this._network[key].Item2;
                        steps++;
                        break;
                    default:
                        throw new InvalidOperationException("Invalid instruction");
                }
            }

            paths.Add(steps);
        }
        
        return FindLcm(paths).ToString();
    }
    
    private static long FindLcm(IEnumerable<long> numbers) =>
        numbers.Aggregate((long)1, (current, number) => current / FindGcd(current, number) * number);
    
    private static long FindGcd(long a, long b)
    {
        while (b != 0)
        {
            a %= b;
            (a, b) = (b, a);
        }
        return a;
    }
}
