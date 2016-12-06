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
            routeManager.OutputFirstIntersection();
        }

        private class RouteManager
        {

            private int _northCoord = 0;
            private int _eastCoord = 0;
            private Direction _direction = Direction.North;
            private List<Tuple<int, int>> _locations;
            private List<Tuple<int, int>> _intersections;

            public RouteManager(int northCoord, int eastCoord)
            {
                _northCoord = northCoord;
                _eastCoord = eastCoord;
                OutputLocation();
                _locations = new List<Tuple<int, int>>(){new Tuple<int, int>(_northCoord, _eastCoord)};
                _intersections = new List<Tuple<int, int>>();
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
                    recordLocations(_northCoord, directionalDisplacement, directionModifier, Direction.North);
                }
                else
                {
                    recordLocations(_eastCoord, directionalDisplacement, directionModifier, Direction.East);
                }
            }

            private void recordLocations(int origPos, int movement, int step, Direction axis)
            {
                //keep track of the newPos we are moving along
                var newPos = origPos;
                do
                {
                    //move along the axis
                    newPos += step;

                    Tuple<int, int> tuple = null;
                    if (axis == Direction.North)
                    {
                        _northCoord = newPos;

                    }
                    else if (axis == Direction.East)
                    {
                        _eastCoord = newPos;
                    }
                    tuple = new Tuple<int, int>(_northCoord, _eastCoord);
                    //check if the locations contains this new location
                    if (_locations.Contains(tuple))
                    {
                        Console.WriteLine(String.Format("This Location has already been visited N{0}E{1}", _northCoord, _eastCoord));
                        _intersections.Add(tuple);
                    }
                    else
                    {
                        _locations.Add(tuple);
                    }
                //keep moving until we are are the original + movement position
                } while (newPos != (origPos + movement));
            }


            internal void OutputLocation()
            {
                Console.WriteLine(String.Format("Current Position N{0}E{1}", _northCoord, _eastCoord));
            }

            internal void OutputDisplacement()
            {
                Console.WriteLine(String.Format("Currently {0} away from origin", Math.Abs(_northCoord) + Math.Abs(_eastCoord)));
            }

            internal void OutputFirstIntersection()
            {
                var firstIntersection = _intersections.First();
                Console.Write(String.Format("First Intersection at N{0}E{1} was {2} away from origin", firstIntersection.Item1, firstIntersection.Item2, Math.Abs(firstIntersection.Item1) + Math.Abs(firstIntersection.Item2)));
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
