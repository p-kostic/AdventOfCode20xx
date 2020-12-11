using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2020
{
    internal class Day11 : ASolution
    {
        private readonly NeighborGrid _seatGrid;

        public Day11() : base(11, 2020, "Seating System")
        {
            this._seatGrid = new NeighborGrid(Input);
        }

        protected override string SolvePartOne()
        {
            var firstClone = this._seatGrid.Copy();
            var secondClone = firstClone.Copy();

            while (true)
            {
                firstClone = secondClone.Copy();
                for (var i = 0; i < firstClone.GetLengthX; i++)
                    for (var j = 0; j < firstClone.GetLengthY; j++)
                    {
                        secondClone[i, j] = firstClone[i, j] switch
                        {
                            NeighborGrid.SeatState.Occupied when firstClone.AmountOccupied(i, j) >= 4 => NeighborGrid.SeatState.Empty,
                            NeighborGrid.SeatState.Empty when firstClone.AmountOccupied(i, j) == 0 => NeighborGrid.SeatState.Occupied,
                            _ => secondClone[i, j]
                        };
                    }
                // Fix-Point
                if (firstClone.Equals(secondClone))
                    break;
            }
            return secondClone.AmountOccupied().ToString();
        }

        protected override string SolvePartTwo()
        {
            var firstClone = this._seatGrid.Copy();
            var secondClone = firstClone.Copy();

            while (true)
            {
                firstClone = secondClone.Copy();
                for (var i = 0; i < firstClone.GetLengthX; i++)
                    for (var j = 0; j < firstClone.GetLengthY; j++)
                    {
                        secondClone[i, j] = firstClone[i, j] switch
                        {
                            NeighborGrid.SeatState.Occupied when firstClone.AmountOccupiedLineOfSight(i, j) >= 5 => NeighborGrid.SeatState.Empty,
                            NeighborGrid.SeatState.Empty when firstClone.AmountOccupiedLineOfSight(i, j) == 0 => NeighborGrid.SeatState.Occupied,
                            _ => secondClone[i, j]
                        };
                    }
                // Fix-Point
                if (firstClone.Equals(secondClone))
                    break;
            }
            return secondClone.AmountOccupied().ToString();
        }
    }

    /// <summary>
    /// Documented for future reference, (e.g. similar AoC question continuation or 'Minesweeper/Sudoku' like (text-based) games)
    /// </summary>
    internal class NeighborGrid
    {
        public enum SeatState { Floor, Empty, Occupied };

        private readonly SeatState[,] _grid;

        private readonly List<(int x, int y)> _neighbors = new List<(int x, int y)>
        {
            (0, -1),  // N,  One up
            (1, -1),  // NE  One right, one up
            (1, 0),   // E   One right
            (1, 1),   // SE  One right, one down
            (0, 1),   // S   One down
            (-1, 1),  // SW  One left, one down
            (-1, 0),  // W   One left
            (-1, -1), // NW  One left, one up
        };

        public int GetLengthX => _grid.GetLength(0);
        public int GetLengthY => _grid.GetLength(1);

        /// <summary>
        /// Constructor to create a new Grid object and parse the <paramref name="input"/> string
        /// </summary>
        /// <param name="input">String input, delimited by '\n', from the AoC 2020 Day 11 Input</param>
        public NeighborGrid(string input)
        {
            var splitInput = input.SplitByNewline();
            this._grid = new SeatState[splitInput.Length, splitInput[0].Length];

            for (var i = 0; i < splitInput.Length; i++)
                for (var j = 0; j < splitInput[i].Length; j++)
                {
                    if (splitInput[i][j] == 'L')
                        this._grid[i, j] = SeatState.Empty;
                }
        }

        /// <returns>Since the grid member-variable is private readonly, this functions returns it as a new Object, Essentially making a copy</returns>
        public NeighborGrid Copy()
        {
            return new NeighborGrid(this._grid);
        }

        /// <summary>
        /// Constructor to assign an exiting <paramref name="grid"/> to this grid object
        /// </summary>
        /// <param name="grid">An existing grid</param>
        public NeighborGrid(SeatState[,] grid)
        {
            this._grid = grid.Clone() as SeatState[,];
        }

        /// <summary>
        /// Getters and setters for the SeatState positions for a given (xPos, yPos) coordinate
        /// </summary>
        /// <param name="x">The xPos-coordinate</param>
        /// <param name="y">The yPos-coordinate</param>
        public SeatState this[int x, int y]
        {
            get => this._grid[x, y];
            set => this._grid[x, y] = value;
        }

        /// <summary>
        /// Checks whether the given position (xPos, yPos) is out of bounds for the _grid member variable
        /// </summary>
        /// <param name="x">the xPos-coordinate</param>
        /// <param name="y">the yPos-coordinate</param>
        /// <returns>True if within bounds, otherwise false</returns>
        private bool IsValidPosition(int x, int y)
        {
            return x >= 0 && x < this._grid.GetLength(0) && y >= 0 && y < this._grid.GetLength(1);
        }

        /// <summary>
        /// Flatten 2D enum array grid and count the amount of Occupied seats using LINQ
        /// source: https://stackoverflow.com/questions/641499/convert-2-dimensional-array
        /// </summary>
        /// <returns>Amount of seats occupied for the current state of the _grid member variable</returns>
        public long AmountOccupied()
        {
            return this._grid.Cast<SeatState>().Count(c => c == SeatState.Occupied);
        }

        /// <summary>
        /// For all valid positions, count the ones that are occupied for the neighbor positions
        /// (e.g. (0,1) + (100, 100) = (100, 101) --> grid[100,101] == SeatState.Occupied = 1, as IEnumerable int, sum those)
        /// </summary>
        /// <param name="xPos">The xPos-coordinate</param>
        /// <param name="yPos">The yPos-coordinate</param>
        /// <returns>The amount of all occupied spaces around xPos,yPos</returns>
        public int AmountOccupied(int xPos, int yPos)
        {
            return this._neighbors.Where(offSet => IsValidPosition(offSet.x + xPos, offSet.y + yPos))
                                  .Select(offSet => this._grid[offSet.x + xPos, offSet.y + yPos] == SeatState.Occupied ? 1 : 0)
                                  .Sum();
        }

        /// <summary>
        /// Checks the state of this _grid memberVariable against the _grid memberVariable of the <paramref name="otherGrid"/> by
        /// 1: Checking if the dimensions are equal to each other
        /// 2: All (flattened) values are equal to each other
        /// source: https://stackoverflow.com/questions/641499/convert-2-dimensional-array
        /// </summary>
        /// <param name="otherGrid">The Grid object to compare with</param>
        /// <returns>True if both grids are equal, otherwise false</returns>
        public bool Equals(NeighborGrid otherGrid)
        {
            return this.GetLengthX == otherGrid.GetLengthX && this.GetLengthY == otherGrid.GetLengthY && this._grid.Cast<int>().SequenceEqual(otherGrid._grid.Cast<int>());
        }

        /// <summary>
        /// For Part 2:
        /// Look for the first seat in each of those eight directions, starting with a multiplier of 1 (first 8-neighbors), 
        /// By multiplying the search area for a given <param name="xPos"/>, <param name="yPos"/> if no seat is found
        /// </summary>
        /// <param name="xPos">xPos-coordinate of the seatGrid</param>
        /// <param name="yPos">yPos-coordinate of the seatGrid</param>
        /// <returns>The amount of first neighbors</returns>
        public int AmountOccupiedLineOfSight(int xPos, int yPos)
        {
            var amountOfNeighbors = 0;
            foreach (var (lineSightOffsetX, lineSightOffsetY) in _neighbors)
            {
                var multiplier = 1;
                var currentSeat = SeatState.Floor;
                while (true)
                {
                    var multipliedLineSightX = lineSightOffsetX * multiplier + xPos;
                    var multipliedLineSightY = lineSightOffsetY * multiplier + yPos;

                    if (!IsValidPosition(multipliedLineSightX, multipliedLineSightY))
                        break;

                    currentSeat = this._grid[multipliedLineSightX, multipliedLineSightY];

                    // Until we reach our first seat
                    if (currentSeat != SeatState.Floor)
                        break;
                    
                    multiplier++;
                }
                amountOfNeighbors += currentSeat == SeatState.Occupied ? 1 : 0;
            }
            return amountOfNeighbors;
        }
    }
}
