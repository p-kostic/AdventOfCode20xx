namespace AdventOfCode.Solutions.Year2023.Day04;

internal class Solution : SolutionBase
{
    private readonly List<int> _matchesPerCard;
    
    public Solution() : base(04, 2023, "Gear Ratios")
    {
        var parsedInput = Input.SplitByNewline(true);
        this._matchesPerCard = new List<int>();
        
        foreach (var line in parsedInput)
        {
            var gameAndNumbers = line.Split(':');
            var winningNumbersAndMyCard = gameAndNumbers[1].Split('|');
            var winningNumbers = winningNumbersAndMyCard[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
            var myCards = winningNumbersAndMyCard[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
            this._matchesPerCard.Add(winningNumbers.Intersect(myCards).ToArray().Length);
        }
    }
    
    protected override string SolvePartOne()
    {
        var result = 0;
        foreach (var matchCountPerCard in this._matchesPerCard)
        {
            switch (matchCountPerCard)
            {
                case 1:
                    result += matchCountPerCard;
                    break;
                case > 1:
                    result += (int)Math.Pow(2, matchCountPerCard - 1);
                    break;
            }
        }
        return result.ToString();
    }
 
    protected override string SolvePartTwo()
    {
        var copyAmountPerCard = new List<int>();
        var copyIndex = 1;

        do
        {
            if (copyAmountPerCard.Count < copyIndex)
                copyAmountPerCard.Add(1);
            else
                copyAmountPerCard[copyIndex - 1]++;

            var currentCardCopies = copyAmountPerCard[copyIndex - 1];

            // Distribute copies to subsequent cards in the range [copyIndex + 1, copyIndex + _matchesPerCard[copyIndex - 1]]
            for (var cardNumber = copyIndex + 1; cardNumber <= copyIndex + _matchesPerCard[copyIndex - 1]; cardNumber++)
            {
                if (cardNumber > copyAmountPerCard.Count)
                    copyAmountPerCard.Add(currentCardCopies);
                else
                    copyAmountPerCard[cardNumber - 1] += currentCardCopies;
            }
            
            // Next card
            copyIndex++;

        } while (copyIndex <= _matchesPerCard.Count);

        return copyAmountPerCard.Sum().ToString();
    }
}