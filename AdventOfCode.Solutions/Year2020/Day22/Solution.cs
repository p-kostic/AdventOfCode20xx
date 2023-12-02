using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2020.Day22
{
    internal class Solution : SolutionBase
    {
        private readonly List<int> _deck1;
        private readonly List<int> _deck2;

        private readonly Queue<int> _deck1Queue;
        private readonly Queue<int> _deck2Queue;

        public Solution() : base(22, 2020, "Crab Combat")
        {
            // Part 1
            var decks = this.Input.SplitByParagraph(true);
            this._deck1 = decks[0].SplitByNewline(true).Skip(1).Select(int.Parse).ToList();
            this._deck2 = decks[1].SplitByNewline(true).Skip(1).Select(int.Parse).ToList();

            // Part 2
            this._deck1Queue = new Queue<int>();
            this._deck2Queue = new Queue<int>();
            decks[0].Split('\n').Skip(1).Select(int.Parse).ForEach(x => this._deck1Queue.Enqueue(x));
            decks[1].Split('\n').Skip(1).Select(int.Parse).ForEach(x => this._deck2Queue.Enqueue(x));
        }

        protected override string SolvePartOne()
        {
            while (this._deck1.Count != 0 && this._deck2.Count != 0)
            {
                if (this._deck1[0] > this._deck2[0])
                {
                    var losingCard = this._deck2[0];
                    var winningCard = this._deck1[0];
                    this._deck1.RemoveAt(0);
                    this._deck2.RemoveAt(0);
                    this._deck1.Add(winningCard);
                    this._deck1.Add(losingCard);
                }
                else
                {
                    var losingCard = this._deck1[0];
                    var winningCard = this._deck2[0];
                    this._deck1.RemoveAt(0);
                    this._deck2.RemoveAt(0);
                    this._deck2.Add(winningCard);
                    this._deck2.Add(losingCard);
                }
            }

            var winner = this._deck1.Count != 0 ? this._deck1 : this._deck2;
            winner.Reverse();

            // Calculate Score
            var count = 0;
            long result = 0;
            foreach (var card in winner)
            {
                count++;
                result += card * count;
            }
            return result.ToString();
        }

        // Note: Didn't bother to change Part 1 with the Queue Data structure I introduced in Part 2
        protected override string SolvePartTwo()
        {
            RecursiveCombat(this._deck1Queue, this._deck2Queue, out var score);
            return score.ToString();
        }

        public static bool RecursiveCombat(Queue<int> deck1, Queue<int> deck2, out long score)
        {
            var currentScores = (CalculateScoreQueue(new Queue<int>(deck1)), CalculateScoreQueue(new Queue<int>(deck2)));
            var previousStates = new HashSet<(long p1, long p2)> {currentScores};

            do
            {
                previousStates.Add(currentScores);

                bool winner;
                var p1 = deck1.Dequeue();
                var p2 = deck2.Dequeue();

                if (p1 <= deck1.Count && p2 <= deck2.Count)
                    winner = RecursiveCombat(new Queue<int>(deck1.Take(p1)), new Queue<int>(deck2.Take(p2)), out _);
                else if (p1 > p2)
                    winner = true;
                else
                    winner = false;
                
                if (winner)
                {
                    deck1.Enqueue(p1);
                    deck1.Enqueue(p2);
                }
                else
                {
                    deck2.Enqueue(p2);
                    deck2.Enqueue(p1);
                }
                currentScores = (CalculateScoreQueue(new Queue<int>(deck1)), CalculateScoreQueue(new Queue<int>(deck2)));
            } 
            while (deck1.Count > 0 && deck2.Count > 0 && !previousStates.Contains(currentScores)); // Order matters

            if (deck1.Count == 0)
            {
                score = CalculateScoreQueue(new Queue<int>(deck2));
                return false;
            }
            score = CalculateScoreQueue(new Queue<int>(deck1));
            return true;
        }

        private static int CalculateScoreQueue(Queue<int> deck)
        {
            var sum = 0;
            while (deck.Count > 0)
                sum += deck.Dequeue() * (deck.Count + 1);
            return sum;
        }
    }
}