namespace AdventOfCode.Solutions.Year2022.Day11;

internal class Solution : SolutionBase
{
    private Dictionary<int, Monkey> _monkeyDictionary;

    public Solution() : base(11, 2022, "Monkey in the Middle")
    {
        InitOrReset();
    }

    protected override string SolvePartOne()
    {
        foreach (int _ in Enumerable.Range(1, 20))
        {
            foreach (var kvp in this._monkeyDictionary)
            {
                while (kvp.Value.WorryLevels.TryDequeue(out long item)) // Resharper's suggestion
                {
                    kvp.Value.Inspected++;

                    long left = kvp.Value.Operations[0] == "old" ? item : long.Parse(kvp.Value.Operations[0]);
                    long right = kvp.Value.Operations[2] == "old" ? item : long.Parse(kvp.Value.Operations[2]);
                    item = kvp.Value.Operations[1] switch
                    {
                        "*" => (left * right) / 3,
                        "/" => (left / right) / 3,
                        "+" => (left + right) / 3,
                        "-" => (left - right) / 3,
                        _ => item
                    };

                    int toMonkey = item % kvp.Value.Mod == 0 ? kvp.Value.TrueIndex : kvp.Value.FalseIndex;
                    this._monkeyDictionary[toMonkey].WorryLevels.Enqueue(item);
                }
            }
        }

        return ReturnAnswer();
    }

    protected override string SolvePartTwo()
    {
        InitOrReset();

        int globalModulo = this._monkeyDictionary.Select(m => m.Value.Mod).Aggregate((m, i) => m * i); // Resharper's suggestion

        foreach (int _ in Enumerable.Range(1, 10000))
        {
            foreach (var kvp in this._monkeyDictionary)
            {
                while (kvp.Value.WorryLevels.TryDequeue(out long item)) // Resharper's suggestion
                {
                    kvp.Value.Inspected++;

                    long left = kvp.Value.Operations[0] == "old" ? item : long.Parse(kvp.Value.Operations[0]);
                    long right = kvp.Value.Operations[2] == "old" ? item : long.Parse(kvp.Value.Operations[2]);

                    item = kvp.Value.Operations[1] switch
                    {
                        "*" => (left * right) % globalModulo,
                        "/" => (left / right) % globalModulo,
                        "+" => (left + right) % globalModulo,
                        "-" => (left - right) % globalModulo,
                        _ => item
                    };

                    int toMonkey = item % kvp.Value.Mod == 0 ? kvp.Value.TrueIndex : kvp.Value.FalseIndex;
                    this._monkeyDictionary[toMonkey].WorryLevels.Enqueue(item);
                }
            }
        }

        return ReturnAnswer();
    }

    private string ReturnAnswer()
    {
        return this._monkeyDictionary.Values.OrderByDescending(monkey => monkey.Inspected)
            .Take(2)
            .Select(monkey => monkey.Inspected)
            .Aggregate((x, y) => x * y)
            .ToString();
    }


    private void InitOrReset()
    {
        this._monkeyDictionary = new Dictionary<int, Monkey>();

        foreach (string paragraphAsLine in this.Input.SplitByParagraph())
        {
            string[] paragraph = paragraphAsLine.SplitByNewline();

            var monkey = new Monkey();
            int monkeyIndex = -1;

            foreach (string line in paragraph.Select(s => s.Trim()))
            {
                switch (line)
                {
                    case { } when line.StartsWith("Monkey"):
                        monkeyIndex = (int)char.GetNumericValue(line[7]); // max 9 monkeys
                        break;
                    case { } when line.StartsWith("Starting items:"):
                        monkey.WorryLevels = new Queue<long>(line[16..].Split(',').Select(long.Parse));
                        break;
                    case { } when line.StartsWith("Operation:"):
                        monkey.Operations = line[17..].Split(' ');
                        break;
                    case { } when line.StartsWith("Test:"):
                        string[] splitDivider = line.Split(' ');
                        monkey.Mod = int.Parse(splitDivider[^1]);
                        break;
                    case { } when line.StartsWith("If true:"):
                        string[] splitTrue = line.Split(' ');
                        monkey.TrueIndex = int.Parse(splitTrue[^1]);
                        break;
                    case { } when line.StartsWith("If false:"):
                        string[] splitFalse = line.Split(' ');
                        monkey.FalseIndex = int.Parse(splitFalse[^1]);
                        break;
                    default:
                        throw new ArgumentException($"Invalid line {line}:");
                }
            }

            this._monkeyDictionary.Add(monkeyIndex, monkey);
        }
    }


    private class Monkey
    {
        public Queue<long> WorryLevels { get; set; }
        public string[] Operations { get; set; }
        public int Mod { get; set; }
        public int TrueIndex { get; set; }
        public int FalseIndex { get; set; }
        public long Inspected { get; set; }
    }

    
}
