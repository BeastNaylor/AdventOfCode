using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2017
{
    class Day14 : DayProgram
    {
        public override void Run(string part)
        {
            var input = "jxqlasbh";
            var mf = new MemoryFragmentation(input);
            if (part == "A")
            {
                Console.WriteLine(mf.UsedCells);
            }
            else
            {
                Console.WriteLine(mf.DetermineGroups());
            }


        }

        private class MemoryFragmentation
        {
            private string _input = "";
            private Dictionary<Tuple<int, int>, bool> _cells = new Dictionary<Tuple<int, int>, bool>();

            public MemoryFragmentation(string input)
            {
                _input = input;
                Process();
            }

            public int UsedCells
            {
                get
                {
                    return _cells.Values.Count(x => x == true);
                }
            }

            private void Process()
            {
                for (int ii = 0; ii <= 127; ii++)
                {
                    //call the knot hashing algorithm
                    var ckt = new Day10.CompleteKnotTier(String.Format("{0}-{1}", _input, ii));
                    var hash = ckt.Run();
                    var colCount = 0;
                    foreach (char c in hash)
                    {
                        string rowBinary = Convert.ToString(Convert.ToUInt32(c.ToString(), 16), 2).PadLeft(4, '0');
                        foreach (char num in rowBinary)
                        {
                            //add the tuples into the dict, along with whether the light is on or not
                            var tuple = new Tuple<int, int>(ii, colCount);
                            _cells.Add(tuple, num == '1');
                            colCount++;
                        }
                    }
                }
            }

            public int DetermineGroups()
            {
                var groupCount = 0;
                var connectedLights = new List<Tuple<int, int>>();
                var cellsToCheck = new List<Tuple<int, int>>();
                //loop through all the true cells
                foreach (KeyValuePair<Tuple<int, int>, bool> kvpCell in _cells.Where(x => x.Value == true))
                {
                    if (!connectedLights.Contains(kvpCell.Key))
                    {
                        groupCount++;
                        cellsToCheck.Add(kvpCell.Key);
                        while (cellsToCheck.Any())
                        {
                            var checkingCell = cellsToCheck.First();
                            for (int ii = -1; ii <= 1; ii++)
                            {
                                for (int jj = -1; jj <= +1; jj++)
                                {
                                    //way to stop any diagonals appearing
                                    if (Math.Abs(ii) != 1 || Math.Abs(jj) != 1)
                                    {
                                        var newTuple = new Tuple<int, int>(checkingCell.Item1 + ii, checkingCell.Item2 + jj);
                                        if (_cells.ContainsKey(newTuple) && !connectedLights.Contains(newTuple) && _cells[newTuple])
                                        {
                                            connectedLights.Add(newTuple);
                                            cellsToCheck.Add(newTuple);
                                        }
                                    }

                                }
                            }
                            cellsToCheck.Remove(checkingCell);
                        }
                    }
                }

                return groupCount;
            }
        }
    }
}
