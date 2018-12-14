using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Utils
{
    public class FileReader
    {
        public static string ReadFile(int year, int day)
        {
            string text = System.IO.File.ReadAllText(String.Format(@"C:\Users\Ryan\source\repos\AdventOfCode\AdventOfCode\AdventOfCode\{0}\Data\Day{1}.txt", year, day));
            return text;
        }
    }
}
