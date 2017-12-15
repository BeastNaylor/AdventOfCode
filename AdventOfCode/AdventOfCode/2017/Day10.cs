using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2017
{
    class Day10 : DayProgram
    {
        public override void Run(string part)
        {
            var input = FileReader.ReadFile(2017, 10);
            if (part == "A")
            {
                var kt = new KnotTier(input, 256);
                kt.TieKnots();
                Console.WriteLine(kt.ProductOfFirstTwoElements);
            }
            else if (part == "B")
            {
                var kt = new CompleteKnotTier(input);
                Console.Write(kt.Run());
            }

        }

        public class CompleteKnotTier
        {
            private string _ascii = "";

            public CompleteKnotTier(string input)
            {
                _ascii = ConvertForPartB(input);
            }

            public string Run()
            {
                //need to tie the knots 64 times, preserving the skip/currentLocation
                var kt = new KnotTier(_ascii, 256);
                for (int ii = 0; ii <= 63; ii++)
                {
                    kt.TieKnots();
                }
                var denseHash = kt.GetDenseHash;
                return ConvertDenseHashToString(denseHash);
            }

            private string ConvertForPartB(string input)
            {
                //foreach character we have in the string, convert to Ascii format
                StringBuilder asciiString = new StringBuilder();
                foreach (char c in input)
                {
                    var asciiCode = (int)c;
                    asciiString.Append(asciiCode);
                    asciiString.Append(",");
                }
                asciiString.Append("17, 31, 73, 47, 23");
                return asciiString.ToString();
            }

            private string ConvertDenseHashToString(List<int> denseHash)
            {
                var hashString = "";
                foreach (int code in denseHash)
                {
                    hashString += code.ToString("X2");
                }
                return hashString;
            }
        }


        private class KnotTier
        {
            private List<int> _lengths = new List<int>();
            private Dictionary<int, int> _elements = new Dictionary<int, int>();
            private int _currentLocation = 0;
            private int _skipSize = 0;

            public int ProductOfFirstTwoElements
            {
                get
                {
                    return _elements[0] * _elements[1];
                }
            }

            public List<int> GetDenseHash
            {
                get
                {
                    var denseHash = new List<int>();
                    //loop through the elements in sets of 16, performing a bitwise XOR on them all
                    for (int ii = 0; ii <= 15; ii++)
                    {
                        //XOR 0 gives the original nunber, so a perfect starting point
                        var combinedValue = 0;
                        //loop through the 16 and bitwise XOR the startingValue, and add to the list
                        for (int jj = 0; jj <= 15; jj++)
                        {
                            //find the element we want
                            var index = (ii * 16) + jj;
                            combinedValue = combinedValue ^ _elements[index];
                        }
                        denseHash.Add(combinedValue);
                    }
                    return denseHash;
                }
            }

            public KnotTier(string input, int numElements)
            {
                _elements = new Dictionary<int, int>();
                for (int ii = 0; ii <= numElements - 1; ii++)
                {
                    _elements.Add(ii, ii);
                }
                _lengths = input.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).ToList();
            }

            public void TieKnots()
            {
                //for each length to tie knots
                foreach (int length in _lengths)
                {
                    //make a new list which we can rotate
                    var tempList = new List<int>();
                    //write from the original list into the tempList from the current position
                    for (int ii = 0; ii <= length - 1; ii++)
                    {
                        var index = (_currentLocation + ii) % _elements.Count;
                        tempList.Add(_elements[index]);
                    }
                    tempList.Reverse();
                    for (int ii = 0; ii <= length - 1; ii++)
                    {
                        var index = (_currentLocation + ii) % _elements.Count;
                        _elements[index] = tempList[ii];
                    }
                    _currentLocation = (_currentLocation + length + _skipSize) % _elements.Count;
                    _skipSize++;
                }
            }
        }
    }
}
