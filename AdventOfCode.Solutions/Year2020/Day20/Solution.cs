using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2020.Day20
{
    internal class Solution : SolutionBase
    {
        private readonly Dictionary<int, SubGrid> _inputGrids;

        public enum Orientation
        {
            Top = 0,
            Bottom = 1,
            Left = 2,
            Right = 3
        };

        public Solution() : base(20, 2020, "Jurassic Jigsaw")
        {
            this._inputGrids = this.Input.SplitByParagraph(true).Select(x => new SubGrid(x)).ToDictionary(x => x.Id, x => x);
        }

        protected override string SolvePartOne()
        {
            var corners = this._inputGrids.Where(x => SideMatchCount(x.Key) == 2);
            return corners.Aggregate<KeyValuePair<int, SubGrid>, long>(1, (curId, kvp) => curId * kvp.Key).ToString();
        }

        private int SideMatchCount(int tileId)
        {
            return this._inputGrids[tileId].Sides.Count(x =>
                   this._inputGrids.Any(y => y.Key != tileId && y.Value.Sides.Any(s => s == x || s == x.Reverse())));
        }

        protected override string SolvePartTwo()
        {
            return "";
        }

        internal class SubGrid
        {
            public int Id;
            public char[,] Grid;
            public List<string> Sides;

            public SubGrid(string inputTile)
            { 
                this.Id = int.Parse(inputTile.Substring(5, 4));

                var gridToParse = inputTile.Substring(11).SplitByNewline();

                // Grid to Char array (Part 2)
                this.Grid = new char[10, 10];
                for (var i = 0; i < gridToParse.Length; i++)
                    for (var j = 0; j < gridToParse[1].Length; j++)
                        this.Grid[i, j] = gridToParse[j][i];

                // Calculate left and right side
                string left = string.Empty, right = string.Empty;
                foreach (var line in gridToParse)
                {
                    left += line[0];
                    right += line[gridToParse.Length - 1];
                }

                this.Sides = new List<string> { gridToParse[0], gridToParse[^1], left, right };
            }
        }
    }
}