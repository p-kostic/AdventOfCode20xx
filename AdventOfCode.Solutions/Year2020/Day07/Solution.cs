using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions.Year2020.Day07
{
	class Solution : SolutionBase
	{
		private struct Bag
		{
			public int Count;
			public string Color;
		}

		private readonly Dictionary<string, List<Bag>> _parsedInput;

		/// <summary>
		/// Parse input to Dictionary<string, List of bags with count and color>
		/// where every bag itself has an entry in the dictionary etc. :)
		/// </summary>
		public Solution() : base(07, 2020, "Handy Haversacks")
		{
			_parsedInput = new Dictionary<string, List<Bag>>();

			var replacedInput = Input.Replace("bags", "bag")
											.Replace(".", "");

			var inputArray = replacedInput.SplitByNewline();
			foreach (var inputLine in inputArray)
			{
				var bagColor = inputLine.Split(" contain ")[0];
				var bagContents = inputLine.Split(" contain ")[1].Split(",");
				_parsedInput.Add(bagColor, bagContents.Select(Parse).ToList());
			}
		}

		private static Bag Parse(string content)
		{
			content = content.Trim();
			// Match digits and parse them, evaluate to 0 if none present
			int[] numbers = Regex.Matches(content, @"\d+")
								 .Select(x => int.Parse(x.Value))
								 .ToArray();
			return new Bag
			{
				Count = !numbers.Any() ? 0 : numbers.First(),
				Color = Regex.Replace(content, @"\d+", "").Trim()
			};
		}

		protected override string SolvePartOne()
		{
			// When value color contains 'shiny gold bag', select the keys to stringList
			List<string> bags = _parsedInput.Where(x => x.Value.Select(y => y.Color)
																					  .Contains("shiny gold bag"))
																					  .Select(x => x.Key)
																					  .ToList();

			// Follow graph where any bag colors intersect, concat to total, distinct count = How many bag colors can eventually contain at least one shiny gold bag?
			List<string> total = bags;
			while (bags.Any())
			{
				var next = _parsedInput.Where(x => x.Value.Select(bag1 => bag1.Color)
																								   .Intersect(bags)
																								   .Any())
														 .ToList();

				bags = next.Select(x => x.Key).ToList();
				total = total.Concat(bags).ToList();
			}

			return total.Distinct().Count().ToString();
		}

		/// <summary>
		/// Recursive function over the dictionary strategy
		/// </summary>
		protected override string SolvePartTwo()
		{
			var goldBag = _parsedInput["shiny gold bag"];
			return CalculateContentsCount(goldBag).ToString();
		}

		/// <summary>
		/// How many individual bags are required inside your single shiny gold bag? Recursive function of
		/// the sum of "shiny gold bag's dictionary value content counts" + the sum of any color within it * it's count
		/// </summary>
		private int CalculateContentsCount(IReadOnlyCollection<Bag> bags)
		{
			return bags.Sum(x => x.Count) +
				   bags.Sum(x => x.Color == "no other bag" ? 0 : CalculateContentsCount(_parsedInput[x.Color]) * x.Count);
		}
	}
}
