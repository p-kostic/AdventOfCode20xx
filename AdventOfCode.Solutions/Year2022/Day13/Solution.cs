using System.Text.Json.Nodes;

namespace AdventOfCode.Solutions.Year2022.Day13;

internal class Solution : SolutionBase
{
    private readonly List<(JsonNode, JsonNode)> _parsedInputAsPairs;
    private readonly List<JsonNode> _parsedInputAsList;
    private readonly JsonNode _divider1;
    private readonly JsonNode _divider2;

    public Solution() : base(13, 2022, "Distress Signal")
    {
        // Part 1: Treat as pairs, add pairs to a List parsed as JsonNodes. 
        this._parsedInputAsPairs = new List<(JsonNode, JsonNode)>();
        foreach (string pairsAsSingleLine in this.Input.SplitByParagraph())
        {
            string[] pairs = pairsAsSingleLine.SplitByNewline();
            this._parsedInputAsPairs.Add((JsonNode.Parse(pairs[0]), JsonNode.Parse(pairs[1]))!);
        }

        // Part 2: Treat as single list items, disregard blank lines, add the dividers to the end.
        this._divider1 = JsonNode.Parse("[[2]]")!;
        this._divider2 = JsonNode.Parse("[[6]]")!;
        this._parsedInputAsList = this.Input.SplitByNewline(true)
                                            .Select(x => JsonNode.Parse(x))
                                            .Append(this._divider1)
                                            .Append(this._divider2)
                                            .ToList()!;
    }

    protected override string SolvePartOne()
    {
        var indicesInRightOrder = new List<int>();

        for (int i = 0; i < this._parsedInputAsPairs.Count; i++)
            if (Compare(this._parsedInputAsPairs[i].Item1, this._parsedInputAsPairs[i].Item2) < 0)
                indicesInRightOrder.Add(i + 1); // 1-based index

        return indicesInRightOrder.Sum().ToString();
    }

    protected override string SolvePartTwo()
    {
        this._parsedInputAsList.Sort(Compare);

        // 1-based index
        return ((this._parsedInputAsList.FindIndex(x => Compare(x, this._divider1) == 0) + 1) * 
                (this._parsedInputAsList.FindIndex(x => Compare(x, this._divider2) == 0) + 1)).ToString();
    }

    private static int Compare(JsonNode? nodeA, JsonNode? nodeB)
    {
        // Case both are integers
        if (nodeA is JsonValue && nodeB is JsonValue)
            return ((int)nodeA).CompareTo((int)nodeB);

        // Case both should be treated as arrays, or one should be converted to an array.
        var arrayA = nodeA as JsonArray ?? new JsonArray((int)(nodeA ?? throw new ArgumentNullException(nameof(nodeA)))); // ReSharper's suggestion
        var arrayB = nodeB as JsonArray ?? new JsonArray((int)(nodeB ?? throw new ArgumentNullException(nameof(nodeB)))); // ReSharper's suggestion

        // Use the first non-zero result from comparing the corresponding elements in the two arrays
        // or return the result of comparing the two array lengths if all elements compare as equal
        foreach (var (p1, second) in arrayA.Zip(arrayB))
        {
            int comparison = Compare(p1, second);
            if (comparison != 0)
                return comparison;
        }
        return arrayA.Count.CompareTo(arrayB.Count);
    }
}
