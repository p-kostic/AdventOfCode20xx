using System.Collections.Generic;

namespace AdventOfCode.Solutions.Year2020.Day08
{
	internal class Solution : SolutionBase
	{
		private readonly List<string> _input;

		public Solution() : base(08, 2020, "Handheld Halting")
		{
			this._input = new List<string>(Input.SplitByNewline());
		}

		protected override string SolvePartOne()
		{
			return Run(_input).ToString();
		}
		
		/// <summary>
		/// Strategy: Change ahead the instructions, and check if the repair was successful by seeing if it terminated
		/// </summary>
		protected override string SolvePartTwo()
		{
			for (var i = 0; i < _input.Count; i++)
			{
				var copiedInput = new List<string>(_input);
				var splitInputLine = _input[i].Split();

				switch (splitInputLine[0])
				{
					case "acc":
						continue;
					case "nop":
						splitInputLine[0] = "jmp";
						break;
					default:
						splitInputLine[0] = "nop";
						break;
				}

				copiedInput[i] = $"{splitInputLine[0]} {splitInputLine[1]}";

				if (TestValidRun(copiedInput))
					return Run(copiedInput).ToString();
			}
			return null;
		}

		private static bool TestValidRun(IReadOnlyList<string> input)
		{
			var visited = new HashSet<int>();
			var counter = 0;

			while (counter < input.Count)
			{
				if (visited.Contains(counter))
					return false;

				var instruction = input[counter].Split();
				visited.Add(counter);

				switch (instruction[0])
				{
					case "jmp":
						counter += int.Parse(instruction[1].TrimStart('+'));
						break;
					default:
						counter++;
						break;
				}
			}
			return true;
		}

		private static int Run(IReadOnlyList<string> input)
		{
			var visited = new HashSet<int>();
			var counter = 0;
			var acc = 0;

			while (counter < input.Count)
			{
				if (visited.Contains(counter))
					break;

				var instruction = input[counter].Split();
				visited.Add(counter);

				switch (instruction[0])
				{
					case "acc":
						acc += int.Parse(instruction[1].TrimStart('+'));
						counter++;
						break;
					case "nop":
						counter++;
						break;
					case "jmp":
						counter += int.Parse(instruction[1].TrimStart('+'));
						break;
				}
			}
			return acc;
		}
	}
}
