using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2020.Day23
{
    internal class Solution : SolutionBase
    {
        private readonly LinkedList<int> _cupsP1;
        private readonly LinkedList<int> _cupsP2;

        public Solution() : base(23, 2020, "Crab Cups")
        {
            // Part 1
            this._cupsP1 = new LinkedList<int>();
            foreach (var val in this.Input.TakeWhile(char.IsDigit).Select(c => (int)char.GetNumericValue(c)))
                this._cupsP1.AddLast(val);

            // Part 2
            this._cupsP2 = new LinkedList<int>(this._cupsP1);
            this._cupsP2.AddRange(Enumerable.Range(10, 999991));
        }

        protected override string SolvePartOne()
        {
            var cup1 = Game(this._cupsP1, 100);
            return string.Join(string.Empty, cup1.TakeToList(8).Select(x => x.Value.ToString()));
        }

        protected override string SolvePartTwo()
        {
            var cup1Result = Game(this._cupsP2, 10000000);
            return (cup1Result.NextOrFirst().Value * (long)cup1Result.TakeNext(2).Value).ToString();
        }

        private static LinkedListNode<int> Game(LinkedList<int> cups, int amountOfMoves)
        {
            // Speed up search by keeping an dic of value -> node
            var cupIndices = new Dictionary<int, LinkedListNode<int>>();
            var cupIndex = cups.First;
            while (cupIndex != null)
            {
                cupIndices.Add(cupIndex.Value, cupIndex);
                cupIndex = cupIndex.Next;
            }

            // Start 
            var curCup = cups.First;
            for (var i = 0; i < amountOfMoves; i++)
            {
                var pickUp = curCup.TakeToList(3);

                foreach (var pick in pickUp)
                    cups.Remove(pick);

                var destCup = curCup.Value - 1;
                while (destCup < 1 || destCup == curCup.Value || pickUp.Any(p => p.Value == destCup)) // order?
                {
                    destCup--;
                    if (destCup >= 1) 
                        continue;
                    destCup = cupIndices.Count();
                }
                curCup = curCup.NextOrFirst();

                var target = cupIndices[destCup];
                foreach (var pick in pickUp)
                {
                    cups.AddAfter(target, pick);
                    target = target.NextOrFirst();
                }
            }
            return cupIndices[1];
        }
    }

    /// <summary>
    /// source: https://stackoverflow.com/questions/716256/creating-a-circularly-linked-list-in-c
    /// </summary>
    internal static class CircularLinkedList
    {
        public static LinkedListNode<T> NextOrFirst<T>(this LinkedListNode<T> current)
        {
            return current?.List != null ? current.Next ?? current.List.First : null;
        }

        //public static LinkedListNode<T> PreviousOrLast<T>(this LinkedListNode<T> current)
        //{
        //    return current?.List != null ? current.Previous ?? current.List.Last : null;
        //}

        public static LinkedListNode<T> TakeNext<T>(this LinkedListNode<T> current, int amount)
        {
            for (var i = 0; i < amount; i++)
                current = current.NextOrFirst();
            return current;
        }

        public static List<LinkedListNode<T>> TakeToList<T>(this LinkedListNode<T> current, int amount, bool includeCurrent = false)
        {
            var result = new List<LinkedListNode<T>>();
            if (includeCurrent)
                result.Add(current);

            Enumerable.Range(0, amount).ForEach(_ =>
            {
                current = current.NextOrFirst();
                result.Add(current);
            });

            return result;
        }

        public static void AddRange<T>(this LinkedList<T> destination, IEnumerable<T> source)
        {
            foreach (var item in source)
                destination.AddLast(item);
        }
    }
}
