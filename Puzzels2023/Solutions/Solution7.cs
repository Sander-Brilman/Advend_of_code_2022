global using System.Diagnostics;

namespace Puzzels2023.Solutions;
internal class Solution7(string path) : SolutionBase(path)
{
    private enum HandType
    {
        HighCard = 1,
        OnePair = 2,
        TwoPair = 3,
        ThreeOfAKind = 4,
        FullHouse = 5,
        FourOfAKind = 6,
        FiveOfAKind = 7,
    }

    private record Card(char DisplayValue, int Value)
    {
        public static Card FromChar(char displayValue, bool withJoker = false) => new(displayValue, displayValue switch
            {
                'A' => 14,
                'K' => 13,
                'Q' => 12,
                'J' => withJoker ? 1 : 11,
                'T' => 10,
                '9' => 9,
                '8' => 8,
                '7' => 7,
                '6' => 6,
                '5' => 5,
                '4' => 4,
                '3' => 3,
                '2' => 2,
                _ => throw new UnreachableException(),
            });
    }

    private record Hand(HandType Type, Card[] Cards, int Bid, string AsText)
    {
        private static HandType GetHandTypeFromCards(Card[] cards, bool countJoker = false)
        {
            if (countJoker is false || cards.Any(c => c.DisplayValue == 'J') is false)
            {
                int distictCount = cards
                    .DistinctBy(x => x.Value)
                    .Count();

                switch (distictCount)
                {
                    case 5: return HandType.HighCard;
                    case 4: return HandType.OnePair;
                    case 1: return HandType.FiveOfAKind;
                }

                var groupCount = cards
                    .GroupBy(x => x.Value)
                    .Select(x => x.Count())
                    .OrderDescending()
                    .ToArray();

                return groupCount switch
                {
                    [4, 1] => HandType.FourOfAKind,
                    [3, 1, 1] => HandType.ThreeOfAKind,
                    [2, 2, 1] => HandType.TwoPair,
                    [3, 2] => HandType.FullHouse,
                    _ => throw new UnreachableException()
                };
            }

            int amountOfJokers = cards
                .Where(c => c.DisplayValue == 'J')
                .Count();

            int distictCardCount = cards
                .DistinctBy(x => x.Value)
                .Count() ;

            switch (distictCardCount)
            {
                case 5: return HandType.OnePair; // one pair due to joker
                case 4: return HandType.ThreeOfAKind;

                case 2: return HandType.FiveOfAKind;// due to joker
                case 1: return HandType.FiveOfAKind;
            }

            var groupsWithoutJokerCount = cards
                .Where(x => x.DisplayValue != 'J')
                .GroupBy(x => x.Value)
                .Select(x => x.Count())
                .OrderDescending()
                .ToArray();

            // max 4
            return groupsWithoutJokerCount switch
            {
                [2, 2] => HandType.FullHouse,

                [1, 1] => HandType.FourOfAKind,
                [2, 1] => HandType.FourOfAKind,
                [3, 1] => HandType.FourOfAKind,
                _ => throw new UnreachableException()
            };

        }

        public static Hand FromString(string str, bool countJoker = false)
        {
            string[] rawParts = str.Split(" ");

            int bid = int.Parse(rawParts[1]);
            Card[] cards = rawParts[0]
                .ToCharArray()
                .Select(c => Card.FromChar(c, countJoker))
                .ToArray();

            HandType type = GetHandTypeFromCards(cards, countJoker);

            return new Hand(type, cards, bid, rawParts[0]);
        }
    }

    private Hand[] GetHands(bool countJokers = false)
    {
        return _lines
            .Select(l => Hand.FromString(l, countJokers))
            .ToArray();
    }

    public override string GetFirstSolution()
    {
        var result = GetHands()
                .OrderBy(h => h.Type)
                .ThenBy(h => h.Cards[0].Value)
                    .ThenBy(h => h.Cards[1].Value)
                        .ThenBy(h => h.Cards[2].Value)
                            .ThenBy(h => h.Cards[3].Value)
                                .ThenBy(h => h.Cards[4].Value)
            .Select((h, index) => (h.Bid * (index + 1)))
            .Sum();

        return result.ToString();
    }

    public override string GetSecondSolution2()
    {
        var result = GetHands(true)
                .OrderBy(h => h.Type)
                .ThenBy(h => h.Cards[0].Value)
                    .ThenBy(h => h.Cards[1].Value)
                        .ThenBy(h => h.Cards[2].Value)
                            .ThenBy(h => h.Cards[3].Value)
                                .ThenBy(h => h.Cards[4].Value)
            .Select((h, index) => (h.Bid * (index + 1)))
            .Sum();

        return result.ToString();
    }
}
