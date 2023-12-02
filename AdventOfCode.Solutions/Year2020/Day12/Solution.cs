using System;

namespace AdventOfCode.Solutions.Year2020.Day12
{
    internal class Solution : SolutionBase
    {
        private readonly string[] _inputLines;

        private enum Direction
        {
            North = 0,
            East = 90,
            South = 180,
            West = 270
        };

        public Solution() : base(12, 2020, "Rain Risk")
        {
            _inputLines = Input.SplitByNewline();
        }

        protected override string SolvePartOne()
        {
            (int x, int y) currentPos = (0, 0);
            var currentDirection = Direction.East;

            foreach (var line in _inputLines)
            {
                var move = line[0];
                var amount = int.Parse(line.Substring(1));
                currentPos = move switch
                {
                    'N' => MoveDir(currentPos, Direction.North, amount),
                    'E' => MoveDir(currentPos, Direction.East, amount),
                    'S' => MoveDir(currentPos, Direction.South, amount),
                    'W' => MoveDir(currentPos, Direction.West, amount),
                    'F' => MoveDir(currentPos, currentDirection, amount),
                    _ => currentPos
                };
                currentDirection = move switch
                {
                    'L' => (Direction)(((int)currentDirection - amount + 360) % 360),
                    'R' => (Direction)(((int)currentDirection + amount) % 360),
                    _ => currentDirection
                };
            }

            return CalculationUtils.ManhattanDistance((0, 0), (currentPos)).ToString();
        }

        protected override string SolvePartTwo()
        {
            (int x, int y) currentPos = (0, 0);
            (int x, int y) wayPoint = (10, 1);

            foreach (var line in _inputLines)
            {
                var move = line[0];
                var amount = int.Parse(line.Substring(1));
                wayPoint = move switch
                {
                    'N' => MoveDir(wayPoint, Direction.North, amount),
                    'E' => MoveDir(wayPoint, Direction.East, amount),
                    'S' => MoveDir(wayPoint, Direction.South, amount),
                    'W' => MoveDir(wayPoint, Direction.West, amount),
                    'L' => amount switch
                    {
                        90 => (-wayPoint.y, wayPoint.x),
                        180 => (-wayPoint.x, -wayPoint.y),
                        270 => (wayPoint.y, -wayPoint.x),
                        _ => wayPoint
                    },
                    'R' => amount switch
                    {
                        270 => (-wayPoint.y, wayPoint.x),
                        180 => (-wayPoint.x, -wayPoint.y),
                        90 => (wayPoint.y, -wayPoint.x),
                        _ => wayPoint
                    },
                    _ => wayPoint
                };

                if (move == 'F')
                    currentPos = currentPos.Add((amount * wayPoint.x, amount * wayPoint.y));
            }

            return CalculationUtils.ManhattanDistance((0, 0), currentPos).ToString();
        }

        private static (int x, int y) MoveDir((int, int) pos, Direction dir, int amount)
        {
            return (dir) switch
            {
                Direction.North => pos.Add((0, amount)),
                Direction.East => pos.Add((amount, 0)),
                Direction.South => pos.Add((0, -amount)),
                Direction.West => pos.Add((-amount, 0)),
                _ => throw new ArgumentOutOfRangeException(nameof(dir), dir, "Invalid direction")
            };
        }
    }
}
