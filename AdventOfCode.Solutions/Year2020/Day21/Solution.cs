using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions.Year2020.Day21
{
    internal class Solution : SolutionBase
    {
        private HashSet<string> _ingredients;
        private HashSet<string> _allergens;
        private readonly HashSet<(HashSet<string> ingredients, HashSet<string> allergens)> _foods;
        private List<string> _unknowns;

        public Solution() : base(21, 2020, "Allergen Assessment")
        {
            this._ingredients = new HashSet<string>();
            this._allergens = new HashSet<string>();
            this._foods = new HashSet<(HashSet<string>, HashSet<string>)>();

            foreach (var foodInputLine in this.Input.SplitByNewline())
            {
                var ingredients = Regex.Match(foodInputLine, @"([\w ]+) \(").Groups[1].Value.Split(' ');
                var allergens = Regex.Match(foodInputLine, @"\(contains ([\w, ]+)\)").Groups[1].Value.Split(", ");

                this._foods.Add((ingredients.ToHashSet(), ingredients.ToHashSet()));
                foreach (var ingredient in ingredients)
                    this._ingredients.Add(ingredient);
                foreach (var allergen in allergens)
                    this._allergens.Add(allergen);
            }
        }

        /// <summary>
        /// For each ingredient, find out all the possible allergens by invalidating it if any of the possible foods contains the allergy
        /// </summary>
        protected override string SolvePartOne()
        {
            this._unknowns = (this._ingredients.Select(ingredient => new
                {
                    ingredient,
                    possibleAllergens = this._foods.Where(x => x.ingredients.Contains(ingredient)).SelectMany(x => x.allergens)
                })
                .Where(t => t.possibleAllergens.All(x => this._foods.Any(y => !y.ingredients.Contains(t.ingredient) && y.allergens.Contains(x))))
                .Select(x => x.ingredient))
                .ToList();

            return this._unknowns.Sum(unknown => this._foods.Count(x => x.ingredients.Contains(unknown))).ToString();
        }

        /// <summary>
        /// Take the list of unknowns from Part 1 and shrink down our list of ingredients: Now we have a list of ingredients that have a possible allergen
        /// If we come across an allergen that had one possible ingredient, add it to our 'foundAllergens' Dictionary and remove it from our list of allergens and ingredients
        /// This is determined by looking at all the foods that share allergens. Then for each, get the ingredients that is in all these foods. Get when length == 1
        /// Loop through the allergens in a do-while for multiple possible ingredients.
        /// </summary>
        protected override string SolvePartTwo()
        {
            this._ingredients = this._ingredients.Where(x => !this._unknowns.Contains(x)).ToHashSet();

            var foundAllergens = new Dictionary<string, string>();
            do
            {
                this._allergens = this._allergens.Where(x => !foundAllergens.Keys.Contains(x)).ToHashSet();
                this._ingredients = this._ingredients.Where(x => !foundAllergens.Values.Contains(x)).ToHashSet();

                foreach (var allergen in this._allergens)
                {
                    var ingredients = this._ingredients.Where(x => this._foods.Where(x => x.allergens.Contains(allergen)).All(y => y.ingredients.Contains(x))).ToArray();
                    if (ingredients.Length == 1)
                        foundAllergens.Add(allergen, ingredients.First());
                }

            } while (this._allergens.Count > 0);

            return string.Join(",", foundAllergens.Keys.OrderBy(x => x).Select(x => foundAllergens[x]));
        }
    }
}
