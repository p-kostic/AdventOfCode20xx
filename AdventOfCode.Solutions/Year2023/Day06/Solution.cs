namespace AdventOfCode.Solutions.Year2023.Day06;

internal class Solution : SolutionBase
{
    private record Race(int TimeInMs, long RecordDistanceInMm);

    private readonly List<Race> _races;

    public Solution() : base(06, 2023, "Wait For It")
    {
        string[] parsedLines = this.Input.SplitByNewline(true);
        int[] times = parsedLines[0].Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
        int[] distances = parsedLines[1].Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

        this._races = new List<Race>();
        for (int i = 0; i < times.Length; i++)
            this._races.Add(new Race(times[i], distances[i]));
        
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
        var race = new Race(41667266, 244104712281040);

        int waysToWinThisRace = 0;

        for (long holdDownTime = 0; holdDownTime < race.TimeInMs; holdDownTime++)
        {
            long remainingTime = race.TimeInMs - holdDownTime;
            long speedInMms = holdDownTime;
            long traveledDistance = speedInMms * remainingTime;

            if (traveledDistance > race.RecordDistanceInMm)
                waysToWinThisRace++;
        }

        return waysToWinThisRace.ToString();
    }
}