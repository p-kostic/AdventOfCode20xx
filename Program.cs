using AdventOfCode;
using AdventOfCode.Solutions;

var config = Config.Get();
int year = config.Year;
int[] days = config.Days;

if (args.Length > 0 && int.TryParse(args.First(), out int day)) 
    days = [day];

foreach (var solution in SolutionCollector.FetchSolutions(year, days))
{
    Console.WriteLine(solution.ToString());
}