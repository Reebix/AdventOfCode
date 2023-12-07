using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.days;

public class Day7 : Day
{
    private static string[] _labelArray = { "A", "K", "Q", "J", "T", "9", "8", "7", "6", "5", "4", "3", "2" };

    public Day7() : base(7)
    {
    }

    protected override void Run(bool isPart2 = false)
    {
        if(isPart2) _labelArray = new []{"A", "K", "Q", "T", "9", "8", "7", "6", "5", "4", "3", "2", "J" };
        
        List<Hand> hands = new();
        foreach (var s in Input)
        {
            var hand = s.Split(' ')[0];
            var bid = long.Parse(s.Split(' ')[1]);
            hands.Add(new Hand { Cards = hand, Bid = bid });
        }

        // sort and reverse
        hands.Sort();
        hands.Reverse();

        long totalValue = 0;
        if (!isPart2)
            if (!File.Exists("test.txt"))
                File.Create("test.txt");

        for (var i = 0; i < hands.Count; i++)
        {
            var hand = hands[i];
            var multiplier = i + 1;
            totalValue += hand.Bid * multiplier;
        }

        File.WriteAllText("test.txt", string.Join("\n", hands.Select(h => $"{h.Cards} {h.Bid}")));
        // 250137961 is too high
        Console.WriteLine(totalValue);
    }

    private static bool IsHandStronger(Hand original, Hand comparator)
    {
        return IsHandStronger(original.Cards, comparator.Cards);
    }

    private static bool IsHandStronger(string original, string comparator)
    {
        var hand1Type = GetHandType(original);
        var hand2Type = GetHandType(comparator);
        if (hand1Type > hand2Type) return true;
        if (hand1Type < hand2Type) return false;

       

        var hand1Array = original.ToCharArray();
        var hand2Array = comparator.ToCharArray();
        var hand1ArrayLength = hand1Array.Length;
        
        // check if hand ist like 2266A > 22A6T
        if (hand1Type == Type.TwoPair)
        {
            var charAmountDict = new Dictionary<char, int>();
            var hand2CharAmountDict = new Dictionary<char, int>();
            
            foreach (var c in hand1Array)
            {
                if (charAmountDict.ContainsKey(c))
                {
                    charAmountDict[c]++;
                }
                else
                {
                    charAmountDict.Add(c, 1);
                }
            }
            
            foreach (var c in hand2Array)
            {
                if (hand2CharAmountDict.ContainsKey(c))
                {
                    hand2CharAmountDict[c]++;
                }
                else
                {
                    hand2CharAmountDict.Add(c, 1);
                }
            }
            
            // check for 2 pairs with multiple cards
            if (charAmountDict.Count == 3 && hand2CharAmountDict.Count != 3)
            {
                return false;
            }
            
            // check for 2 pairs with multiple cards
            if (charAmountDict.Count != 3 && hand2CharAmountDict.Count == 3)
            {
                return true;
            }
        }
        
        for (var i = 0; i < hand1ArrayLength; i++)
        {
            var hand1Index = Array.IndexOf(_labelArray, hand1Array[i].ToString());
            var hand2Index = Array.IndexOf(_labelArray, hand2Array[i].ToString());
            if (hand1Index > hand2Index) return true;
            if (hand1Index < hand2Index) return false;
        }

        return false;
    }

    private static Type GetHandType(string hand)
    {
        var handType = Type.HighCard;
        var handArray = hand.ToCharArray();
        // check five of a kind
        if (GetAmountOf(hand, handArray[0]) == 5) return Type.FiveOfAKind;

        // check four of a kind
        if (handArray.Any(c => GetAmountOf(hand, c) == 4))
            return Type.FourOfAKind;
        // check full house
        if (handArray.Any(c => GetAmountOf(hand, c) == 3) &&
            handArray.Any(c => GetAmountOf(hand, c) == 2))
            return Type.FullHouse;
        // check three of a kind
        if (handArray.Any(c => GetAmountOf(hand, c) == 3))
            return Type.ThreeOfAKind;
        // check two pair
        if (handArray.Any(c => GetAmountOf(hand, c) == 2) &&
            handArray.Any(c => GetAmountOf(hand, c) == 2))
            return Type.TwoPair;
        // check one pair
        if (handArray.Any(c => GetAmountOf(hand, c) == 2))
            return Type.OnePair;


        // check four of a kind


        return handType;
    }

    private static int GetAmountOf(string hand, char card)
    {
        var amount = 0;
        foreach (var c in hand)
            if (c == card)
                amount++;

        return amount;
    }

    private struct Hand : IComparable<Hand>
    {
        public string Cards;
        public long Bid;

        public int CompareTo(Hand other)
        {
            // Assuming IsHandStronger is a method in the same class as Hand
            if (IsHandStronger(this, other))
                return 1;
            if (IsHandStronger(other, this))
                return -1;
            return 0;
        }
    }

    private enum Type
    {
        FiveOfAKind,
        FourOfAKind,
        FullHouse,
        ThreeOfAKind,
        TwoPair,
        OnePair,
        HighCard
    }
}