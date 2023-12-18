using System.Drawing;
using System.Globalization;

namespace AdventOfCode.Solutions.Year2023.Day18;

internal class Solution : SolutionBase
{
    private record Instruction(Direction Direction, int Amount, Color? Color);

    private record Vertex(long X, long Y);

    private enum Direction
    {
        North,
        East,
        South,
        West
    }

    private readonly List<Instruction> _instructions;
    private readonly List<Instruction> _instructionsPartTwo;

    public Solution() : base(18, 2023, "Lavaduct Lagoon")
    {
        string[] inputByLine = this.Input.SplitByNewline(true);
        this._instructions = new List<Instruction>();
        this._instructionsPartTwo = new List<Instruction>();

        foreach (string line in inputByLine)
        {
            string[] splitLine = line.Split(' ');

            // Part 1
            var directionPartOne = splitLine[0] switch
            {
                "U" => Direction.North,
                "R" => Direction.East,
                "D" => Direction.South,
                "L" => Direction.West,
            };
            int distanceInMPartOne = int.Parse(splitLine[1]);
            string colorHex = splitLine[2].TrimStart('(').TrimEnd(')');
            var color = ColorTranslator.FromHtml(colorHex);

            this._instructions.Add(new Instruction(directionPartOne, distanceInMPartOne, color));

            // Part 2
            int distanceInMPartTwo = int.Parse(colorHex[1..6], NumberStyles.HexNumber);
            var directionPartTwo = colorHex[6] switch
            {
                '0' => Direction.East,
                '1' => Direction.South,
                '2' => Direction.West,
                '3' => Direction.North,
            };

            this._instructionsPartTwo.Add(new Instruction(directionPartTwo, distanceInMPartTwo, null));
        }
    }

    protected override string SolvePartOne() => Solve(this._instructions);

    protected override string SolvePartTwo() => Solve(this._instructionsPartTwo);

    private static string Solve(List<Instruction> instructions)
    {
        var vertices = new List<Vertex>();
        var currentPos = new Vertex(0, 0);

        foreach (var instruction in instructions)
        {
            currentPos = instruction.Direction switch
            {
                Direction.North => currentPos with { Y = currentPos.Y - instruction.Amount },
                Direction.East => currentPos with { X = currentPos.X + instruction.Amount },
                Direction.South => currentPos with { Y = currentPos.Y + instruction.Amount },
                Direction.West => currentPos with { X = currentPos.X - instruction.Amount },
            };
            vertices.Add(currentPos);
        }

        return (CalculateArea(vertices) + CalculatePerimeter(vertices) / 2 + 1).ToString();
    }

    // Uses https://en.wikipedia.org/wiki/Shoelace_formula 
    private static long CalculateArea(IReadOnlyList<Vertex> vertices)
    {
        long area = 0;
        for (int i = 0; i < vertices.Count; i++)
        {
            int j = (i + 1) % vertices.Count; // Wrap around and connect to the first.
            area += vertices[i].X * vertices[j].Y - vertices[j].X * vertices[i].Y; // Cross product
        }
        return Math.Abs(area / 2);
    }

    private static long CalculatePerimeter(IReadOnlyList<Vertex> vertices)
    {
        long perimeter = 0;
        for (int i = 0; i < vertices.Count; i++)
        {
            // Sum the distance between consecutive "Vertices".
            int j = (i + 1) % vertices.Count; // Wrap around and connect to the first.
            perimeter += Math.Abs(vertices[i].X - vertices[j].X) + Math.Abs(vertices[i].Y - vertices[j].Y);
        }
        return perimeter;
    }
}