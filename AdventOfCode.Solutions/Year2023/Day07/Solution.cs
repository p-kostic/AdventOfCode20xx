namespace AdventOfCode.Solutions.Year2023.Day07;

internal class Solution : SolutionBase
{
    private readonly List<Hand> _handsPart1;
    private readonly List<Hand> _handsPart2;

    public Solution() : base(07, 2023, "Camel Cards")
    {
        string[] lines = this.Input.SplitByNewline(true);

        this._handsPart1 = new List<Hand>();
        this._handsPart2 = new List<Hand>();
        foreach (string line in lines)
        {
            string[] parts = line.Split(' ');
            this._handsPart1.Add(new Hand(parts[0], int.Parse(parts[1]), false));
            this._handsPart2.Add(new Hand(parts[0], int.Parse(parts[1]), true));
        }
    }

    protected override string SolvePartOne()
    {
        this._handsPart1.Sort(new HandComparer());
        return this._handsPart1.Select((t, i) => t.Bid * (i + 1)).Sum().ToString();
    }

    protected override string SolvePartTwo()
    {
        this._handsPart2.Sort(new HandComparer());
        return this._handsPart2.Select((t, i) => t.Bid * (i + 1)).Sum().ToString();
    }
}

public enum HandType
{
    HighCard = 6,
    OnePair = 5,
    TwoPairs = 4,
    ThreeOfAKind = 3,
    FullHouse = 2,
    FourOfAKind = 1,
    FiveOfAKind = 0
}

public class HandComparer : Comparer<Hand>
{
    public override int Compare(Hand? x, Hand? y)
    {
        if (x == null || y == null)
            throw new ArgumentNullException();

        if (x.Type != y.Type)
            return y.Type.CompareTo(x.Type);

        // Compare cards
        for (int i = 0; i < 4; i++)
        {
            if (x.Cards[i] != y.Cards[i])
                return x.Cards[i].CompareTo(y.Cards[i]);
        }

        // Compare the last card
        return x.Cards[4].CompareTo(y.Cards[4]);
    }
}

public record Hand
{
    public HandType Type { get; init; }
    public int Bid { get; init; }
    public List<int> Cards { get; init; } = new();

    public Hand(string hand, int bid, bool partTwo)
    {
        this.Bid = bid;

        foreach (char card in hand)
        {
            switch (card)
            {
                case 'A':
                    this.Cards.Add(14);
                    break;
                case 'K':
                    this.Cards.Add(13);
                    break;
                case 'Q':
                    this.Cards.Add(12);
                    break;
                case 'J':
                    this.Cards.Add(partTwo ? 1 : 11);
                    break;
                case 'T':
                    this.Cards.Add(10);
                    break;
                default:
                    this.Cards.Add(int.Parse(card.ToString()));
                    break;
            }
        }
        
        var cardCount = CardCount(this.Cards);

        this.Type = partTwo ? DetermineHandTypePartTwo(cardCount) : DetermineHandTypePartOne(cardCount);
    }

    private static HandType DetermineHandTypePartOne(Dictionary<int, int> cardCount)
    {
        if (cardCount.Values.Any(x => x == 5))
            return HandType.FiveOfAKind;
        if (cardCount.Values.Any(x => x == 4))
            return HandType.FourOfAKind;
        if (cardCount.Values.Any(x => x == 3) && cardCount.Values.Any(x => x == 2))
            return HandType.FullHouse;
        if (cardCount.Values.Any(x => x == 3))
            return HandType.ThreeOfAKind;
        if (cardCount.Values.Count(x => x == 2) == 2)
            return HandType.TwoPairs;
        return cardCount.Values.Any(x => x == 2) ? HandType.OnePair : HandType.HighCard;
    }

    private static Dictionary<int, int> CardCount(List<int> cards)
    {
        var cardCount = new Dictionary<int, int>();
        foreach (int card in cards)
        {
            if (cardCount.TryGetValue(card, out int value))
                cardCount[card] = ++value;
            else
                cardCount.Add(card, 1);
        }

        return cardCount;
    }

    private static HandType DetermineHandTypePartTwo(Dictionary<int, int> cardCount)
    {
        var handType = DetermineHandTypePartOne(cardCount);

        if (!cardCount.TryGetValue(1, out int count)) 
            return handType;

        handType = handType switch
        {
            HandType.FourOfAKind => HandType.FiveOfAKind,
            HandType.FullHouse when count == 1 => HandType.FourOfAKind,
            HandType.FullHouse => HandType.FiveOfAKind,
            HandType.ThreeOfAKind => HandType.FourOfAKind,
            HandType.TwoPairs when count == 1 => HandType.FullHouse,
            HandType.TwoPairs => HandType.FourOfAKind,
            HandType.OnePair => HandType.ThreeOfAKind,
            HandType.HighCard => HandType.OnePair,
            _ => handType
        };
        return handType;
    }
}
