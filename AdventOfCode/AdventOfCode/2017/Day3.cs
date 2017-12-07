using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2017
{
    class Day3 : DayProgram
    {
        public override void Run(string part)
        {
            int input = 368078;
            if (part == "A")
            {
                Console.WriteLine(SpiralMatrixDistance.DetermineDistance(input));
            }
            else
            {
                var matrixValues = new SpiralMatrixValues();
                var cellValue = 0;
                do
                {
                    cellValue = matrixValues.NextCell();
                } while (cellValue < input);
                Console.WriteLine(cellValue);
            }

        }

        private class SpiralMatrixValues
        {
            private Dictionary<Tuple<int, int>, int> _locations = new Dictionary<Tuple<int, int>, int>();
            private Tuple<int, int> _currentLocation = new Tuple<int, int>(0, 0);
            Direction direction;
            int distance = 0;
            int maxDistance = 1;

            private enum Direction
            {
                Right = 0,
                Up = 1,
                Left = 2,
                Down = 3
            }

            public SpiralMatrixValues()
            {
                _locations.Add(_currentLocation, 1);
                //move to the initial position and increase the looping distance
                //start pointing up as we have already moved left
                direction = Direction.Right;
                distance = 0;
            }

            public int NextCell()
            {
                MoveToNextLocation();
                int cellValue = DetermineCellValue();
                _locations.Add(_currentLocation, cellValue);
                return cellValue;
            }

            private void MoveToNextLocation() {
                //from the current value, determine the direction and move that many
                var xPos = _currentLocation.Item1;
                var yPos = _currentLocation.Item2;
                var movement = (direction == Direction.Right || direction == Direction.Up) ? 1 : -1;
                if (direction == Direction.Up || direction == Direction.Down)
                {
                    yPos += movement;
                }
                else
                {
                    xPos += movement;
                }
                _currentLocation = new Tuple<int, int>(xPos, yPos);
                //after movement, change the distance you have moved and spin if needed
                //change the direction and MOD division to get back within our range
                distance += 1;
                if (distance == maxDistance)
                {
                    int newDirectionInt = (((int)direction + 1) % 4); ;
                    direction = (Direction)newDirectionInt;
                    distance = 0;
                    //if we are facing Left or Right, we need to move one extra space
                    if (direction == Direction.Left || direction == Direction.Right) { maxDistance += 1; }
                }
            }



            private int DetermineCellValue()
            {
                
                int cellValue = 0;
                //get the values in the eight adjacent cells (if they exist), sum them and save them in the cell
                for (int ii = _currentLocation.Item1 - 1; ii <= _currentLocation.Item1 + 1; ii++)
                {
                    for (int jj = _currentLocation.Item2 - 1; jj <= _currentLocation.Item2 + 1; jj++)
                    {
                        var key = new Tuple<int,int>(ii,jj);
                        if (_locations.Keys.Contains(key))
                        {
                            cellValue += _locations[key];
                        }
                    }
                }
                return cellValue;
            }
        }

        private class SpiralMatrixDistance
        {
            public static int DetermineDistance(int location)
            {
                //first, determine the shell in which the number lies
                //find the squares of odd numbers until one is larger than our number
                var maxShellNumber = 0;
                var ii = 1;
                do
                {
                    ii += 2;
                    maxShellNumber = ii * ii;
                } while (maxShellNumber < location);
                //now we have the outer shell, and the square number, we can need to determine the distance to the center line, and the center-line distance from the entry point
                //for distance to center, work out the length of one side and half it
                int sideLength = (ii - 1);
                int centrePoint = sideLength / 2;
                //now we subtract the location from the previous shell, to get the total number in the outer shell
                int secondOuterShell = (int)Math.Pow((ii - 2), 2);
                int outerShellCount = location - secondOuterShell;
                int rowRemainder = outerShellCount % sideLength;
                //take the modulus of the rowRemainder minus the centrePoint to find the distance to the centrePoint from the location
                int outerShellDistance = Math.Abs(rowRemainder - centrePoint);

                int centrePointToEntryDistance = (ii - 1) / 2;

                return outerShellDistance + centrePointToEntryDistance;
            }
        }
    }
}
