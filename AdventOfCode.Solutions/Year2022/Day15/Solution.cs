namespace AdventOfCode.Solutions.Year2022.Day15;

internal class Solution : SolutionBase
{
    private readonly HashSet<(long x, long y)> _map;
    private readonly List<(long x, long y, long d)> _sensors;

    public Solution() : base(15, 2022, "Beacon Exclusion Zone")
    {
        this._map = new HashSet<(long x, long y)>();
        this._sensors = new List<(long x, long y, long d)>();
        foreach (string line in this.Input.SplitByNewline())
        {
            string[] splitLine = line.Replace(",", "").Replace(":", "").Split(' ');
            var beacon = (long.Parse(splitLine[8].Split('=')[1]), long.Parse(splitLine[9].Split('=')[1]));
            (long x, long y) parsedSensor = (long.Parse(splitLine[2].Split('=')[1]), long.Parse(splitLine[3].Split('=')[1]));

            this._map.Add(beacon);
            this._map.Add((parsedSensor.x, parsedSensor.y));
            this._sensors.Add((parsedSensor.x, parsedSensor.y, CalculationUtils.ManhattanDistance(parsedSensor, beacon)));
        }
    }

    protected override string SolvePartOne()
    {
        long minRange = this._sensors.Select(s => s.x - (s.d - Math.Abs(s.y - 2000000))).Min();
        long maxRange = this._sensors.Select(s => s.x + (s.d - Math.Abs(s.y - 2000000))).Max();

        long total = 0;
        for (long i = minRange; i <= maxRange; i++)
        {
            long maxX = this._sensors.Where(s => CalculationUtils.ManhattanDistance((s.x, s.y), (i, 2000000)) <= s.d)
                                     .Select(s => s.d + s.x - Math.Abs(s.y - 2000000))
                                     .OrderByDescending(x => x)
                                     .FirstOrDefault(minRange - 1);

            if (maxX <= minRange - 1)
                continue;

            total += maxX - i + 1 - this._map.Count(m => m.y == 2000000 && m.x >= i && m.x <= maxX);
            i = maxX;
        }

        return total.ToString();
    }

    protected override string SolvePartTwo()
    {
        foreach ((long x, long y, long d) in this._sensors)
            for (long i = Math.Max(0, x - d - 1); i <= Math.Min(4000000, x + d + 1); i++)
            {
                long positiveY = Math.Min(y + d + 1 - Math.Abs(x - i), 4000000);
                long negativeY = Math.Max(y - (d + 1 - Math.Abs(x - i)), 0);

                if (this._sensors.All(s => CalculationUtils.ManhattanDistance((s.x, s.y), (i, positiveY)) > s.d))
                    return (i * 4000000 + positiveY).ToString();

                if (this._sensors.All(s => CalculationUtils.ManhattanDistance((s.x, s.y), (i, negativeY)) > s.d))
                    return (i * 4000000 + negativeY).ToString();
            }

        return "Unknown";
    }
}
