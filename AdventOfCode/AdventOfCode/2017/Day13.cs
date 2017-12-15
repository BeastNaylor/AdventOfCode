using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode._2017
{
    class Day13 : DayProgram
    {
        public override void Run(string part)
        {
            var input = FileReader.ReadFile(2017, 13);
            var ss = new SecuritySystem(input);
            if (part == "A")
            {
                Console.WriteLine(ss.CalculateSeverity(0).Severity);
            }
            else
            {
                var delay = 0;
                ScanResults result;
                do
                {
                    delay++;
                    result = ss.CalculateSeverity(delay, true);
                } while (result.Caught);
                Console.WriteLine(delay);
            }
        }

        private class SecuritySystem
        {
            Dictionary<int, int> _scanners = new Dictionary<int, int>();
            private int _severity = 0;

            public int Severity { get { return _severity; } }

            public SecuritySystem(string input)
            {
                foreach (string line in input.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
                {
                    var regEx = @"(\d+): (\d+)";
                    var matches = Regex.Match(line, regEx);
                    var position = matches.Groups[1].Value;
                    _scanners.Add(int.Parse(position), int.Parse(matches.Groups[2].Value));
                }
            }

            public ScanResults CalculateSeverity(int delay, bool CantBeCaught = false)
            {
                var results = new ScanResults() { Caught = false, Severity = 0 };
                foreach (KeyValuePair<int, int> scanner in _scanners)
                {
                    //add the delay to the index of the scanner to determine to time taken to reach it
                    var timeTaken = scanner.Key + delay;
                    //find how long it takes the scanner to reach back to zero
                    var scannerReturnTime = (scanner.Value - 1) * 2;
                    //the position as the packet arrives is then
                    var positionDuringPacket = timeTaken % scannerReturnTime;
                    if (positionDuringPacket == 0)
                    {
                        results.Caught = true;
                        results.Severity += scanner.Key * scanner.Value;
                        if (CantBeCaught) { break; }
                    }
                }
                return results;
            }
        }
        private struct ScanResults
        {
            public bool Caught;
            public int Severity;
        }
    }
}
