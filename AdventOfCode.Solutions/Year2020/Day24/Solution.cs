using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions.Year2020.Day24
{
    internal class Solution : SolutionBase
    {
        private readonly List<List<string>> _rules;
        private readonly List<string> _direction;

        public Solution() : base(24, 2020, "Lobby Layout")
        {
            this._rules = new List<List<string>>();

            var lines = this.Input.SplitByNewline();
            foreach (var line in lines)
            {
                var matches = Regex.Matches(line, @"e|se|sw|w|nw|ne");
                var rule = new List<string>();

                for (var i = 0; i < matches.Count; i++)
                    rule.Add(matches[i].Value);
                
                this._rules.Add(rule);
            }

            // Part 2
            this._direction = new List<string> { "e", "se", "sw", "w", "nw", "ne" };
        }

        protected override string SolvePartOne()
        {
            var tiles = new Dictionary<(int, int, int), bool>();

            foreach (var tile in this._rules.Select(rule => new {rule, tile = (0, 0, 0)}).Select(x => x.rule.Aggregate(x.tile, MoveTile)))
            {
                if (tiles.ContainsKey(tile))
                    tiles[tile] = !tiles[tile];
                else
                    tiles.Add(tile, true);
            }
            return tiles.Count(x => x.Value).ToString();
        }

        public (int, int, int) MoveTile((int, int, int) tile, string direction)
        {
            switch (direction)
            {
                case "e":
                    tile.Item3++;
                    break;
                case "se":
                    tile.Item2 += tile.Item1;
                    tile.Item3 += tile.Item1;
                    tile.Item1 = 1 - tile.Item1;
                    break;
                case "sw":
                    tile.Item2 += tile.Item1;
                    tile.Item3 -= (1 - tile.Item1);
                    tile.Item1 = 1 - tile.Item1;
                    break;
                case "w":
                    tile.Item3--;
                    break;
                case "nw":
                    tile.Item2 -= (1 - tile.Item1);
                    tile.Item3 -= (1 - tile.Item1);
                    tile.Item1 = 1 - tile.Item1;
                    break;
                case "ne":
                    tile.Item2 -= (1 - tile.Item1);
                    tile.Item3 += tile.Item1;
                    tile.Item1 = 1 - tile.Item1;
                    break;
            }

            return tile;
        }

        protected override string SolvePartTwo()
        {
            var tiles = new Dictionary<(int, int, int), bool>();

            // redo part 1
            foreach (var tile in this._rules.Select(rule => new { rule, tile = (0, 0, 0) }).Select(x => x.rule.Aggregate(x.tile, MoveTile)))
            {
                if (tiles.ContainsKey(tile))
                    tiles[tile] = !tiles[tile];
                else
                    tiles.Add(tile, true);
            }

            for (var i = 0; i < 100; i++)
            {
                var changeToWhite = tiles.Where(x => x.Value).Where(x =>
                {
                    var neighbors = GetNeighbors(x.Key);
                    var blackTileCount = tiles.Count(t => neighbors.Contains(t.Key) && t.Value);
                    return blackTileCount == 0 || blackTileCount > 2;
                }).ToList();

                // start slowness
                var changeToBlack = tiles.Where(x => x.Value)
                                         .SelectMany(x => GetNeighbors(x.Key))
                                         .ToHashSet()
                                         .Where(x => !tiles.ContainsKey(x) || !tiles[x])
                                         .Where(x =>
                                         { 
                                             var neighbors = GetNeighbors(x); 
                                             var blackTileCount = tiles.Count(t => neighbors.Contains(t.Key) && t.Value); 
                                             return blackTileCount == 2;
                                         }).ToList();
                // end slowness
                
                foreach (var (key, _) in changeToWhite)
                    tiles[key] = false;
                
                foreach (var ctb in changeToBlack)
                {
                    if (tiles.ContainsKey(ctb))
                        tiles[ctb] = true;
                    else
                        tiles.Add(ctb, true);
                }
            }

            return tiles.Count(x => x.Value).ToString();
        }

        public List<(int, int, int)> GetNeighbors((int, int, int) tile) => this._direction.Select(dir => MoveTile(tile, dir)).ToList();
    }
}
