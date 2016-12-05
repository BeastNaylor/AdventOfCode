using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    class AdventOne
    {
        public static void Run()
        {
            Console.WriteLine("Running Advent One");
            var instructions = Properties.Resources.AdventOne;

            //start at 0,0 for North and East directions
            //keep track of which direction facing
            //add/subtract from relevant figure as appropriate

            int northDistance = 0;
            int eastDistance = 0;
            Direction dir = Direction.North;
            Console.WriteLine(String.Format("Start Facing {0} at N{1}E{2}", dir, northDistance, eastDistance));
            foreach (string movement in instructions.Split(','))
            {

                var trimmedMovement = movement.Trim();
                Console.WriteLine(trimmedMovement);
                //trim first letter for the direction to turn in, and cast the rest to an int
                var turnDirection = trimmedMovement.Substring(0, 1);
                int distance = Int32.Parse(trimmedMovement.Substring(1, trimmedMovement.Length - 1));

                dir = changeDirection(dir, turnDirection);
                Console.WriteLine(String.Format("Moving {0} {1} spaces", dir, distance));
                switch (dir)
                {
                    case Direction.North:
                        northDistance += distance;
                        break;
                    case Direction.East:
                        eastDistance += distance;
                        break;
                    case Direction.South:
                        northDistance -= distance;
                        break;
                    case Direction.West:
                        eastDistance -= distance;
                        break;
                }
                Console.WriteLine(String.Format("Currently at N{0} E{1}", northDistance, eastDistance));
            }
            int totalDistance = Math.Abs(northDistance) + Math.Abs(eastDistance);
            Console.WriteLine(String.Format("Final Position N{0}E{1}, total distance {2}", northDistance, eastDistance, totalDistance));
        }

        private static Direction changeDirection(Direction currentDir, string turnDirection) 
        {
            //use MOD 4 to spin through the different directions (R turn addition, L turn subtraction)
            int newDirectionInt = 0;
            int oldDirectionInt = (int)currentDir;
            //get rid of 0, so we don't have to deal with negatives
            if (oldDirectionInt == 0) { oldDirectionInt = 4; }
            switch (turnDirection)
            {
                case "L":
                    newDirectionInt = oldDirectionInt - 1;
                    break;
                case "R":
                    newDirectionInt = oldDirectionInt + 1;
                    break;
            }
            //can do MOD division to get back within our range
            newDirectionInt = (newDirectionInt % 4);
            return (Direction)newDirectionInt;
        }

        private enum Direction
        {
            North = 0,
            East = 1,
            South = 2,
            West = 3
        }
    }
}
