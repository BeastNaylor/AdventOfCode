using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode._2018
{
    class Day3 : DayProgram
    {
        public override void Run(string part)
        {
            var fileText = FileReader.ReadFile(2018, 3);
            var reader = new FabricCutter(fileText);
            Console.WriteLine(reader.DetermineOverlappingFabric());
            Console.WriteLine(reader.DetermineNonOverlappingClaim());
        }
        private class FabricCutter
        {
            private string pattern = "^#([0-9]{1,}) @ ([0-9]{1,}),([0-9]{1,}):\\s([0-9]{1,})x([0-9]{1,})";

            string _input;
            Dictionary<Tuple<int, int>, int> _coords;
            public FabricCutter(string input)
            {
                _input = input;
            }

            public int DetermineOverlappingFabric()
            {
                _coords = new Dictionary<Tuple<int, int>, int>();
                Regex r = new Regex(pattern, RegexOptions.IgnoreCase);
                foreach (string square in _input.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    var m = r.Match(square);
                    if (m.Success)
                    {
                        var id = int.Parse(m.Groups[1].Value);
                        var leftOffset = int.Parse(m.Groups[2].Value);
                        var topOffset = int.Parse(m.Groups[3].Value);
                        var width = int.Parse(m.Groups[4].Value);
                        var height = int.Parse(m.Groups[5].Value);
                        for (int ii = leftOffset; ii <= leftOffset + width -1; ii++)
                        {
                            for (int jj = topOffset; jj <= topOffset + height -1; jj++)
                            {
                                //add to tuples, or increase
                                var tuple = new Tuple<int, int>(ii, jj);

                                if (!_coords.ContainsKey(tuple))
                                {
                                    _coords.Add(tuple, 0);
                                }
                                _coords[tuple]++;
                            }
                        }

                    }
                    else
                    {
                        Console.WriteLine($"RegexError on {square}");
                    }
                }
                return _coords.Where(x => x.Value > 1).Count();
            }

            public int DetermineNonOverlappingClaim()
            {
                Regex r = new Regex(pattern, RegexOptions.IgnoreCase);
                //loop through them all again, and find the only one without an overlap
                foreach (string square in _input.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    var m = r.Match(square);
                    if (m.Success)
                    {
                        bool IsOverlapping = false;
                        var id = int.Parse(m.Groups[1].Value);
                        var leftOffset = int.Parse(m.Groups[2].Value);
                        var topOffset = int.Parse(m.Groups[3].Value);
                        var width = int.Parse(m.Groups[4].Value);
                        var height = int.Parse(m.Groups[5].Value);
                        for (int ii = leftOffset; ii <= leftOffset + width - 1; ii++)
                        {
                            for (int jj = topOffset; jj <= topOffset + height - 1; jj++)
                            {
                                var tuple = new Tuple<int, int>(ii, jj);
                                if (_coords[tuple] != 1)
                                {
                                    IsOverlapping = true;
                                }
                            }
                        }
                        if (!IsOverlapping)
                        {
                            return id;
                        }
                    }
                    else
                    {
                        Console.WriteLine($"RegexError on {square}");
                    }

                }

                return 0;
            }
        }
    }
}
