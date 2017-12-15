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
            int usedCells = 0;
            for (int ii = 0; ii <= 127; ii++)
            {
                //call the knot hashing algorithm
                var ckt = new Day10.CompleteKnotTier(String.Format("{0}-{1}", input, ii));
                var hash = ckt.Run();
                foreach (char c in hash)
                {
                    usedCells += Convert.ToString(Convert.ToUInt32(c.ToString(), 16), 2).PadLeft(4, '0').Count(x => x =='1');
                }
            }
            Console.WriteLine(usedCells);
        }
    }
}
