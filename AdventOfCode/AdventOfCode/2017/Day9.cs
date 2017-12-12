using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2017
{
    class Day9 : DayProgram
    {
        public override void Run(string part)
        {
            var input = FileReader.ReadFile(2017, 9);
            var gs = new GarbageSifter(input);
            gs.SetStreamValues();
            if (part == "A")
            {
                Console.WriteLine(gs.GroupValue);
            }
            else
            {
                Console.WriteLine(gs.GarbageCount);
            }

        }

        private class GarbageSifter
        {
            private string _input;
            private int _totalGroupValue = 0;
            private int _totalGarbage = 0;

            public int GroupValue
            {
                get
                {
                    return _totalGroupValue;
                }
            }

            public int GarbageCount
            {
                get
                {
                    return _totalGarbage;
                }
            }

            public GarbageSifter(string input)
            {
                _input = input;
            }

            public void SetStreamValues()
            {

                var groupValue = 0;
                var ignoreNext = false;
                var isGarbage = false;
                foreach (char chr in _input)
                {
                    //if we have an ignore flag, then we just skip to the next character
                    if (ignoreNext)
                    {
                        ignoreNext = false;
                        continue;
                    }
                    //if we are in garbage, and we aren't looking at a garbage closer or nullifier, skip it
                    if (isGarbage && chr.ToString() != ">" && chr.ToString() != "!")
                    {
                        _totalGarbage++;
                        continue;
                    }
                    switch (chr.ToString())
                    {
                        case "{":
                            //we have an opening group, so increase the group value by 1
                            groupValue++;
                            break;
                        case "}":
                            //once we hit the end of a group, we add the group value to the total, and reduce the group value
                            _totalGroupValue += groupValue;
                            groupValue--;
                            break;
                        case "<":
                            //we are now in some garbage
                            isGarbage = true;
                            break;
                        case ">":
                            isGarbage = false;
                            break;
                        case "!":
                            ignoreNext = true;
                            break;
                    }
                }
            }
        }


    }
}
