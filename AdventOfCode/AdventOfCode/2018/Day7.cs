using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode._2018
{
    class Day7 : DayProgram
    {
        public override void Run(string part)
        {
            var fileText = FileReader.ReadFile(2018, 7);

            var parser = new SleighBuilder(fileText);
            Console.WriteLine(parser.DetermineBuildOrder());
            Console.WriteLine(parser.DetermineBuildMultipleElvesOrder());
        }

        private class SleighBuilder
        {
            private string _input;
            public SleighBuilder(string input)
            {
                _input = input;
            }

            public string DetermineBuildOrder()
            {
                string output = "";
                var logicHolder = ParseFile();
                //once we have the logic all set up we can loop through all the fields

                while (logicHolder.Any())
                {
                    var nextRule = logicHolder.Where(k => !k.Value.Any()).OrderBy(k => k.Key).Take(1).First();
                    output += nextRule.Key;
                    logicHolder.Remove(nextRule.Key);
                    foreach (List<string> needed in logicHolder.Select(k => k.Value))
                    {
                        if (needed.Contains(nextRule.Key))
                        {
                            needed.Remove(nextRule.Key);
                        }
                    }

                }
                return output;
            }

            public int DetermineBuildMultipleElvesOrder()
            {
                var output = "";
                var logic = ParseFile();
                var numWorkers = 4;
                //need to set up multiple worker, and keep track of timing
                Dictionary<string, int> workerChart = new Dictionary<string, int>();
                //until we have no more instructions to process
                var seconds = 0;
                while (logic.Any() || workerChart.Any())
                {
                    var nextRules = logic.Where(k => !k.Value.Any()).OrderBy(k => k.Key);
                    //if we have any workers who are free, pick up the next task
                    while (workerChart.Count < numWorkers && nextRules.Any())
                    {
                        var nextRule = nextRules.Take(1).First();
                        var letterToProcess = nextRule.Key;
                        logic.Remove(nextRule.Key);

                        int index = char.ToUpper(letterToProcess[0]) - 64 + 60;//index == 1
                        workerChart.Add(letterToProcess, index);
                    }
                    var workingWorkers = workerChart.Where(w => w.Value != 0).ToList();
                    foreach (KeyValuePair<string, int> worker in workingWorkers)
                    {
                        workerChart[worker.Key]--;
                        if (workerChart[worker.Key] == 0)
                        {
                            workerChart.Remove(worker.Key);
                            foreach (List<string> needed in logic.Select(k => k.Value))
                            {
                                if (needed.Contains(worker.Key))
                                {
                                    needed.Remove(worker.Key);
                                }
                            }
                            output += worker.Key;
                        }
                    }

                    seconds++;
                }

                Console.WriteLine(output);
                return seconds;
            }

            private Dictionary<string, List<string>> ParseFile()
            {
                var logicHolder = new Dictionary<string, List<string>>();
                Regex r = new Regex("^Step (\\D) must be finished before step (\\D) can begin.$", RegexOptions.IgnoreCase);
                //loop through the list of instructions and parse out the conditions
                foreach (string instruction in _input.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    var m = r.Match(instruction);
                    if (m.Success)
                    {
                        var needed = m.Groups[1].Value;
                        var toComplete = m.Groups[2].Value;

                        //add both outcome and condition, so both are present in the list (first step won't have any conditons)
                        if (!logicHolder.ContainsKey(toComplete))
                        {
                            logicHolder.Add(toComplete, new List<string>());
                        }
                        if (!logicHolder.ContainsKey(needed))
                        {
                            logicHolder.Add(needed, new List<string>());
                        }

                        logicHolder[toComplete].Add(needed);
                    }
                }
                return logicHolder;
            }
        }


    }
}
