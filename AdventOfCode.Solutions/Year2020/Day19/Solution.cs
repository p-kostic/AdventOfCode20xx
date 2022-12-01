using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions.Year2020
{
    internal class Day19 : SolutionBase
    {
        private readonly string[] _messages;
        private readonly Dictionary<string, string> _rules;

        public Day19() : base(19, 2020, "Monster Messages")
        {
            var splitInput = this.Input.Split("\n\n");

            this._rules = splitInput[0].SplitByNewline()
                                       .Select(ParseRule)
                                       .ToDictionary(x => x.Key, x => x.Value);

            this._messages = splitInput[1].SplitByNewline();
        }

        private static KeyValuePair<string, string> ParseRule(string line)
        {
            var splitLine = line.Split(":");
            var value = splitLine[1].Replace("\"", "").Trim();

            if (!value.Contains("|"))
                return new KeyValuePair<string, string>(splitLine[0], value);

            var splitValue = value.Split("|");
            value = $"( {splitValue[0]} | {splitValue[1]} )";

            return new KeyValuePair<string, string>(splitLine[0], value);
        }

        protected override string SolvePartOne()
        {
            var regex = $"^{GenerateRegex()}$";
            return this._messages.Count(x => Regex.IsMatch(x, regex)).ToString();
        }

        protected override string SolvePartTwo()
        {
            this._rules["8"] = "( 42 | 42 8 )";
            this._rules["11"] = "( 42 31 | 42 11 31 )";
            var regex = $"^{GenerateRegex()}$";
            return this._messages.Count(x => Regex.IsMatch(x, regex)).ToString();
        }

        private string GenerateRegex()
        {
            var current = this._rules["0"].Split(" ").ToList();

            while (current.Any(x => x.Any(char.IsDigit)) && current.Count < 100000)
                current = current.Select(x => this._rules.ContainsKey(x) ? this._rules[x] : x)
                                 .SelectMany(x => x.Split(" "))
                                 .ToList();
            
            current.Remove("8");
            current.Remove("11");

            return string.Join("", current);
        }
    }
}

