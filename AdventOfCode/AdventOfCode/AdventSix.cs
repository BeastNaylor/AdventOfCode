using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    class AdventSix
    {
        public static void Run()
        {
            var codes = Properties.Resources.AdventSix;
            //for each code, take out each of the letters and save them into their respective arrays
            var codeManager = new CodeManager();
            var managerInitialised = false;
            foreach (string code in codes.Split(new string[] { Environment.NewLine }, StringSplitOptions.None))
            {
                var positions = code.Length;
                if (!managerInitialised)
                {
                    codeManager.InitialisePositions(positions);
                    managerInitialised = true;
                }
                for(int index = 0; index <= code.Length -1; index++)
                {
                    var letter = code[index];
                    codeManager.AddLetter(index,letter);
                }
            }
            //once we have all the codes split out, we want to get the most common in each position
            Console.WriteLine(String.Format("The most common letters are {0}", codeManager.GetOrderOfLetters(false)));
            Console.WriteLine(String.Format("The least common letters are {0}", codeManager.GetOrderOfLetters(true)));
        }

        private class CodeManager
        {
            private Dictionary<int, List<char>> _lettersAtPosition;

            //create the dictionary will a list for each position of the code
            public void InitialisePositions(int positions)
            {
                _lettersAtPosition = new Dictionary<int,List<char>>();
                for (int ii = 0; ii < positions; ii++)
                {
                    _lettersAtPosition.Add(ii, new List<char>());
                }
            }

            public void AddLetter(int index, char letter)
            {
                //add the letter to the relevant list
                _lettersAtPosition[index].Add(letter);
            }

            internal string GetOrderOfLetters(bool ascend)
            {
                var sb = new StringBuilder();
                //loop through each list in the dict and get the common letters
                foreach (KeyValuePair<int, List<char>> letters in _lettersAtPosition)
                {
                    IGrouping<char, char> letter;
                    var groupedLetters = letters.Value.GroupBy(x => x);
                    if (ascend)
                    {
                        letter = groupedLetters.OrderBy(x => x.Count()).First();
                    }
                    else
                    {
                        letter = groupedLetters.OrderByDescending(x => x.Count()).First();
                    }
                    sb.Append(letter.Key);
                }
                return sb.ToString();
            }
        }
    }
}
