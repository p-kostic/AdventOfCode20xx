namespace AdventOfCode.Solutions.Year2022.Day08;

internal class Solution : SolutionBase
{
    private readonly int[][] _trees;

    public Solution() : base(08, 2022, "Treetop Tree House")
    {
        this._trees = this.Input.SplitByNewline()
                                .Select(s => s.Select(c => (int)char.GetNumericValue(c)).ToArray())
                                .ToArray();
    }

    protected override string SolvePartOne()
    {
        int visible = 0;
        for (int i = 0; i < this._trees.Length; i++)
            for (int j = 0; j < this._trees[i].Length; j++)
            {
                if (IsVisible(j, i, -1,  0, this._trees[i][j]) ||
                    IsVisible(j, i,  1,  0, this._trees[i][j]) ||
                    IsVisible(j, i,  0, -1, this._trees[i][j]) ||
                    IsVisible(j, i,  0,  1, this._trees[i][j]))
                    visible++;
            }

        return visible.ToString();
    }
    
    private bool IsVisible(int x, int y, int xOffSet, int yOffSet, int curTreeValue)
    {
        while (true)
        {
            if (x == 0 || x == this._trees.Length - 1 || y == 0 || y == this._trees.Length - 1)
                return true;
            if (curTreeValue <= this._trees[y + yOffSet][x + xOffSet])
                return false;

            x += xOffSet;
            y += yOffSet;
        }
    }

    protected override string SolvePartTwo()
    {
        var scores = new List<int>();

        for (int i = 0; i < this._trees.Length; i++)
            for (int j = 0; j < this._trees[i].Length; j++)
            {
                var col = this._trees.Select(t => t[j]).ToList();

                int left = this._trees[i].Take(j).Reverse().TakeWhile(t => t < this._trees[i][j]).Count();
                int right = this._trees[i].Skip(j + 1).TakeWhile(t => t < this._trees[i][j]).Count();
                int up = col.Take(i).Reverse().TakeWhile(t => t < this._trees[i][j]).Count();
                int down = col.Skip(i + 1).TakeWhile(t => t < this._trees[i][j]).Count() + 1;

                scores.Add(left * right * up * down);
            }

        return scores.Max().ToString();
    }

}
