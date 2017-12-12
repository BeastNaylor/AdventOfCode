using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode._2017
{
    class Day8 : DayProgram
    {
        public override void Run(string part)
        {
            var input = FileReader.ReadFile(2017, 8);
            var rc = new RegisterCalculator(input);
            rc.RunInstructions();
            Console.WriteLine(rc.GetLargestCurrentRegister());
            Console.WriteLine(rc.maxRegisterSize);
        }

        private class RegisterCalculator
        {
            private Dictionary<string, int> _registers = new Dictionary<string, int>();
            private List<Instruction> _instructions = new List<Instruction>();
            public int maxRegisterSize;

            public RegisterCalculator(string input)
            {
                //parse the instructions into 
                foreach (string line in input.Split(new[] { Environment.NewLine }, StringSplitOptions.None))
                {
                    //we have some RegEx to pull out the necessary instructions
                    var regEx = @"(\w+) (\w+) (-?\d+) if (\w+) (\W+) (-?\d+)";
                    var matches = Regex.Match(line, regEx);
                    var increment = int.Parse(matches.Groups[3].Value);

                    var instruction = new Instruction() { register = matches.Groups[1].Value, change = (matches.Groups[2].Value == "inc") ? increment : -1 * increment, testRegister = matches.Groups[4].Value, testOperation = matches.Groups[5].Value, testValue = int.Parse(matches.Groups[6].Value) };
                    _instructions.Add(instruction);
                }
            }

            public void RunInstructions()
            {
                foreach (Instruction instr in _instructions)
                {
                    //all registers start at 0
                    if (!_registers.ContainsKey(instr.register)) { _registers.Add(instr.register, 0); }
                    //check the conditional
                    if (CheckConditional(instr.testRegister, instr.testOperation, instr.testValue))
                    {
                        _registers[instr.register] += instr.change;
                        if (_registers[instr.register] > maxRegisterSize) { maxRegisterSize = _registers[instr.register]; }
                    }

                }
            }

            public int GetLargestCurrentRegister()
            {
                return _registers.OrderByDescending(x => x.Value).First().Value;
            }

            private bool CheckConditional(string register, string func, int value)
            {
                var conditional = false;
                var registerValue = (_registers.ContainsKey(register)) ? _registers[register] : 0;
                switch (func)
                {
                    case "==":
                        conditional = (registerValue == value);
                        break;
                    case "!=":
                        conditional = (registerValue != value);
                        break;
                    case ">=":
                        conditional = (registerValue >= value);
                        break;
                    case "<=":
                        conditional = (registerValue <= value);
                        break;
                    case ">":
                        conditional = (registerValue > value);
                        break;
                    case "<":
                        conditional = (registerValue < value);
                        break;
                    default:
                        throw new ArgumentNullException("Missing operator logic for " + func);
                }

                return conditional;
            }

            private struct Instruction
            {
                public string register;
                public int change;
                public string testRegister;
                public string testOperation;
                public int testValue;
            }
        }


    }
}
