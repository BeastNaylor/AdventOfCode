using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using AdventOfCode.Utils;

namespace AdventOfCode
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new Options();
            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                var className = String.Format("AdventOfCode._{0}.Day{1}", options.Year, options.Day);
                var type = Type.GetType(className);
                if (type != null)
                {
                    var myObject = (DayProgram)Activator.CreateInstance(type);
                    myObject.Run(options.Part);
                }
                else
                {
                    throw new ArgumentNullException(String.Format("No Valid Class found to Run the Advent of Code for Day {0} and Year {1}", options.Day, options.Year));
                }

            }
            Console.ReadLine();
        }
    }

    class Options
    {
        [Option('y', "year", Required = true,
          HelpText = "Advent of Code Year.")]
        public int Year { get; set; }

        [Option('d', "day", Required = true,
          HelpText = "Advent of Code Day.")]
        public int Day { get; set; }

        [Option('p', "part", Required = true,
        HelpText = "Advent of Code Day Part.")]
        public string Part { get; set; }
    }
}
