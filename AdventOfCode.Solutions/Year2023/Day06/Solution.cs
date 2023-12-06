namespace AdventOfCode.Solutions.Year2023.Day06;

internal class Solution : SolutionBase
{
    private record Race(int TimeInMs, long RecordDistanceInMm);

    private readonly List<Race> _races;
    private readonly Race _racePartTwo;

    public Solution() : base(06, 2023, "Wait For It")
    {
        string[] parsedLines = this.Input.SplitByNewline(true);
        int[] times = parsedLines[0].Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
        int[] distances = parsedLines[1].Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

        this._races = new List<Race>();
        for (int i = 0; i < times.Length; i++)
            this._races.Add(new Race(times[i], distances[i]));

        // Part 2
        this._racePartTwo = new Race(int.Parse(string.Concat(times.Select(x => x.ToString()))), long.Parse(string.Concat(distances.Select(x => x.ToString()))));
    }

    protected override string SolvePartOne()
    {
        var waysToWin = new List<int>();

        foreach (var race in this._races)
        {
            int waysToWinThisRace = 0;

            for (int holdDownTime = 0; holdDownTime < race.TimeInMs; holdDownTime++)
            {
                int remainingTime = race.TimeInMs - holdDownTime;
                int speedInMms = holdDownTime;
                int traveledDistance = speedInMms * remainingTime;

                if (traveledDistance > race.RecordDistanceInMm)
                    waysToWinThisRace++;
            }

            waysToWin.Add(waysToWinThisRace);
        }

        return waysToWin.Aggregate(1, (acc, val) => acc * val).ToString();
    }

    protected override string SolvePartTwo()
    {
        int waysToWinThisRace = 0;

        for (long holdDownTime = 0; holdDownTime < this._racePartTwo.TimeInMs; holdDownTime++)
        {
            long remainingTime = this._racePartTwo.TimeInMs - holdDownTime;
            long speedInMms = holdDownTime;
            long traveledDistance = speedInMms * remainingTime;

            if (traveledDistance > this._racePartTwo.RecordDistanceInMm)
                waysToWinThisRace++;
        }

        return waysToWinThisRace.ToString();
    }
}