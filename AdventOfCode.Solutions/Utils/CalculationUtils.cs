namespace AdventOfCode.Solutions.Utils;

public static class CalculationUtils
{
    public static double FindGCD(double a, double b)
    {
        if (a == 0 || b == 0) return Math.Max(a, b);
        return (a % b == 0) ? b : FindGCD(b, a % b);
    }

    public static double FindLCM(double a, double b) => a * b / FindGCD(a, b);

    public static int ManhattanDistance((int x, int y) a, (int x, int y) b) =>
        Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);

    public static long ManhattanDistance((long x, long y) a, (long x, long y) b) =>
        Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);

    public static void ForEach<T>(this IEnumerable<T> @enum, Action<T> mapFunction)
    {
        foreach (var item in @enum) mapFunction(item);
    }

    /// <summary>
    /// Source: https://stackoverflow.com/questions/33336540/how-to-use-linq-to-find-all-combinations-of-n-items-from-a-set-of-numbers
    /// </summary>
    public static IEnumerable<IEnumerable<T>> DifferentCombinations<T>(this IEnumerable<T> elements, int k)
    {
        return k == 0 ? new[] { Array.Empty<T>() } :
            elements.SelectMany((e, i) =>
                elements.Skip(i + 1).DifferentCombinations(k - 1).Select(c => new[] { e }.Concat(c)));
    }

    public static (int, int) Add(this (int x, int y) a, (int x, int y) b) => (a.x + b.x, a.y + b.y);
}
