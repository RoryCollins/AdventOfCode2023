namespace Day07;

internal class Hand : IComparable<Hand>
{
    private const string Cards = "J23456789TQKA";
    private readonly string hand;

    public Hand(string hand)
    {
        this.hand = hand;
    }

    public int CompareTo(Hand? other)
    {
        if (Type() != other!.Type()) return Type().CompareTo(other.Type());

        for (var i = 0; i < hand.Length; i++)
        {
            var thisIndex = Cards.IndexOf(hand[i]);
            var otherIndex = Cards.IndexOf(other.hand[i]);
            if (thisIndex == otherIndex) continue;
            return thisIndex.CompareTo(otherIndex);
        }

        return 0;

    }

    private HandType Type()
    {
        if (hand.Equals("JJJJJ")) return HandType.FiveOfAKind;

        var bestJokerSubstitute = hand
            .Replace("J", "")
            .GroupBy(c => c)
            .OrderByDescending(it => it.Count())
            .ThenByDescending(it => Cards.IndexOf(it.Key))
            .First()
            .Key;

        var newHand = hand.Replace('J', bestJokerSubstitute)
            .GroupBy(c => c)
            .OrderByDescending(it => it.Count())
            .ThenByDescending(it => Cards.IndexOf(it.Key))
            .ToList();


        if (newHand[0]
                .Count() == 5) return HandType.FiveOfAKind;
        if (newHand[0]
                .Count() == 4) return HandType.FourOfAKind;
        if (newHand[0]
                .Count() == 3)
            return newHand[1]
                .Count() == 2
                ? HandType.FullHouse
                : HandType.ThreeOfAKind;
        if (newHand[0]
                .Count() == 2)
            return newHand[1]
                .Count() == 2
                ? HandType.TwoPair
                : HandType.OnePair;
        return HandType.HighCard;
    }
}