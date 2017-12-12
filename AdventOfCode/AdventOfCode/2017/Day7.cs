using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode._2017
{
    class Day7 : DayProgram
    {
        public override void Run(string part)
        {
            var input = FileReader.ReadFile(2017, 7);
            var tc = new TowerConstructor(input);
            Console.WriteLine(tc.GetBaseTower());
            tc.GetCorrectWeight();

        }

        private class TowerConstructor
        {
            private Dictionary<string, int> _towers = new Dictionary<string, int>();
            private List<Tuple<string, string>> _relations = new List<Tuple<string, string>>();

            public TowerConstructor(string input)
            {
                BuildTowers(input);
            }

            public string GetBaseTower()
            {
                //get the first tuple and check if it exists as a child in any of the other tuples.
                //if it does, then continue looking for it as a child
                //once you can't find a tower it is the child of, it must be the base
                var searchTuple = _relations.First().Item1;
                while (true)
                {
                    if (_relations.Where(x => x.Item2 == searchTuple).Any())
                    {
                        searchTuple = _relations.Where(x => x.Item2 == searchTuple).Select(x => x.Item1).Single();
                    }
                    else
                    {
                        break;
                    }
                }
                return searchTuple;
            }

            public int GetCorrectWeight()
            {
                //to find which of the discs is incorrect
                var baseTower = GetBaseTower();
                while (true)
                {
                    var childWeights = new Dictionary<string, int>();
                    foreach (string child in _relations.Where(x => x.Item1 == baseTower).Select(x => x.Item2))
                    {
                        //get children of base, and determine the weight of each
                        var childWeight = GetWeightOfTower(child);
                        var totalWeight = childWeight.Item1 + childWeight.Item2;
                        childWeights.Add(child, totalWeight);
                    }
                    var sorted = childWeights.GroupBy(x => x.Value).OrderBy(x => x.Count());
                    if (sorted.Count() > 1)
                    {
                        //if we have more than one value, we need to repeat the search
                        //get the towername from the grouping
                        baseTower = sorted.First().First().Key;
                    }
                    else
                    {
                        //if we have only one grouping, it means the towers are correctly aligned at this point
                        break;
                    }
                }
                //now we have the incorrect tower, get the siblings for it again and thus all the weights which should be the same
                var parent = _relations.Where(x => x.Item2 == baseTower).Select(x => x.Item1).Single();

                var siblingWeights = new Dictionary<string, Tuple<int,int>>();
                foreach (string sibling in _relations.Where(x => x.Item1 == parent).Select(x => x.Item2))
                {
                    //get children of base, and determine the weight of each
                    var siblingWeight = GetWeightOfTower(sibling);
                    siblingWeights.Add(sibling, siblingWeight);
                }

                var grouped = siblingWeights.GroupBy(x => x.Value.Item1 + x.Value.Item2).OrderBy(x => x.Count());
                //get the difference between the two groups
                var wrongTotalWeight = grouped.First().Key;
                var correctTotalWeight = grouped.Last().Key;
                var diff = wrongTotalWeight - correctTotalWeight;
                //now subtract this difference from the base tower weight for the wrong stack, to get the correct value
                var correctWeight = grouped.First().First().Value.Item1 - diff;
                return correctWeight;
            }

            private Tuple<int, int> GetWeightOfTower(string towerName)
            {
                //get the weight of the tower
                var baseWeight = _towers[towerName];
                var totalChildrenWeight = 0;
                foreach (string child in _relations.Where(x => x.Item1 == towerName && x.Item2 != "").Select(x => x.Item2))
                {
                    var childWeight = GetWeightOfTower(child);
                    totalChildrenWeight += childWeight.Item1 + childWeight.Item2;
                }
                return new Tuple<int,int>(baseWeight, totalChildrenWeight);
            }

            private void BuildTowers(string input)
            {
                foreach (string single in input.Split(new[] { Environment.NewLine }, StringSplitOptions.None))
                {
                    //for each row, we need to parse out the name, weight and the children for it
                    //if we have an arrow, we have children
                    var towerInfo = "";
                    var children = "";
                    if (single.Contains("->"))
                    {
                        towerInfo = single.Substring(0, single.IndexOf("-")).Trim();
                        children = single.Substring(single.IndexOf(">") + 1).Trim();
                    }
                    else
                    {
                        towerInfo = single;
                    }

                    //now we add the towers to the list of towers we have
                    var nameRegExp = @"([\w]+)\s\(";
                    var weightRegExp = @"\(([\d]+)\)";
                    string name = Regex.Match(towerInfo, nameRegExp).Groups[1].Value;
                    int weight = int.Parse(Regex.Match(towerInfo, weightRegExp).Groups[1].Value);
                    _towers.Add(name, weight);

                    //now take the children and for each one add into list of parent-children
                    foreach (string child in children.Split(new[] { "," }, StringSplitOptions.None))
                    {
                        var relation = new Tuple<string, string>(name, child.Trim());
                        _relations.Add(relation);
                    }
                }
            }
        }
    }
}
