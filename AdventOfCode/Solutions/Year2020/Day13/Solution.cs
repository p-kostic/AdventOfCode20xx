using System.Linq;

namespace AdventOfCode.Solutions.Year2020
{
    internal class Day13 : ASolution
    {
        private readonly int _earliestDepart;
        private readonly int[] _busDepartures;

        public Day13() : base(13, 2020, "Shuttle Search")
        {
            var parsedInput = Input.SplitByNewline();
            _earliestDepart = int.Parse(parsedInput[0]);
            _busDepartures = parsedInput[1].Split(',')
                                           .Select(x => int.TryParse(x, out var parsedDepart) ? parsedDepart : 0)
                                           .ToArray();
        }

        protected override string SolvePartOne()
        {
            var (time, busId) = _busDepartures.Where(bus => bus > 0)
                                                     .Select(bus => ((_earliestDepart / bus + 1) * bus, bus))
                                                     .Min();
            return ((time - _earliestDepart) * busId).ToString();
        }

        protected override string SolvePartTwo()
        {
            long earliest = _busDepartures[0];
            long inc = earliest;

            for (int i = 1; i < _busDepartures.Length; i++)
            {
                if (_busDepartures[i] == 0) // Don't do .Where(x => x > 0) initially like Part 1, since i++ will be skipped for mod iterations
                    continue;
                do earliest += inc; 
                while (earliest % _busDepartures[i] != _busDepartures[i] - i % _busDepartures[i]);

                inc = (long)Utilities.FindLCM(inc, _busDepartures[i]);
            }
            return earliest.ToString();
        }
    }
}
