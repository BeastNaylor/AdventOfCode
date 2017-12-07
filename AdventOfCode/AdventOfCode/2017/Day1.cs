using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2017
{
    class Day1 : DayProgram
    {
        public override void Run(string part)
        {
            var fileText = FileReader.ReadFile(2017, 1);

            var parser = new MatchingCharacterParser(fileText, part);
            var total = parser.ParseFile();
            Console.WriteLine(total);
        }


        private class MatchingCharacterParser {

            private string text = "";
            private int _offset = 0;

            public MatchingCharacterParser(string fileText, string part)
            {
                this.text = fileText;
                _offset = (part == "B") ? (fileText.Length / 2) : 1;
            }

            public int ParseFile() {
                //loop through all the characters within the file
                int total = 0;
                for (int ii = 0; ii <= this.text.Length - 1; ii++)
                {
                    //determine the current and next character positions (looping around when it reaches the end of the string)
                    var currCharPosition = ii;
                    int nextCharPosition = (ii + _offset) % this.text.Length;

                    char currChar = this.text[currCharPosition];
                    char nextChar = this.text[nextCharPosition];
                    //if the characters match, add the current character to the total
                    if (currChar == nextChar)
                    {
                        total += int.Parse(currChar.ToString());
                    }
                }
                return total;
            }
        }

    }
}
