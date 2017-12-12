using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode._2017
{
    class Day12 : DayProgram
    {
        public override void Run(string part)
        {
            var input = FileReader.ReadFile(2017, 12);
            var portIdentifier = new PortIdentifier(input);
            if (part == "A")
            {
                Console.WriteLine(portIdentifier.GetConnectionsToRoot().Count);
            }
            else
            {
                Console.WriteLine(portIdentifier.GetGroups());
            }
        }

        private class PortIdentifier
        {
            private Dictionary<int, List<int>> _mappings = new Dictionary<int, List<int>>();
            public PortIdentifier(string input)
            {
                foreach (string line in input.Split(new[] { Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries))
                {
                    //for each line, take the port and add the list of connected ports
                    var regEx = @"(\d+) <-> ([\d\s,]+)";
                    var matches = Regex.Match(line, regEx);
                    int port = int.Parse(matches.Groups[1].Value);
                    string connectionString = matches.Groups[2].Value;
                    List<int> connections = new List<int>();
                    foreach (string connection in connectionString.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        connections.Add(int.Parse(connection));
                    }
                    _mappings.Add(port, connections);
                }
            }

            public List<int> GetConnectionsToRoot() {
                var connections = new List<int>();
                connections.Add(0);
                var toCheckPorts = new List<int>();
                toCheckPorts.Add(0);
                while (toCheckPorts.Any())
                {
                    var portToCheck = toCheckPorts.First();
                    var newConnections = _mappings[portToCheck].Except(connections);
                    toCheckPorts.AddRange(newConnections);
                    connections.AddRange(newConnections);

                    toCheckPorts.Remove(portToCheck);
                }
                return connections;
            }

            public int GetGroups()
            {
                var groupCount = 0;
                var portsInGroup = new List<int>();
                foreach (int port in _mappings.Keys)
                {
                    if (!portsInGroup.Contains(port))
                    {
                        groupCount++;
                        var connections = new List<int>();
                        connections.Add(port);
                        var toCheckPorts = new List<int>();
                        toCheckPorts.Add(port);
                        while (toCheckPorts.Any())
                        {
                            var portToCheck = toCheckPorts.First();
                            var newConnections = _mappings[portToCheck].Except(connections).ToList();
                            toCheckPorts.AddRange(newConnections);
                            connections.AddRange(newConnections);
                            portsInGroup.AddRange(newConnections);

                            toCheckPorts.Remove(portToCheck);
                        }
                    }
                }
                return groupCount;
            }
        }
    }
}
