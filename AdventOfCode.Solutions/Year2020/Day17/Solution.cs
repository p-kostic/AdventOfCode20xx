using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2020
{
    internal class Day17 : SolutionBase
    {
        private Dictionary<(int x, int y, int z), bool> _cube3D;
        private Dictionary<(int x, int y, int z, int w), bool> _cube4D;

        public Day17() : base(17, 2020, "Conway Cubes")
        {
            this._cube3D = new Dictionary<(int x, int y, int z), bool>();
            this._cube4D = new Dictionary<(int x, int y, int z, int w), bool>();

            var lines = this.Input.SplitByNewline();

            // Parse all coordinates that are == '#' in the plane ('z' (and 4th dimension 'w' for Part 2) are 0) to true/false
            for (var x = 0; x < lines.Length; x++)
                for (var y = 0; y < lines[x].Length; y++)
                {
                    this._cube3D[(x, y, 0)] = lines[x][y] == '#';
                    this._cube4D[(x, y, 0, 0)] = lines[x][y] == '#';
                }

            // Init the bounding volume since the cube will expand. Values here are somewhat 'safe'. TODO: An improvement can be made here
            // Remember to not overwrite the ones we just parsed in the loop above
            for (var x = -15; x < 15; x++)
                for (var y = -15; y < 15; y++)
                    for (var z = -8; z < 8; z++)
                    {
                        if (!this._cube3D.ContainsKey((x, y, z)))
                            this._cube3D[(x, y, z)] = false;

                        // Part 2
                        for (var w = -8; w < 8; w++)
                            if (!this._cube4D.ContainsKey((x, y, z, w)))
                                this._cube4D[(x, y, z, w)] = false;
                    }
        }

        /// <summary>
        /// Run the following two rules for six steps
        /// 1: If a cube is active and exactly 2 or 3 of its neighbors are also active, the cube remains active. Otherwise, the cube becomes inactive
        /// 2: If a cube is inactive but exactly 3 of its neighbors are active, the cube becomes active. Otherwise, the cube remains inactive.
        /// </summary>
        protected override string SolvePartOne()
        {
            Enumerable.Range(0, 6).ForEach(_ =>
            {
                var nextCube3D = new Dictionary<(int x, int y, int z), bool>();
                foreach (var c in this._cube3D.Keys)
                {
                    var amountTrue = 0;
                    foreach (var neighborPos in NeighborDirections3D().Select(a => (a.x + c.x, a.y + c.y, a.z + c.z)))
                    {
                        if (this._cube3D.GetValueOrDefault(neighborPos, false))
                            amountTrue++;
                        if (!nextCube3D.ContainsKey(neighborPos))
                            nextCube3D[neighborPos] = false;
                    }
                    if (this._cube3D[c])
                        nextCube3D[c] = amountTrue == 2 || amountTrue == 3;
                    if (!this._cube3D[c])
                        nextCube3D[c] = amountTrue == 3;
                }
                this._cube3D = new Dictionary<(int x, int y, int z), bool>(nextCube3D);
            });
            return this._cube3D.Count(a => a.Value).ToString();
        }

        /// <summary>
        /// Same as Part 1, but with an added dimension
        /// </summary>
        protected override string SolvePartTwo()
        {
            Enumerable.Range(0, 6).ForEach(_ =>
            {
                var nextCube4D = new Dictionary<(int x, int y, int z, int w), bool>();
                foreach (var c in this._cube4D.Keys)
                {
                    var amountTrue = 0;
                    foreach (var neighborPos in NeighborDirections4D().Select(a => (a.x + c.x, a.y + c.y, a.z + c.z, a.w + c.w)))
                    {
                        if (this._cube4D.GetValueOrDefault(neighborPos, false))
                            amountTrue++;

                        if (!nextCube4D.ContainsKey(neighborPos))
                            nextCube4D[neighborPos] = false;
                    }

                    if (this._cube4D[c])
                        nextCube4D[c] = amountTrue == 2 || amountTrue == 3;
                    if (!this._cube4D[c])
                        nextCube4D[c] = amountTrue == 3;
                }
                this._cube4D = new Dictionary<(int x, int y, int z, int w), bool>(nextCube4D);
            });
            return this._cube4D.Count(a => a.Value).ToString();
        }

        private static IEnumerable<(int x, int y, int z, int w)> NeighborDirections4D()
        {
            var result = new List<(int x, int y, int z, int w)>();
            foreach (var x in Enumerable.Range(-1, 3))
                foreach (var y in Enumerable.Range(-1, 3))
                    foreach (var z in Enumerable.Range(-1, 3))
                        foreach (var w in Enumerable.Range(-1, 3))
                        {
                            if (x == 0 && y == 0 && z == 0 && w == 0)
                                continue;
                            result.Add((x, y, z, w));
                        }
            return result;
        }

        private static IEnumerable<(int x, int y, int z)> NeighborDirections3D()
        {
            var result = new List<(int x, int y, int z)>();
            foreach (var x in Enumerable.Range(-1, 3))
                foreach (var y in Enumerable.Range(-1, 3))
                    foreach (var z in Enumerable.Range(-1, 3))
                    {
                        if (x == 0 && y == 0 && z == 0)
                            continue;
                        result.Add((x, y, z));
                    }
            return result;
        }
    }
}
