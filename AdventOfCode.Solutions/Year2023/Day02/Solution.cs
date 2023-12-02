namespace AdventOfCode.Solutions.Year2023.Day02;

internal class Solution : SolutionBase
{
    private sealed record Game(int Id, List<Set> RevealedBags);

    private sealed record Set(int Red, int Green, int Blue);
    
    private readonly List<Game> _games;
    
    public Solution() : base(02, 2023, "Cube Conundrum")
    {
        var parsedInput = this.Input.SplitByNewline(true);
        this._games = new List<Game>();
        
        // Example:
        // Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
        foreach (var line in parsedInput)
        {
            var splitByIdentifier = line.Split(":");
            var id = int.Parse(splitByIdentifier[0].Split(" ")[1]);
            var sets = new List<Set>();
            
            var splitBySemicolon = splitByIdentifier[1].Split(";", StringSplitOptions.TrimEntries);

            foreach (var set in splitBySemicolon)
            {
                var colors = set.Split(", ");
                var red = 0;
                var green = 0;
                var blue = 0;
                
                foreach (var color in colors)
                {
                    var splitBySpace = color.Split(" ");
                    var amount = int.Parse(splitBySpace[0]);
                    var colorName = splitBySpace[1];

                    switch (colorName)
                    {
                        case "red":
                            red = amount;
                            break;
                        case "green":
                            green = amount;
                            break;
                        case "blue":
                            blue = amount;
                            break;
                    }
                }
                sets.Add(new Set(red, green, blue));
            }
            this._games.Add(new Game(id, sets));
        }
    }
    
    protected override string SolvePartOne()
    {
        return this._games
            .Where(g => g.RevealedBags.TrueForAll(gs => gs is { Red: <= 12, Green: <= 13, Blue: <= 14 }))
            .Sum(r => r.Id).ToString();
    }

    protected override string SolvePartTwo()
    {
        return this._games
            .Select(g => g.RevealedBags.Max(set => set.Red)
                         * g.RevealedBags.Max(set => set.Green)
                         * g.RevealedBags.Max(set => set.Blue))
            .Sum()
            .ToString();
    }
}