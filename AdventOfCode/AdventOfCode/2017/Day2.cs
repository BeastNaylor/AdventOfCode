using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2017
{
    class Day2 : DayProgram
    {
        public override void Run(string part)
        {
            var fileText = FileReader.ReadFile(2017, 2);
            var calculator = new ExcelCalculator(fileText);
            var checkSum = calculator.DetermineCheckSum(part);
            Console.WriteLine(checkSum);
        }

        private class ExcelCalculator
        {
            private string _text;

            public ExcelCalculator(string text)
            {
                _text = text;
            }

            public int DetermineCheckSum(string part)
            {
                int checkSum = 0;
                foreach (string line in _text.Split(new[] { Environment.NewLine }, StringSplitOptions.None))
                {
                    var maxValue = 0;
                    var minValue = 0;
                    var numbers = new List<int>();
                    //for each row in the excel, loop through all the numbers, keeping track of the max and min values encountered
                    foreach (string entry in line.Split(new[] { "\t" }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        int number = int.Parse(entry);
                        numbers.Add(number);
                        if (number > maxValue) { maxValue = number; }
                        if (number < minValue || minValue == 0) { minValue = number; }
                    }
                    
                    if (part == "A")
                    {
                        //then at the end, take the difference of these values and add to the checksum
                        checkSum += (maxValue - minValue);
                    }
                    else if (part == "B")
                    {
                        //sort the list of numbers, and loop through each one, dividing by the remaining smaller numbers
                        var orderedNumbers = numbers.OrderByDescending(x => x).ToList();
                        bool divisorFound = false;
                        for (int ii = 0; ii <= orderedNumbers.Count - 1; ii++)
                        {
                            var numerator = orderedNumbers[ii];
                            for (int jj = ii + 1; jj <= orderedNumbers.Count - 1; jj++)
                            {
                                var denominator = orderedNumbers[jj];
                                var remainder = numerator % denominator;
                                if (remainder == 0)
                                {
                                    checkSum += numerator / denominator;
                                    divisorFound = true;
                                    break;
                                }
                            }
                            if (divisorFound) { break; }
                        }
                    }
                    
                }
                return checkSum;
            }

        }
    }
}
