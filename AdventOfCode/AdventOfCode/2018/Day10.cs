using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode._2018
{
    class Day10 : DayProgram
    {
        public override void Run(string part)
        {
            var fileText = FileReader.ReadFile(2018, 10);
            var nav = new StarNavigator(fileText);
            int time = 0;
            while (true)
            {
                nav.Tick();
                if (nav.CheckGridSize())
                {
                    nav.Untick();
                    nav.Output();
                    Console.WriteLine(time);
                    Console.ReadKey();
                    
                }
                time++;
            }
        }

        private class StarNavigator
        {
            private long previousArea = 0;
            IList<Star> stars = new List<Star>();
            string pattern = "^position=<(.*), (.*)> velocity=<(.*), (.*)>$";

            public StarNavigator(string input)
            {
                Regex r = new Regex(pattern, RegexOptions.IgnoreCase);
                //loop through the list of instructions and parse out the conditions
                foreach (string instruction in input.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    var m = r.Match(instruction);
                    if (m.Success)
                    {
                        var star = new Star()
                        {
                            xPos = int.Parse(m.Groups[1].Value),
                            yPos = int.Parse(m.Groups[2].Value),
                            xVel = int.Parse(m.Groups[3].Value),
                            yVel = int.Parse(m.Groups[4].Value)
                        };
                        stars.Add(star);
                    }
                }
            }

            public bool CheckGridSize()
            {
                //get the size grid we need
                var left = stars.Min(x => x.xPos);
                var top = stars.Max(x => x.yPos);
                var right = stars.Max(x => x.xPos);
                var bottom = stars.Min(x => x.yPos);

                long width = right - left;
                long height = top - bottom;

                long currentArea = width * height;
                var bigger = currentArea > previousArea && previousArea != 0;
                previousArea = currentArea;
                return bigger;
            }

            public void Tick()
            {
                foreach (Star star in stars)
                {
                    star.Move();
                }
            }

            public void Untick()
            {
                foreach (Star star in stars)
                {
                    star.Reverse();
                }
            }

            public void Output()
            {
                //get the size grid we need
                var left = stars.Min(x => x.xPos);
                var top = stars.Max(x => x.yPos);
                var right = stars.Max(x => x.xPos);
                var bottom = stars.Min(x => x.yPos);

                for (int yGrid = bottom; yGrid <= top; yGrid++)

                {
                    var row = "";
                    for (int xGrid = left; xGrid <= right; xGrid++)
                    {
                        if (stars.Where(x => x.xPos == xGrid && x.yPos == yGrid).Any())
                        {
                            row += "*";
                        }
                        else
                        {
                            row += ".";
                        }
                    }
                    Console.WriteLine(">"+row);
                }
            }

        }

        private class Star
        {
            public int xPos = 0;
            public int yPos = 0;
            public int xVel = 0;
            public int yVel = 0;

            public void Move()
            {
                xPos += xVel;
                yPos += yVel;
            }

            public void Reverse()
            {
                xPos -= xVel;
                yPos -= yVel;
            }
        }
    }
}
