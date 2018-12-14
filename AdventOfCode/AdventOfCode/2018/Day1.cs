using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2018
{
    class Day1 : DayProgram
    {
        public override void Run(string part)
        {
            var fileText = FileReader.ReadFile(2018, 1);

            var parser = new FrequencyCalculator(fileText);
            if (part == "A")
            {
                Console.WriteLine(parser.ParseFile());
            }
            else if (part == "B")
            {
                Console.WriteLine(parser.FindFirstRepeatingFreq());
            }
        }

        private class FrequencyCalculator
        {
            private string _input;

            public FrequencyCalculator(string input)
            {
                _input = input;
            }

            public int ParseFile()
            {
                //loop through all the input and sum the numbers
                var startFreq = 0;
                foreach (string freq in _input.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    int nextFreq = int.Parse(freq);
                    startFreq += nextFreq;
                }

                return startFreq;
            }

            public int FindFirstRepeatingFreq()
            {
                var repeatedFreq = 0;
                var startFreq = 0;
                var freqs = new HashSet<int>();
                while (repeatedFreq == 0)
                {
                    foreach (string freq in _input.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        int nextFreq = int.Parse(freq);
                        startFreq += nextFreq;
                        if (freqs.Contains(startFreq)) {
                            repeatedFreq = startFreq;
                            break;
                        } else
                        {
                            freqs.Add(startFreq);
                        }
                    }
                };
                return repeatedFreq;
            }
        }
    }
}
