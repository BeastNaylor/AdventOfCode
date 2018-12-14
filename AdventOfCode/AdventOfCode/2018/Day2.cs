using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2018
{
    class Day2 : DayProgram
    {
        public override void Run(string part)
        {
            var fileText = FileReader.ReadFile(2018, 2);
            var reader = new BoxReader(fileText);
            Console.WriteLine(reader.CalculateCheckSum());
            Console.WriteLine(reader.FindMostSimilarBoxIds());
        }

        private class BoxReader
        {
            private string _input;
            public BoxReader(string input)
            {
                _input = input;
            }

            public int CalculateCheckSum()
            {
                var doubleLetterCount = 0;
                var tripleLetterCount = 0;

                foreach (string box in _input.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    var groupings = box.GroupBy(x => x);
                    if (groupings.Where(x => x.Count() == 2).Any())
                    {
                        doubleLetterCount++;
                    }

                    if (groupings.Where(x => x.Count() == 3).Any())
                    {
                        tripleLetterCount++;
                    }

                }

                return doubleLetterCount * tripleLetterCount;
            }

            public string FindMostSimilarBoxIds()
            {
                var boxIds = _input.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                var checkedBoxIds = new HashSet<string>();

                foreach (string box in boxIds)
                {
                    //check box against existing ids
                    foreach (string checkedBox in checkedBoxIds)
                    {
                        HashSet<int> missCount = new HashSet<int>();
                        //iterate over the characters in the string, we are allowed one 'miss'
                        for(int ii = 0;  ii < checkedBox.Length -1;  ii++)
                        {
                            if (box[ii] != checkedBox[ii])
                            {
                                missCount.Add(ii);
                            }
                        }
                        if (missCount.Count < 2)
                        {
                            return box.Remove(missCount.First(), 1);
                        }
                    }

                    //not found, then add to list
                    checkedBoxIds.Add(box);
                }

                return "";
            }
        }


    }

}