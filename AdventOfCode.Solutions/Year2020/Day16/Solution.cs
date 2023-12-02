using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2020.Day16
{
    internal class Solution : SolutionBase
    {
        private readonly Dictionary<string, (int Min, int Max)[]> _criteria;
        private readonly List<long> _ticket;
        private readonly List<List<int>> _otherTickets;

        public Solution() : base(16, 2020, "Ticket Translation")
        {
            var splitBlockInput = this.Input.Split("\n\n").ToList();

            // Parse the criteria
            this._criteria = new Dictionary<string, (int Min, int Max)[]>();
            foreach (var line in splitBlockInput[0].Split("\n"))
            {
                string[] splitLine = line.Split(" ");
                string key = string.Join(" ", splitLine[..^3]);

                string[] splitCriteriaOne = splitLine[^3].Split('-');
                var criteriaOne = (int.Parse(splitCriteriaOne[0]), int.Parse(splitCriteriaOne[1]));

                string[] splitCriteriaTwo = splitLine[^1].Split('-');
                var criteriaTwo = (int.Parse(splitCriteriaTwo[0]), int.Parse(splitCriteriaTwo[1]));

                this._criteria.Add(key, new[] { criteriaOne, criteriaTwo });
            }

            // Parse 'your ticket'
            this._ticket = splitBlockInput[1].Split("\n")[1]
                                        .Split(",")
                                        .Select(long.Parse)
                                        .ToList();

            // Parse 'nearby tickets'
            this._otherTickets = splitBlockInput[2].SplitByNewline(true)[1..]
                                              .Select(x => x.Split(","))
                                              .Select(x => x.Select(int.Parse).ToList())
                                              .ToList();
        }

        // Strategy: Check against criteria range bounds
        protected override string SolvePartOne()
        {
            return _otherTickets.SelectMany(ticket => ticket, (ticket, value) => new { ticket, value })
                                .Where(@t => !_criteria.SelectMany(x => x.Value).Any(x => @t.value >= x.Min && @t.value <= x.Max))
                                .Select(@t => @t.value)
                                .Sum()
                                .ToString();
        }

        // Strategy: Transpose in order to look at indices across tickets against the rule sets.
        //           Sort by the number of rule sets each index can meet (i.e. not yet in usedCriteriaSet) and eliminate one-by-one.
        protected override string SolvePartTwo()
        {
            var validTicketsTransposed = Transpose(this._otherTickets.Where(x => Check(this._criteria.SelectMany(kvp => kvp.Value).ToList(), x)));
            var matchingFields = validTicketsTransposed.Select((fields, index) => (this._criteria.Keys.Where(x => Check(x, fields)), index)).ToList();

            var result = new List<int>();
            var usedCriteria = new HashSet<string>();

            foreach ((var fields, int index) in validTicketsTransposed.Select(_ => matchingFields.First()))
            {
                string[] enumeratedFields = fields as string[] ?? fields.ToArray();
                if (enumeratedFields.First().StartsWith("depart"))
                    result.Add(index);

                usedCriteria.Add(enumeratedFields.First());
                matchingFields = matchingFields.Select(x => (x.Item1.Where(y => !usedCriteria.Contains(y)), x.index))
                                               .Where(x => x.Item1.Any()).OrderBy(x => x.Item1.Count())
                                               .ToList();
            }

            return result.Select(x => this._ticket[x])
                         .Aggregate((x, y) => x * y)
                         .ToString();
        }

        /// <summary>
        /// Checks the tickets for a given criteria
        /// </summary>
        private bool Check(string key, IEnumerable<int> tickets)
        {
            return tickets.All(x => this._criteria[key].Any(y => x >= y.Min && x <= y.Max));
        }

        /// <summary>
        /// Checks all tickets against all criterion
        /// </summary>
        private static bool Check(IReadOnlyCollection<(int Min, int Max)> allCriterion, IEnumerable<int> ticket)
        {
            return ticket.All(x => allCriterion.ToList().Any(y => x >= y.Min && x <= y.Max));
        }

        // Source https://stackoverflow.com/questions/39484996/rotate-transposing-a-listliststring-using-linq-c-sharp
        private static List<List<int>> Transpose(IEnumerable<List<int>> input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            return input.SelectMany(inner => inner.Select((item, index) => new { item, index }))
                        .GroupBy(i => i.index, i => i.item)
                        .Select(g => g.ToList())
                        .ToList();
        }
    }
}
