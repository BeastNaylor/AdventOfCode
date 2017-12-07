using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2017
{
    class Day5 : DayProgram
    {
        public override void Run(string part)
        {
            string input = FileReader.ReadFile(2017, 5);
            var mazeTraverser = new MazeTraverser(input, part);
            Console.WriteLine(mazeTraverser.Traverse());
        }


        private class MazeTraverser
        {
            List<int> _positions = new List<int>();
            string _part = "";

            public MazeTraverser(string input, string part)
            {
                //convert string into list of instructions
                _positions = input.Split(new[] { Environment.NewLine }, StringSplitOptions.None).Select(x => int.Parse(x)).ToList();
                _part = part;
            }

            public int Traverse()
            {
                //start at index 0
                var currLocation = 0;
                var jumps = 0;
                while (true)
                {
                    //get next instruction from current location
                    if (currLocation >= _positions.Count) { break; }
                    var nextInstruction = _positions[currLocation];
                    //increment old instruction by 1
                    var change = 1;
                    //if we are in part B and the location is 3 or more, then we need to reduce it 
                    if (_part == "B" && _positions[currLocation] >= 3) {change = -1;}
                    _positions[currLocation] += change;
                    //change location based on next Instruction
                    currLocation += nextInstruction;

                    jumps++;
                }
                return jumps;
            }
        }
    }
}
