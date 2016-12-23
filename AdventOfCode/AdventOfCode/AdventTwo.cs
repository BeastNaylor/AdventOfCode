using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    class AdventTwo
    {
        public static void Run()
        {
            var instructions = Properties.Resources.AdventTwo;
            var numPad = new NumPadManager("B");
            numPad.OutputCurrentPosition();
            foreach (string row in instructions.Split(new string[] { Environment.NewLine }, StringSplitOptions.None))
            {
                foreach (char letter in row.ToList())
                {
                    Console.WriteLine(String.Format("Input Received is {0}", letter));
                    switch (letter)
                    {
                        case 'U':
                            numPad.MoveUp();
                            break;
                        case 'D':
                            numPad.MoveDown();
                            break;
                        case 'L':
                            numPad.MoveLeft();
                            break;
                        case 'R':
                            numPad.MoveRight();
                            break;

                    }
                    numPad.OutputCurrentPosition();
                }
                numPad.SaveFinalPosition();
            }


        }

        private class NumPadManager
        {
            //start on the number 5;
            private int _currentRowLocation = 0;
            private int _currentColumnLocation = 0;
            private string _doorCode = "";
            //make a dict of dict to map out all the row/col combinations and the relevant value on the keypad
            private Dictionary<int, Dictionary<int, string>> padKeys = new Dictionary<int, Dictionary<int, string>>();

            public NumPadManager(string type)
            {
                switch (type)
                {
                    case "A":
                        InitialisePartA();
                        break;
                    case "B":
                        InitialisePartB();
                        break;
                    default:
                        break;
                }
            }

            private void InitialisePartA()
            {
                padKeys.Add(1, new Dictionary<int, string>() { { 1, "1" }, { 2, "2" }, { 3, "3" } });
                padKeys.Add(2, new Dictionary<int, string>() { { 1, "4" }, { 2, "5" }, { 3, "6" } });
                padKeys.Add(3, new Dictionary<int, string>() { { 1, "7" }, { 2, "8" }, { 3, "9" } });
                _currentColumnLocation = 2;
                _currentRowLocation = 2;
            }

            private void InitialisePartB()
            {
                padKeys.Add(1, new Dictionary<int, string>() { { 1, null }, { 2, null }, { 3, "1" }, { 4, null }, { 5, null } });
                padKeys.Add(2, new Dictionary<int, string>() { { 1, null }, { 2, "2" }, { 3, "3" }, { 4, "4" }, { 5, null } });
                padKeys.Add(3, new Dictionary<int, string>() { { 1, "5" }, { 2, "6" }, { 3, "7" }, { 4, "8" }, { 5, "9" } });
                padKeys.Add(4, new Dictionary<int, string>() { { 1, null }, { 2, "A" }, { 3, "B" }, { 4, "C" }, { 5, null } });
                padKeys.Add(5, new Dictionary<int, string>() { { 1, null }, { 2, null }, { 3, "D" }, { 4, null }, { 5, null } });
                _currentColumnLocation = 1;
                _currentRowLocation = 3;
            }

            internal void MoveUp()
            {
                //to move up, decrease the current position for the row and check it is within bounds still
                var testRowLocation = _currentRowLocation - 1;
                if (checkLocationValid(testRowLocation, _currentColumnLocation))
                {
                    _currentRowLocation = testRowLocation;
                }
            }

            internal void MoveDown()
            {
                //to move down, increase the current position for the row and check it is within bounds still
                var testRowLocation = _currentRowLocation + 1;
                if (checkLocationValid(testRowLocation, _currentColumnLocation))
                {
                    _currentRowLocation = testRowLocation;
                }
            }

            internal void MoveRight()
            {
                //to move right, increase the col count and check validity
                var testColLocation = _currentColumnLocation + 1;
                if (checkLocationValid(_currentRowLocation, testColLocation))
                {
                    _currentColumnLocation = testColLocation;
                }
            }

            internal void MoveLeft()
            {
                //to move left, decrease column and check
                var testColLocation = _currentColumnLocation - 1;
                if (checkLocationValid(_currentRowLocation, testColLocation))
                {
                    _currentColumnLocation = testColLocation;
                }
            }

            //check if the location is a valid one
            private bool checkLocationValid(int _row, int _col)
            {
                var valid = false;
                var locationValue = getLocationValue(_row, _col);
                if (locationValue != null)
                {
                    valid = true;
                }
                return valid;
            }

            //get the value for a given location
            private string getLocationValue(int _row, int _col)
            {
                string result = null;
                if (padKeys.ContainsKey(_row))
                {
                    if (padKeys[_row].ContainsKey(_col))
                    {
                        result = padKeys[_row][_col];
                    }
                }
                return result;
            }

            internal void OutputCurrentPosition()
            {
                Console.WriteLine(String.Format("Currently at {0}", getLocationValue(_currentRowLocation, _currentColumnLocation)));
            }

            internal void SaveFinalPosition()
            {
                var currentLocation = getLocationValue(_currentRowLocation, _currentColumnLocation);
                _doorCode += currentLocation.ToString();
                Console.WriteLine(String.Format("Final Position for Row is {0}, list is now {1}", currentLocation, _doorCode));
            }
        }
    }
}
