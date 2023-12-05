namespace AdventOfCode.Solutions.Year2023.Day05;

internal class Solution : SolutionBase
{
    private readonly List<long> _seeds;
    private List<Range> _seedRanges;
    private readonly List<List<MapEntry>> _maps;

    private sealed record Range(long From, long To)
    {
        public long From { get; set; } = From;
    }

    private sealed record MapEntry(long From, long To, long Adjustment);

    public Solution() : base(05, 2023, "If You Give A Seed A Fertilizer")
    {
        string[] inputByParagraph = this.Input.SplitByParagraph(true);

        this._seeds = ParseSeeds(inputByParagraph[0]);
        this._maps = ParseMaps(inputByParagraph.Skip(1));
        this._seedRanges = ParseSeedRanges(this._seeds);
    }

    private static List<long> ParseSeeds(string seedParagraph)
    {
        return seedParagraph
            .Split(":", StringSplitOptions.RemoveEmptyEntries)[1]
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(long.Parse)
            .ToList();
    }

    private static List<List<MapEntry>> ParseMaps(IEnumerable<string> mapParagraphs)
    {
        return mapParagraphs
            .Select(mapParagraph => mapParagraph.SplitByNewline(true)
            .Skip(1)
            .Select(mapLine =>
             {
                 string[] parts = mapLine.Split(' ');

                 long destinationRangeStart = long.Parse(parts[0]);
                 long sourceRangeStart = long.Parse(parts[1]);
                 long rangeLength = long.Parse(parts[2]);

                 return new MapEntry(
                     From: sourceRangeStart,
                     To: sourceRangeStart + rangeLength - 1,
                     Adjustment: destinationRangeStart - sourceRangeStart
                 );
             }).ToList()
            ).ToList();
    }

    private static List<Range> ParseSeedRanges(IReadOnlyList<long> seeds)
    {
        var result = new List<Range>();

        if (seeds is null || seeds.Count == 0)
            throw new ArgumentNullException();

        for (int i = 0; i < seeds.Count; i += 2)
            result.Add(new Range(From: seeds[i], To: seeds[i] + seeds[i + 1] - 1));

        return result;
    }

    protected override string SolvePartOne()
    {
        return this._seeds.Select(seed => 
            this._maps.Aggregate(seed, (currentValue, map) =>
                map.FirstOrDefault(item => 
                    currentValue >= item.From && currentValue <= item.To) is var conversion && conversion != null ? currentValue + conversion.Adjustment : currentValue))
            .Min()
            .ToString();
    }

    protected override string SolvePartTwo()
    {
        this._maps.Select(map => map.OrderBy(x => x.From).ToList()).ForEach(map =>
        {
            this._seedRanges = this._seedRanges.SelectMany(range =>
            {
                var result = new List<Range>();

                foreach (var mapping in map)
                {
                    if (range.From < mapping.From)
                    {
                        result.Add(range with { To = Math.Min(range.To, mapping.From - 1) });

                        range.From = mapping.From;

                        if (range.From > range.To)
                            break;
                    }

                    if (range.From > mapping.To)
                        continue;

                    result.Add(new Range(range.From + mapping.Adjustment, Math.Min(range.To, mapping.To) + mapping.Adjustment));

                    range.From = mapping.To + 1;

                    if (range.From > range.To)
                        break;
                }

                if (range.From <= range.To)
                    result.Add(range);

                return result;

            }).ToList();
        });

        return this._seedRanges
            .Min(r => r.From)
            .ToString();
    }
}