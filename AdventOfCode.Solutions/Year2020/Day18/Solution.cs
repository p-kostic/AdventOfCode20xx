using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions.Year2020
{
    internal class Day18 : SolutionBase
    {
        private readonly string[] _splitInput;

        public Day18() : base(18, 2020, "Operation Order")
        {
            this._splitInput = this.Input.SplitByNewline();
        }

        protected override string SolvePartOne() => this._splitInput.Select(EvaluateSequencePart1).Sum().ToString();

        /// <summary>
        /// Evaluate left-to-right regardless of the order in which they appear.
        /// </summary>
        private static long EvaluatePart1(string expression)
        {
            var expressionParts = expression.Replace("(", "")
                                                   .Replace(")", "")
                                                   .Split(' ');

            var result = long.Parse(expressionParts[0]);
            for (var i = 1; i < expressionParts.Length; i += 2)
            {
                switch (expressionParts[i])
                {
                    case "+":
                        result += long.Parse(expressionParts[i + 1]);
                        break;
                    case "*":
                        result *= long.Parse(expressionParts[i + 1]);
                        break;
                }
            }
            return result;
        }

        private static long EvaluateSequencePart1(string expression)
        {
            var matches = MatchParenthesis(expression);

            if (matches.Count == 0)
                return EvaluatePart1(expression);

            while (matches.Count != 0)
            {
                foreach (Match match in matches)
                    expression = expression.Replace(match.Value, EvaluatePart1(match.Value).ToString());

                matches = MatchParenthesis(expression);
            }
            return EvaluatePart1(expression);
        }

        protected override string SolvePartTwo() => this._splitInput.Select(EvaluateSequencePart2).Sum().ToString();

        /// <summary>
        /// Now addition is evaluated before multiplication. First for all additions:
        /// 1: Evaluate first occurrence (i.e. long.Parse() + long.Parse())
        /// 2: Replace evaluated added value in the expression once
        /// 3: Repeat until the Regex doesn't match anymore
        /// Do the same for all the multiplications
        /// </summary>
        private static long EvaluatePart2(string expression)
        {
            expression = expression.Replace("(", "").Replace(")", "");
            var addition = MatchAddition(expression);
            while (addition.Success)
            {
                var addSplit = addition.Value.Split(' ');
                var evaluatedAdd = long.Parse(addSplit[0]) + long.Parse(addSplit[2]);
                expression = new Regex(Regex.Escape(addition.Value)).Replace(expression, evaluatedAdd.ToString(), 1);
                addition = MatchAddition(expression);
            }

            var multiplication = MatchMultiplication(expression);
            while (multiplication.Success)
            {
                var multiSplit = multiplication.Value.Split(' ');
                var evaluatedMulti = long.Parse(multiSplit[0]) * long.Parse(multiSplit[2]);
                expression = new Regex(Regex.Escape(multiplication.Value)).Replace(expression, evaluatedMulti.ToString(), 1);
                multiplication = MatchMultiplication(expression);
            }
            return long.Parse(expression);
        }

        private static long EvaluateSequencePart2(string expression)
        {
            var matches = MatchParenthesis(expression);

            if (matches.Count == 0)
                return EvaluatePart2(expression);

            while (matches.Count != 0)
            {
                foreach (Match match in matches)
                    expression = expression.Replace(match.Value, EvaluatePart2(match.Value).ToString());

                matches = MatchParenthesis(expression);
            }

            return EvaluatePart2(expression);
        }

        // Match inner parentheses within the expression, containing digits, add, or multiply
        private static MatchCollection MatchParenthesis(string expression) => Regex.Matches(expression, @"\([\d +\*]+\)");

        // Match 'digits + digits'
        private static Match MatchAddition(string expression) => Regex.Match(expression, @"\d+ \+ \d+");

        // Match 'digits * digits'
        private static Match MatchMultiplication(string expression) => Regex.Match(expression, @"\d+ \* \d+");
    }
}
