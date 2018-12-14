using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode._2018
{
    class Day6 : DayProgram
    {
        public override void Run(string part)
        {
            var fileText = FileReader.ReadFile(2018, 6);
            var nav = new DangerIdentifier(fileText);
            Console.WriteLine(nav.FindSafeSpotB());
        }

        private class DangerIdentifier
        {
            Dictionary<Tuple<int, int>, int> locations = new Dictionary<Tuple<int, int>, int>();
            string pattern = "^(.*), (.*)$";

            public DangerIdentifier(string input)
            {
                Regex r = new Regex(pattern, RegexOptions.IgnoreCase);
                //loop through the list of instructions and parse out the conditions
                foreach (string instruction in input.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    var m = r.Match(instruction);
                    if (m.Success)
                    {
                        var location = new Tuple<int, int>(int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value));
                        locations.Add(location, 0);
                    }
                }
            }

            public int FindSafeSpotA()
            {
                var left = locations.Min(x => x.Key.Item1);
                var top = locations.Max(x => x.Key.Item2);
                var right = locations.Max(x => x.Key.Item1);
                var bottom = locations.Min(x => x.Key.Item2);

                //first, exclude anything which is deemed 'infinite'
                //take the first row / column outside the area, and determine the closest location
                //that one will be infinite
                var infiniteLocations = new List<Tuple<int, int>>();
                for (int yGrid = bottom; yGrid <= top; yGrid++)
                {
                    foreach (int xGrid in new int[] { left, right })
                    {
                        var closestLocation = FindClosest(xGrid, yGrid);
                        if (closestLocation != null)
                        {
                            infiniteLocations.Add(closestLocation);
                        }
                    }
                }

                for (int xGrid = left; xGrid <= right; xGrid++)
                {
                    foreach (int yGrid in new int[] { bottom, top })
                    {
                        var closestLocation = FindClosest(xGrid, yGrid);
                        if (closestLocation != null)
                        {
                            infiniteLocations.Add(closestLocation);
                        }
                    }
                }

                //loop through everywhere in the grid and work out the distance to each location, then increment the closest one
                for (int yGrid = bottom; yGrid <= top; yGrid++)
                {
                    for (int xGrid = left; xGrid <= right; xGrid++)
                    {
                        var closestLocation = FindClosest(xGrid, yGrid);
                        if (closestLocation != null)
                        {
                            locations[closestLocation]++;
                        }
                    }
                }
                return locations.Where(loc => !infiniteLocations.Contains(loc.Key)).Max(x => x.Value);
            }

            public int FindSafeSpotB()
            {
                var left = locations.Min(x => x.Key.Item1);
                var top = locations.Max(x => x.Key.Item2);
                var right = locations.Max(x => x.Key.Item1);
                var bottom = locations.Min(x => x.Key.Item2);

                var region = 0;
                //loop through everywhere in the grid and work out the distance to each location, then increment the closest one
                for (int yGrid = bottom; yGrid <= top; yGrid++)
                {
                    for (int xGrid = left; xGrid <= right; xGrid++)
                    {
                        var totalDistance = FindTotalDistance(xGrid, yGrid);
                        if (totalDistance < 10000)
                        {
                            region++;
                        }
                    }
                }
                return region;
            }

            private Tuple<int, int> FindClosest(int x, int y)
            {
                var distances = new Dictionary<Tuple<int, int>, int>();
                foreach(Tuple<int, int> location in locations.Keys)
                {
                    var distance = CalculatePointToPointDistance(location.Item1, location.Item2, x, y);
                    distances.Add(location, distance);
                }
                var minDistance = distances.Min(loc => loc.Value);
                var closestLocations = distances.Where(loc => loc.Value == minDistance);
                if (closestLocations.Count() == 1)
                {
                    return closestLocations.First().Key;
                } else
                {
                    return null ;
                }
            }

            private int FindTotalDistance(int x, int y)
            {
                var totalDistance = 0;
                foreach (Tuple<int, int> location in locations.Keys)
                {
                    var distance = CalculatePointToPointDistance(location.Item1, location.Item2, x, y);
                    totalDistance += distance;
                }
                return totalDistance;
            }

            private int CalculatePointToPointDistance(int firstX, int firstY, int secondX, int secondY)
            {
                return (Math.Abs(firstX - secondX) + Math.Abs(firstY - secondY));
            }
        }
    }
}
