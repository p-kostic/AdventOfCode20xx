namespace AdventOfCode.Solutions.Year2023.Day03;

internal class Solution : SolutionBase
{
    private sealed record Number
    {
        public int Value {get; init;}
        public (int x, int y) Start {get; init;}
        public (int x, int y) End {get; init;}
    }

    private sealed record Symbol(char Value, (int x, int y) Position);
    
    private readonly List<Number> _numbers;
    private readonly List<Symbol> _symbols;


    public Solution() : base(03, 2023, "Gear Ratios")
    {
        this._numbers = new List<Number>();
        this._symbols = new List<Symbol>();
        
        var parsedInput = Input.SplitByNewline(false);

        for (var i = 0; i < parsedInput.Length; i++)
        {
            var curr = new Number();
            var digits = new StringBuilder();

            for (var j = 0; j < parsedInput[i].Length; j++)
            {
                if (parsedInput[i][j] == '.')
                    continue;

                if (char.IsDigit(parsedInput[i][j]))
                {
                    digits.Append(parsedInput[i][j]);
                    curr = curr with { Start = (i, j) };

                    while (j < parsedInput[i].Length - 1 && char.IsDigit(parsedInput[i][j + 1]))
                    {
                        digits.Append(parsedInput[i][j + 1]);
                        j++;
                    }

                    curr = curr with
                    {
                        End = (i, j),
                        Value = int.Parse(digits.ToString())
                    };
                    _numbers.Add(curr);
                    curr = new Number();
                    digits.Clear();
                }
                else
                {
                    _symbols.Add(new Symbol(parsedInput[i][j], (i, j)));
                }
            }
        }
    }

    protected override string SolvePartOne()
    {
        return this._numbers
            .Where(number => this._symbols.Exists(symbol => IsAdjacent(number, symbol)))
            .Sum(number => number.Value)
            .ToString();
    }

    protected override string SolvePartTwo()
    {
        return this._symbols
            .Where(symbol => symbol.Value == '*')
            .Select(symbol => this._numbers.Where(number => IsAdjacent(number, symbol)).ToArray())
            .Where(gears => gears.Length == 2)
            .Sum(gears => gears[0].Value * gears[1].Value)
            .ToString();
    }
    
    private static bool IsAdjacent(Number number, Symbol symbol)
    {
        return Math.Abs(symbol.Position.x - number.Start.x) <= 1 && symbol.Position.y >= number.Start.y - 1 && symbol.Position.y <= number.End.y + 1;
    }
}