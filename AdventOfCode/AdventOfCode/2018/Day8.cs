using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2018
{
    class Day8 : DayProgram
    {
        public override void Run(string part)
        {
            var fileText = FileReader.ReadFile(2018, 8);

            var parser = new Navigator(fileText);
            Console.WriteLine(parser.SumMetaData(0).MetaDataSum);
            Console.WriteLine(parser.SumMetaDataB(0).MetaDataSum);
        }

        private class Navigator
        {
            List<int> nodeInfo;
            public Navigator(string input)
            {
                nodeInfo = input.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).ToList<int>();
            }

            public NodeResultInfo SumMetaData(int index)
            {
                NodeResultInfo result = new NodeResultInfo();
                int metaDataSum = 0;
                int nodeLength = 2;
                var numNodes = nodeInfo[index];
                var metaDataCount = nodeInfo[index + 1];
                var nodeOffset = 0;
                for (int numIndex = 0; numIndex <= numNodes - 1; numIndex++)
                {
                    var nodeResult = SumMetaData(index + nodeLength + nodeOffset);
                    nodeOffset += nodeResult.NodeLength;
                    metaDataSum += nodeResult.MetaDataSum;
                }
                for (int metaIndex = 0; metaIndex <= metaDataCount - 1; metaIndex++)
                {
                    metaDataSum += nodeInfo[index + 2 + nodeOffset + metaIndex];
                }
                result.NodeLength = nodeLength + nodeOffset + metaDataCount;
                result.MetaDataSum = metaDataSum;
                return result;
            }

            public NodeResultInfo SumMetaDataB(int index)
            {
                NodeResultInfo result = new NodeResultInfo();
                int metaDataSum = 0;
                int nodeLength = 2;
                var numNodes = nodeInfo[index];
                var metaDataCount = nodeInfo[index + 1];
                var nodeOffset = 0;
                var childNodes = new List<NodeResultInfo>();
                if (numNodes > 0)
                {
                    for (int numIndex = 0; numIndex <= numNodes - 1; numIndex++)
                    {
                        var nodeResult = SumMetaDataB(index + nodeLength + nodeOffset);
                        nodeOffset += nodeResult.NodeLength;
                        childNodes.Add(nodeResult);
                    }
                    for (int metaIndex = 0; metaIndex <= metaDataCount - 1; metaIndex++)
                    {
                        var childNodeIndex = nodeInfo[index + 2 + nodeOffset + metaIndex];
                        if (childNodes.Count >= childNodeIndex)
                        {
                            metaDataSum += childNodes[childNodeIndex-1].MetaDataSum;
                        }
                    }
                } else
                {
                    for (int metaIndex = 0; metaIndex <= metaDataCount - 1; metaIndex++)
                    {
                        metaDataSum += nodeInfo[index + 2 + nodeOffset + metaIndex];
                    }
                }

                result.NodeLength = nodeLength + nodeOffset + metaDataCount;
                result.MetaDataSum = metaDataSum;
                return result;
            }

            public class NodeResultInfo
            {
                public int MetaDataSum;
                public int NodeLength;
            }
        }
    }
}
