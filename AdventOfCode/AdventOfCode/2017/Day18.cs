using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode._2017
{
    class Day18 : DayProgram
    {
        public override void Run(string part)
        {
            var input = FileReader.ReadFile(2017, 18);
            var se = new SoundEngine(input);
            Console.WriteLine(se.DetermineFirstSound());
        }

        private class SoundEngine
        {
            private List<string> _instructions = new List<string>();
            private Dictionary<string, int> _registers = new Dictionary<string, int>();
            public SoundEngine(string input)
            {
                foreach (string line in input.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
                {
                    _instructions.Add(line);
                }
            }

            public int DetermineFirstSound()
            {
                var recoveredSound = 0;
                var instructionPointer = 0;
                var currentSound = 0;
                while (recoveredSound == 0)
                {
                    var instruction = _instructions[instructionPointer];
                    var matches = Regex.Match(instruction, @"(\w+) (\w+) ?(-?\w+)?");
                    //check the first group to see what we are supposed to be doing
                    var code = matches.Groups[1].Value;
                    var val2 = matches.Groups[2].Value;
                    var val3 = matches.Groups[3].Value;

                    switch (code)
                    {
                        case "snd":
                            //for snd, we play the value
                            currentSound = _registers[val2];
                            break;
                        case "set":
                            //set makes the relevant register equal to that value
                            if (!_registers.ContainsKey(val2)) { _registers.Add(val2, 0); }
                            _registers[val2] = checkRegisterForValues(val3);
                            break;
                        case "add":
                            //add increases the relevant register by that value
                            if (!_registers.ContainsKey(val2)) { _registers.Add(val2, 0); }
                            _registers[val2] += checkRegisterForValues(val3);
                            break;
                        case "mul":
                            //multiply the reg
                            if (!_registers.ContainsKey(val2)) { _registers.Add(val2, 0); }
                            _registers[val2] *= checkRegisterForValues(val3);
                            break;
                        case "mod":
                            //mod the reg
                            if (!_registers.ContainsKey(val2)) { _registers.Add(val2, 0); }
                            _registers[val2] = _registers[val2] % checkRegisterForValues(val3);
                            break;
                        case "rcv":
                            //check if this register is non-zero, if it is, set recovered to the last sound
                            if (_registers.ContainsKey(val2) && _registers[val2] != 0)
                            {
                                recoveredSound = currentSound;
                            }
                            break;
                        case "jgz":
                            //if register is non-zero, change the instruction pointer to jump to a different instruction
                            if (checkRegisterForValues(val2) > 0)
                            {
                                var offset = checkRegisterForValues(val3);
                                instructionPointer = instructionPointer + offset;
                                //need to subrtract one from the pointer, as it will automatically increment by one at the end
                                instructionPointer--;
                            }
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(code + " doesn't have some logic defined.");
                    }
                    //increase the instruction
                    instructionPointer++;
                }

                return recoveredSound;
            }

            private int checkRegisterForValues(string key)
            {
                int value = 0;
                if (_registers.ContainsKey(key))
                {
                    value = _registers[key];
                }
                else
                {
                    value = int.Parse(key);
                }
                return value;
            }

        }
    }
}
