using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions.Year2023.Day19;

internal partial class Solution : SolutionBase
{
    private record Instruction(char Lhs, char Op, int Rhs, string Next);

    private static readonly Dictionary<string, Workflow> Workflows = new();

    private readonly List<Part> _parts;

    public Solution() : base(19, 2023, "Aplenty")
    {
        string[] workflowsAndParts = this.Input.SplitByParagraph(true);

        // Workflows + Instructions
        foreach (string workflowLine in workflowsAndParts[0].SplitByNewline())
        {
            var wfParts = MatchWorkflows().Match(workflowLine);
            var workflow = new Workflow(wfParts.Groups["Id"].Value, new List<Instruction>(), wfParts.Groups["Next"].Value);

            foreach (string step in wfParts.Groups["Steps"].Value.Split(','))
            {
                int rhs = MatchDigits().Matches(step).Select(x => int.Parse(x.Value)).First();
                workflow.Instructions.Add(new Instruction(step[0], step[1], rhs, step.Split(":")[^1]));
            }

            Workflows[workflow.Name] = workflow;
        }

        // Parts
        this._parts = new List<Part>();
        foreach (string part in workflowsAndParts[1].SplitByNewline())
        {
            int[] values = MatchDigits().Matches(part).Select(m => int.Parse(m.Value)).ToArray();
            this._parts.Add(new Part(values[0], values[1], values[2], values[3]));
        }
    }

    protected override string SolvePartOne() => this._parts.Sum(p => Workflows["in"].IsEvaluated(p) ? p.Total : 0).ToString();

    protected override string SolvePartTwo()
    {
        Dictionary<char, Range> initRanges = new()
        {
            ['x'] = new Range(1, 4000),
            ['m'] = new Range(1, 4000),
            ['a'] = new Range(1, 4000),
            ['s'] = new Range(1, 4000),
        };

        return GetLengths(initRanges, Workflows["in"]).ToString();
    }

    private static long GetLengths(Dictionary<char, Range> initRanges, Workflow initWorkflow)
    {
        long result = 0;

        foreach (var instruction in initWorkflow.Instructions)
        {
            var currRanges = initRanges.ToDictionary(kvp => kvp.Key, kvp => new Range(kvp.Value.Min, kvp.Value.Max));

            switch (instruction.Op)
            {
                case '>' when initRanges[instruction.Lhs].Max > instruction.Rhs:
                    {
                        currRanges[instruction.Lhs] = currRanges[instruction.Lhs] with { Min = Math.Max(currRanges[instruction.Lhs].Min, instruction.Rhs + 1) };

                        if (instruction.Next == "A")
                            result += GetProductOfLengths(currRanges);
                        else if (instruction.Next != "R")
                            result += GetLengths(currRanges, Workflows[instruction.Next]);

                        initRanges[instruction.Lhs] = initRanges[instruction.Lhs] with { Max = instruction.Rhs };
                        break;
                    }
                case '<' when initRanges[instruction.Lhs].Min < instruction.Rhs:
                    {
                        currRanges[instruction.Lhs] = currRanges[instruction.Lhs] with { Max = Math.Min(currRanges[instruction.Lhs].Max, instruction.Rhs - 1) };

                        if (instruction.Next == "A")
                            result += GetProductOfLengths(currRanges);
                        else if (instruction.Next != "R")
                            result += GetLengths(currRanges, Workflows[instruction.Next]);

                        initRanges[instruction.Lhs] = initRanges[instruction.Lhs] with { Min = instruction.Rhs };
                        break;
                    }
            }
        }

        if (initWorkflow.Target == "A")
            result += GetProductOfLengths(initRanges);
        else if (initWorkflow.Target != "R")
            result += GetLengths(initRanges, Workflows[initWorkflow.Target]);

        return result;
    }

    private static long GetProductOfLengths(Dictionary<char, Range> ranges)
    {
        return ranges.Aggregate(1L, (a, b) => a * b.Value.Length);
    }

    private record Range(long Min, long Max)
    {
        public long Length => this.Max - this.Min + 1;
    }

    private record Part(int X, int M, int A, int S)
    {
        public int Total => this.X + this.M + this.A + this.S;
    }

    private record Workflow(string Name, List<Instruction> Instructions, string Target)
    {
        public bool IsEvaluated(Part part)
        {
            foreach (var instruction in this.Instructions)
            {
                int partValue = instruction.Lhs switch
                {
                    'x' => part.X,
                    'm' => part.M,
                    'a' => part.A,
                    's' => part.S,
                };

                bool needsEvaluation = instruction.Op switch
                {
                    '<' => partValue < instruction.Rhs,
                    '>' => partValue > instruction.Rhs,
                };

                if (!needsEvaluation)
                    continue;

                return instruction.Next switch
                {
                    "A" => true,
                    "R" => false,
                    _ => Workflows[instruction.Next].IsEvaluated(part) // static data to the rescue
                };
            }

            return (this.Target) switch
            {
                "R" => false,
                "A" => true,
                _ => Workflows[this.Target].IsEvaluated(part) // static data to the rescue
            };
        }
    }

    [GeneratedRegex("-?\\d+")]
    private static partial Regex MatchDigits();
    [GeneratedRegex("^(?'Id'.+){(?'Steps'.*),(?'Next'.+)}")]
    private static partial Regex MatchWorkflows();
}