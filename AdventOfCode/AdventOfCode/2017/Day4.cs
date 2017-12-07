using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2017
{
    class Day4 : DayProgram
    {
        public override void Run(string part)
        {
            string input = FileReader.ReadFile(2017, 4);
            var includeAnagrams = (part == "B");
            Console.WriteLine(PassPhraseValidator.GetPassPhrasesWithNoRepeatingWordsCount(input, includeAnagrams));

        }

        private class PassPhraseValidator
        {
            public static int GetPassPhrasesWithNoRepeatingWordsCount(string input, bool includeAnagrams) {
                var totalValid = 0;
                foreach (string line in input.Split(new[] { Environment.NewLine }, StringSplitOptions.None))
                {
                    List<string> phrases = new List<string>();
                    List<string> allPhrases = line.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    foreach (string phrase in allPhrases)
                    {
                        var currentPhrase = phrase;
                        if (includeAnagrams) { currentPhrase = String.Concat(currentPhrase.OrderBy(x => x)); }
                        if (!phrases.Contains(currentPhrase))
                        {
                            phrases.Add(currentPhrase);
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (phrases.Count == allPhrases.Count) { totalValid += 1; }
                }
                return totalValid;
            }
        }
    }
}
