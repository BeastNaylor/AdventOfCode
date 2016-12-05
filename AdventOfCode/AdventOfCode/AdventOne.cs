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

            var routeManager = new RouteManager(0, 0);
            foreach (string movement in instructions.Split(','))
            {
                Console.WriteLine(movement);
                routeManager.NewDirection(movement.Trim());
                routeManager.OutputLocation();
            }
            routeManager.OutputDisplacement();
        }

        private class RouteManager
        {

            private int _northCoord = 0;
            private int _eastCoord = 0;
            private Direction _direction = Direction.North;

            public RouteManager(int northCoord, int eastCoord)
            {
                _northCoord = northCoord;
                _eastCoord = eastCoord;
                OutputLocation();
            }

            public void NewDirection(string direction)
            {
                //get the first part of the instructions and turn the direction
                var turnDirection = direction.Substring(0, 1);
                changeDirection(turnDirection);
                //get the distance we are travelling in this new direction
                int displacement = Int32.Parse(direction.Substring(1, direction.Length - 1));
                move(displacement);
            }

            private void changeDirection(string turnDirection)
            {
                int oldDirectionInt = (int)_direction;
                int change = ((turnDirection == "R") ? 1 : -1);
                //get rid of 0, so we don't have to deal with negatives
                if (oldDirectionInt == 0) { oldDirectionInt = 4; }
                //change the direction and MOD division to get back within our range
                int newDirectionInt = ((oldDirectionInt += change) % 4); ;
                _direction = (Direction)newDirectionInt;
                Console.WriteLine(String.Format("Now facing {0}", _direction));
            }

            private void move(int displacement)
            {
                //if we are going north or east, it is a positive change, otherwise we need to subtract
                int directionModifier = ((_direction == Direction.North || _direction == Direction.East) ? 1 : -1);
                int directionalDisplacement = directionModifier * displacement;
                //if moving north or south, change NC, otherwise changing the EC
                if (_direction == Direction.North || _direction == Direction.South)
                {
                    _northCoord += directionalDisplacement;
                }
                else
                {
                    _eastCoord += directionalDisplacement;
                }
            }


            internal void OutputLocation()
            {
                Console.WriteLine(String.Format("Current Position {0}{1}", _northCoord, _eastCoord));
            }

            internal void OutputDisplacement()
            {
                Console.WriteLine(String.Format("Currently {0} away from origin", Math.Abs(_northCoord) + Math.Abs(_eastCoord)));
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
}
