using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2017
{
    class Day11 : DayProgram
    {
        public override void Run(string part)
        {
            var input = FileReader.ReadFile(2017, 11);
            var hn = new HexNavigator(input);
            if (part == "A")
            {
                Console.WriteLine(hn.CurrentSmallestDistance);
            }
            else
            {
                Console.WriteLine(hn.LargestDistance);
            }
            
        }

        private class HexNavigator
        {

            private List<string> _directions = new List<string>() { "n", "ne", "se", "s", "sw", "nw" };
            private Dictionary<string, int> _dictDirections = new Dictionary<string, int>();
            public int LargestDistance;

            public int CurrentSmallestDistance
            {
                get
                {
                    return _dictDirections.Sum(x => x.Value);
                }
            }

            public HexNavigator(string input)
            {
                foreach (string direction in _directions)
                {
                    _dictDirections.Add(direction, 0);
                }

                foreach (string hex in input.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                {
                    AppendCounts(hex);
                    CheckLargestDistance();
                }
            }

            private void CheckLargestDistance()
            {
                if (CurrentSmallestDistance > LargestDistance) { LargestDistance = CurrentSmallestDistance; }
            }

            private void AppendCounts(string hex)
            {
                //get the index of the _directions list, and we can program based on that
                var directionIndex = _directions.FindIndex(x => x == hex);
                //now some conditional statements to determine what we should do with the movements

                //if we already have movement in this direction, simply increase it
                int oppositeIndex = (directionIndex + 3) % _directions.Count;
                string oppositeDirection = _directions[oppositeIndex];
                //if the opposing side has a value, then subtract from it
                if (_dictDirections[oppositeDirection] > 0)
                {
                    _dictDirections[oppositeDirection]--;
                }
                else
                {
                    //check to see if there are any numbers two away in either direction
                    string leftDiagonalDirection = _directions[((directionIndex + 4) % _directions.Count)];
                    string rightDiagonalDirection = _directions[((directionIndex + 2) % _directions.Count)];
                    if (_dictDirections[leftDiagonalDirection] > 0)
                    {
                        string leftSiblingDirection = _directions[((directionIndex + 5) % _directions.Count)];
                        _dictDirections[leftDiagonalDirection]--;
                        _dictDirections[leftSiblingDirection]++;
                    }
                    else if (_dictDirections[rightDiagonalDirection] > 0)
                    {
                        string rightSiblingDirection = _directions[((directionIndex + 1) % _directions.Count)];
                        _dictDirections[rightDiagonalDirection]--;
                        _dictDirections[rightSiblingDirection]++;
                    }
                    else
                    {
                        _dictDirections[hex]++;
                    }

                }

            }
        }
    }
}
