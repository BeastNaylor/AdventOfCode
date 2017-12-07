using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using AdventOfCode.Utils;

namespace AdventOfCode._2016
{
    class Day5: DayProgram
    {
        public override void Run(string part)
        {
            var code = new List<string>();
            var orderedCode = new List<Tuple<string, int>>();
            var seedCode = "ugkcyxxp";
            var seedCount = 0;
            var positional = (part == "B");
            do
            {
                seedCount += 1;
                var md5String = computeHash(seedCode + seedCount.ToString());
                if (md5String.StartsWith("00000")) 
                {
                    if (positional)
                    {
                        int characterLocation = 0;
                        var position = md5String.Substring(5, 1);
                        var codeCharacter = md5String.Substring(6, 1);
                        if (Int32.TryParse(position, out characterLocation))
                        {
                            if (characterLocation < 8 && !orderedCode.Any(x => x.Item2 == characterLocation))
                            {
                                Console.WriteLine(String.Format("Found character {0} after {1} loops at position {2}", codeCharacter, seedCount, characterLocation));
                                orderedCode.Add(new Tuple<string, int>(codeCharacter, characterLocation));
                            }
                        }
                    }
                    else
                    {
                        //just add the characters to the list
                        var sixthCharacter = md5String.Substring(5, 1);
                        code.Add(sixthCharacter);
                        Console.WriteLine(String.Format("Found character {0} after {1} loops", sixthCharacter, seedCount));
                    }

                }
            } while (code.Count < 8);
            Console.WriteLine(String.Format("The Code is {0}", String.Join("", code)));
        }


        private static string computeHash(string input)
        {
            // step 1, calculate MD5 hash from input

            MD5 md5 = MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }

            return sb.ToString();
        }

    }
}
