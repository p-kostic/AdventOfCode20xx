using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2020
{
    internal class Day16 : SolutionBase
    {
        private readonly Dictionary<string, (int Min, int Max)[]> _criteria;
        private readonly List<long> _ticket;
        private readonly List<List<int>> _otherTickets;

        public Day16() : base(16, 2020, "Ticket Translation")
        {
            var splitBlockInput = Input.Split("\n\n").ToList();

            // Parse the criteria
            _criteria = new Dictionary<string, (int Min, int Max)[]>();
            foreach (var line in splitBlockInput[0].Split("\n"))
            {
                var splitLine = line.Split(" ");
                var key = string.Join(" ", splitLine[..^3]);

                var splitCriteriaOne = splitLine[^3].Split('-');
                var criteriaOne = (int.Parse(splitCriteriaOne[0]), int.Parse(splitCriteriaOne[1]));

                var splitCriteriaTwo = splitLine[^1].Split('-');
                var criteriaTwo = (int.Parse(splitCriteriaTwo[0]), int.Parse(splitCriteriaTwo[1]));

                _criteria.Add(key, new[] { criteriaOne, criteriaTwo });
            }

            // Parse 'your ticket'
            _ticket = splitBlockInput[1].Split("\n")[1]
                                        .Split(",")
                                        .Select(long.Parse)
                                        .ToList();

            // Parse 'nearby tickets'
            _otherTickets = splitBlockInput[2].Split("\n")[1..]
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
            var validTicketsTransposed = Transpose(_otherTickets.Where(x => Check(_criteria.SelectMany(kvp => kvp.Value).ToList(), x)));
            var matchingFields = validTicketsTransposed.Select((fields, index) => (_criteria.Keys.Where(x => Check(x, fields)), index)).ToList();

            var result = new List<int>();
            var usedCriteria = new HashSet<string>();

            foreach (var (fields, index) in validTicketsTransposed.Select(_ => matchingFields.First()))
            {
                var enumeratedFields = fields as string[] ?? fields.ToArray();
                if (enumeratedFields.First().StartsWith("depart"))
                    result.Add(index);

                usedCriteria.Add(enumeratedFields.First());
                matchingFields = matchingFields.Select(x => (x.Item1.Where(y => !usedCriteria.Contains(y)), x.index))
                                               .Where(x => x.Item1.Any()).OrderBy(x => x.Item1.Count())
                                               .ToList();
            }

            return result.Select(x => _ticket[x])
                         .Aggregate((x, y) => x * y)
                         .ToString();
        }

        /// <summary>
        /// Checks the tickets for a given criteria
        /// </summary>
        private bool Check(string key, IEnumerable<int> tickets)
        {
            return tickets.All(x => _criteria[key].Any(y => x >= y.Min && x <= y.Max));
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
