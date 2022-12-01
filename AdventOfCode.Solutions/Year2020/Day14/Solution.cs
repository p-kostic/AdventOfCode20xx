using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2020
{
    internal class Day14 : SolutionBase
    {
        private readonly string[] _splitInput;
        public Day14() : base(14, 2020, "Docking Data")
        {
            _splitInput = Input.SplitByNewline();
        }

        protected override string SolvePartOne()
        {
            var memory = new long[100000];
            long forcedMask = 0;
            long currentMask = 0;
            foreach (var line in _splitInput)
            {
                var splitLine = line.Split(new[] { ' ', '[', ']', '=' }, StringSplitOptions.RemoveEmptyEntries);
                switch (splitLine[0])
                {
                    case "mask":
                        currentMask = Convert.ToInt64(splitLine[1].Replace('X', '1'), 2);
                        forcedMask = Convert.ToInt64(splitLine[1].Replace('X', '0'), 2);
                        break;
                    default:
                        memory[int.Parse(splitLine[1])] = long.Parse(splitLine[2]) & currentMask | forcedMask;
                        break;
                }
            }
            return memory.Sum().ToString();
        }

        /// <summary>
        /// String/CharArray bit representations strategy: replace indices with those of the masks
        ///
        /// Although I didn't do this initially for part 1, I decided to parse the input to a more convenient format (also makes it a "bit" more readable)
        /// I, however, didn't want to bother with changing Part 1 accordingly, so parsing is done in the SolvePartTwo() method
        /// 
        /// I parse each mask with its corresponding instructions to 
        /// Key = Mask, Value = list of instructions where each list element is a tuple (long index, long value)
        /// </summary>
        protected override string SolvePartTwo()
        {
            var parsedInput = new Dictionary<string, List<(long index, long value)>>();
            var currentMask = "";
            var currentMaskAddressUpdates = new List<(long index, long value)>();

            foreach (var line in _splitInput)
            {
                var splitLine = line.Split(new[] { ' ', '[', ']', '=' }, StringSplitOptions.RemoveEmptyEntries);
                switch (splitLine[0])
                {
                    case "mask":
                        if (currentMask != "")
                            parsedInput.Add(currentMask, currentMaskAddressUpdates);
                        currentMask = splitLine[1];
                        currentMaskAddressUpdates = new List<(long index, long value)>();
                        break;
                    default:
                        currentMaskAddressUpdates.Add((long.Parse(splitLine[1]), long.Parse(splitLine[2])));
                        break;
                }
            }
            parsedInput.Add(currentMask, currentMaskAddressUpdates);
            
            // Solution
            var memory = new Dictionary<long, long>();
            foreach (var (mask, instructionList) in parsedInput)
            {
                var maskArray = mask.ToCharArray();
                maskArray.Reverse();

                foreach (var (insIndex, insValue) in instructionList)
                {
                    var binary = ConvertToBitRepresentation(insIndex, 36);

                    for (var i = 0; i < maskArray.Length; i++)
                        binary[i] = maskArray[i] == '0' ? binary[i] : maskArray[i];
                    
                    var floatIndicesList = binary.Select((val, index) => new { Value = val, Index = index })
                                                 .Where(x => x.Value == 'X')
                                                 .Select(x => x.Index)
                                                 .ToList();

                    if (floatIndicesList.Any())
                    {
                        for (var i = 0; i < (int)Math.Pow(2, floatIndicesList.Count); i++)
                        {
                            var bitsToUpdate = ConvertToBitRepresentation(i, floatIndicesList.Count);
                            for (var j = 0; j < floatIndicesList.Count; j++)
                                binary[floatIndicesList[j]] = bitsToUpdate[j];
                            memory[Convert.ToInt64(new string(binary), 2)] = insValue;
                        }
                    }
                    else
                        memory[Convert.ToInt64(new string(binary), 2)] = insIndex;
                }
            }

            return memory.Values.Sum().ToString();
        }

        private static char[] ConvertToBitRepresentation(long input, int maxLength) => Convert.ToString(input, 2).PadLeft(maxLength, '0').ToCharArray();
    }
}