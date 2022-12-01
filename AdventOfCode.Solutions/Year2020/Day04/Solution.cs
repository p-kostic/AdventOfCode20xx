using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions.Year2020
{

    class Day04 : SolutionBase
    {
        private readonly List<string> parsedInput;

        List<Dictionary<string, string>> passportList;
        List<Dictionary<string, string>> validPassPortList;


        /// <summary>
        /// O(N * L) where L is the maximum number of key-value pairs
        /// </summary>
        public Day04() : base(04, 2020, "Passport Processing")
        {
            parsedInput = Input.Split("\n\n")
                               .Select(s => s.Replace('\n', ' '))
                               .ToList();

            passportList = new List<Dictionary<string, string>>();
            validPassPortList = new List<Dictionary<string, string>>();


            foreach (var line in parsedInput)
            {
                var passportDic = new Dictionary<string, string>();
                var keyValuePairs = line.Split(' ');

                foreach (var kvp in keyValuePairs)
                {
                    var splitKvp = kvp.Split(':');
                    passportDic.Add(splitKvp[0], splitKvp[1]);
                }
                passportList.Add(passportDic);
            }
        }

        /// <summary>
        /// O(N * L) where L is the maximum number of key-value pairs
        /// </summary>
        protected override string SolvePartOne()
        {
            int validCounter = 0;

            foreach (var passportDic in passportList)
            {
                bool valid = true;
                string[] requireds = new string[7] { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" };
                foreach (var item in requireds)
                    if (!passportDic.ContainsKey(item))
                        valid = false;
                if (valid)
                {
                    validCounter++;
                    validPassPortList.Add(passportDic);
                }
            }
            return validCounter.ToString();
        }

        /// <summary>
        /// Switch-case strategy where the validPassport boolean for each passport can only be assigned to false 
        /// after initially assigning it to true for each valid passport
        /// 
        /// O(N * L) where L is the maximum number of key-value pairs
        /// </summary>
        protected override string SolvePartTwo()
        {
            int validCounter = 0;
            foreach (Dictionary<string, string> passport in validPassPortList)
            {
                bool validPassport = true;

                if (!validPassport)
                    break;

                foreach (string key in passport.Keys)
                {
                    int value;
                    switch (key)
                    {
                        case "byr":
                            value = int.Parse(passport[key]);
                            if (value < 1920 || value > 2002) 
                                validPassport = false;
                            break;
                        case "iyr":
                            value = int.Parse(passport[key]);
                            if (value < 2010 || value > 2020) 
                                validPassport = false;
                            break;
                        case "eyr":
                            value = int.Parse(passport[key]);
                            if (value < 2020 || value > 2030) 
                                validPassport = false;
                            break;
                        case "hgt":
                            string inchOrCm = passport[key][^2..];

                            if (inchOrCm == "in")
                            {
                                if (!int.TryParse(passport[key].TrimEnd(new char[] { 'i', 'n' }), out int height))
                                    validPassport = false;
                                if (height < 59 || height > 76)
                                    validPassport = false;
                            }
                            else if (inchOrCm == "cm")
                            {
                                if (!int.TryParse(passport[key].TrimEnd(new char[] { 'c', 'm' }), out int height))
                                    validPassport = false;
                                if (height < 150 || height > 193)
                                    validPassport = false;
                            }
                            else
                                validPassport = false;
                            break;
                        case "hcl":
                            if (!Regex.IsMatch(passport[key], "^#[0-9a-f]{6}$"))
                                validPassport = false;
                            break;
                        case "ecl":
                            if (passport[key].Length != 3)
                                validPassport = false;
                            else if (!"amb blu brn gry grn hzl oth".Contains(passport[key]))
                                validPassport = false;
                            break;
                        case "pid":
                            if (passport[key].Length != 9)
                                validPassport = false;
                            if (!int.TryParse(passport[key], out int l))
                                validPassport = false;
                            break;
                    }
                }
                if (validPassport)
                    validCounter++;
            }
            return validCounter.ToString();
        }
    }
}

