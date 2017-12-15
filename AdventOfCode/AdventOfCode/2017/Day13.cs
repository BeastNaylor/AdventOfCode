using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode._2017
{
    class Day13 : DayProgram
    {
        public override void Run(string part)
        {
            var input = FileReader.ReadFile(2017, 13);

            if (part == "A")
            {
                var ss = new SecuritySystem(input);
                ss.SetPacket(0);
                ss.TraversePacket();
                Console.Write(ss.Severity);
            }
            else
            {
                var delay = 0;
                var severity = 0;
                do
                {
                    delay++;
                    var ss = new SecuritySystem(input);
                    ss.SetPacket(delay);
                } while (severity == 0);
                Console.Write(delay);
            }
        }

        private class SecuritySystem
        {
            Dictionary<int, SecurityScanner> _scanners = new Dictionary<int, SecurityScanner>();
            private int _packetDepth = 0;
            private int _severity = 0;

            public int Severity { get { return _severity; } }

            public SecuritySystem(string input)
            {
                foreach (string line in input.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
                {
                    var regEx = @"(\d+): (\d+)";
                    var matches = Regex.Match(line, regEx);
                    var position = matches.Groups[1].Value;
                    var scanner = new SecurityScanner(int.Parse(matches.Groups[2].Value));
                    _scanners.Add(int.Parse(position), scanner);
                }
            }

            public void SetPacket(int delay)
            {
                for(int ii = 1; ii < delay; ii++) {
                    this.Tick();
                }
            }

            public void TraversePacket()
            {
                //keep ticking until the packet makes it past the last scanner
                while (_packetDepth < _scanners.Keys.Max())
                {
                    Tick();
                }
            }

            private void Tick()
            {
                //tick all the security scanners
                foreach (SecurityScanner scanner in _scanners.Values)
                {
                    scanner.Tick();
                }
                //then tick the packet and check if there is a scanner at this depth
                _packetDepth++;
                if (_scanners.ContainsKey(_packetDepth))
                {
                    //if there is, check if the scanner is at 0
                    if (_scanners[_packetDepth].Position == 0)
                    {
                        _severity += (_scanners[_packetDepth].Size * _packetDepth);
                    }
                }
            }
        }

        private class SecurityScanner
        {
            private int _size;
            private int _position = 0;
            private int _direction = 1;
            public SecurityScanner(int size)
            {
                _size = size;
            }

            public int Position
            {
                get { return _position; }
            }
            public int Size
            {
                get { return _size; }
            }

            public void Tick()
            {
                _position += _direction;
                if (_position == 0) { _direction = 1; }
                if (_position == _size - 1) { _direction = -1; }
            }
        }
    }
}
