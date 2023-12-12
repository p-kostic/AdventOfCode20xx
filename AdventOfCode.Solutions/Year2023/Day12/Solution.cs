namespace AdventOfCode.Solutions.Year2023.Day12;

internal class Solution : SolutionBase
{
    private readonly Dictionary<string, long> _cache;
    private readonly List<(string, List<int>)> _springsAndGroupsList;

    public Solution() : base(12, 2023, "Hot Springs")
    {
        this._cache = new Dictionary<string, long>();
        this._springsAndGroupsList = new List<(string, List<int>)>();

        string[] splitLines = this.Input.SplitByNewline(true);

        foreach (string[] line in splitLines.Select(line => line.Split(' ')))
            this._springsAndGroupsList.Add((line[0].Trim(), line[1].Split(',').Select(int.Parse).ToList()));
    }

    protected override string SolvePartOne()
    {
        long result = 0;
        foreach ((string springs, var groups) in this._springsAndGroupsList)
            result += this.CalculateWithCache(springs, groups);
        
        return result.ToString();
    }

    protected override string SolvePartTwo()
    {
        long result = 0;
        this._cache.Clear();
        foreach ((string springs, var groups) in this._springsAndGroupsList)
        {
            // To unfold the records, on each row, replace the list of spring conditions with five copies of itself (separated by ?)
            // and replace the list of contiguous groups of damaged springs with five copies of itself (separated by ,).
            string springsP2 = string.Join('?', Enumerable.Repeat(springs, 5));
            var groupsP2 = Enumerable.Repeat(groups, 5).SelectMany(g => g).ToList();

            result += this.CalculateWithCache(springsP2, groupsP2);
        }

        return result.ToString();
    }

    private long CalculateWithCache(string springs, List<int> groups)
    {
        string key = $"{springs},{string.Join(',', groups)}";

        if (this._cache.TryGetValue(key, out long value))
            return value;
        
        value = Calculate(springs, groups);
        this._cache[key] = value;

        return value;
    }

    private long Calculate(string springs, List<int> groups)
    {
        var sb = new StringBuilder(springs);

        while (true)
        {
            if (groups.Count == 0)
                return sb.ToString().Contains('#') ? 0 : 1;

            if (string.IsNullOrEmpty(sb.ToString()))
                return 0;

            switch (sb[0])
            {
                case '.':
                    sb.Remove(0, 1);
                    continue;
                case '?':
                    return CalculateWithCache("." + sb.ToString(1, sb.Length - 1), groups) + CalculateWithCache("#" + sb.ToString(1, sb.Length - 1), groups);
                case '#' when groups.Count == 0:
                    return 0;
                case '#' when sb.Length < groups[0]:
                    return 0;
                case '#' when sb.ToString(0, groups[0]).Contains('.'):
                    return 0;
                case '#' when groups.Count > 1 && (sb.Length < groups[0] + 1 || sb[groups[0]] == '#'):
                    return 0;
                case '#' when groups.Count > 1:
                    sb.Remove(0, groups[0] + 1);
                    groups = groups.Skip(1).ToList();
                    continue;
                case '#':
                    sb.Remove(0, groups[0]);
                    groups = groups.Skip(1).ToList();
                    break;
                default:
                    throw new Exception("Invalid character");
            }
        }
    }
}