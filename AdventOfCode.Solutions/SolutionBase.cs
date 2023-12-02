global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Text;
global using AdventOfCode.Solutions.Utils;

using System.Diagnostics;
using System.Net;
using AdventOfCode.AdventOfCode.Solutions;

namespace AdventOfCode.Solutions;

public abstract class SolutionBase
{
    public int Day { get; }
    public int Year { get; }
    public string Title { get; }
    public bool Debug { get; set; }
    public string Input => LoadInput(this.Debug);
    public string DebugInput => LoadInput(true);

    public SolutionResult Part1 => Solve(1);
    public SolutionResult Part2 => Solve(2);

    private protected SolutionBase(int day, int year, string title, bool useDebugInput = false)
    {
        this.Day = day;
        this.Year = year;
        this.Title = title;
        this.Debug = useDebugInput;
    }

    public IEnumerable<SolutionResult> SolveAll()
    {
        yield return Solve(SolvePartOne);
        yield return Solve(SolvePartTwo);
    }

    public SolutionResult Solve(int part = 1)
    {
        return part switch
        {
            1 => Solve(SolvePartOne),
            2 => Solve(SolvePartTwo),
            _ => throw new InvalidOperationException("Invalid part param supplied.")
        };
    }

    private SolutionResult Solve(Func<string> solverFunction)
    {
        if (this.Debug)
        {
            if (string.IsNullOrEmpty(this.DebugInput))
            {
                throw new Exception("DebugInput is null or empty");
            }
        }
        else if (string.IsNullOrEmpty(this.Input))
        {
            throw new Exception("Input is null or empty");
        }

        try
        {
            var then = DateTime.Now;
            string result = solverFunction();
            var now = DateTime.Now;
            return string.IsNullOrEmpty(result)
                ? SolutionResult.Empty
                : new SolutionResult { Answer = result, Time = now - then };
        }
        catch (Exception)
        {
            if (!Debugger.IsAttached) 
                throw;

            Debugger.Break();

            return SolutionResult.Empty;
        }
    }

    private string LoadInput(bool debug = false)
    {
        string inputFilepath = $"./AdventOfCode.Solutions/Year{this.Year}/Day{this.Day:D2}/{(debug ? "debug" : "input")}";

        if (File.Exists(inputFilepath) && new FileInfo(inputFilepath).Length > 0)
        {
            return File.ReadAllText(inputFilepath);
        }

        if (debug) return "";

        try
        {
            string input = InputService.FetchInput(this.Year, this.Day).Result;
            File.WriteAllText(inputFilepath, input);
            return input;
        }
        catch (HttpRequestException e)
        {
            var code = e.StatusCode;
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            switch (code)
            {
                case HttpStatusCode.BadRequest:
                    Console.WriteLine($"Day {this.Day}: Received 400 when attempting to retrieve puzzle input. Your session cookie is probably not recognized.");
                    break;
                case HttpStatusCode.NotFound:
                    Console.WriteLine($"Day {this.Day}: Received 404 when attempting to retrieve puzzle input. The puzzle is probably not available yet.");
                    break;
                default:
                    Console.ForegroundColor = color;
                    Console.WriteLine(e.ToString());
                    break;
            }
            Console.ForegroundColor = color;
        }
        catch (InvalidOperationException)
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"Day {this.Day}: Cannot fetch puzzle input before given date (Eastern Standard Time).");
            Console.ForegroundColor = color;
        }

        return "";
    }

    public override string ToString() =>
        $"\n--- Day {this.Day}: {this.Title} --- {(this.Debug ? "!! Debug mode active, using DebugInput !!" : "")}\n"
        + $"{ResultToString(1, this.Part1)}\n"
        + $"{ResultToString(2, this.Part2)}";

    private static string ResultToString(int part, SolutionResult result) =>
        $"  - Part{part} => " + (string.IsNullOrEmpty(result.Answer)
            ? "Unsolved"
            : $"{result.Answer} ({result.Time.TotalMilliseconds}ms)");

    protected abstract string SolvePartOne();
    protected abstract string SolvePartTwo();
}