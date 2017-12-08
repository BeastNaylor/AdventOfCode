using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2017
{
    class Day6 : DayProgram
    {
        public override void Run(string part)
        {
            var input = FileReader.ReadFile(2017, 6);
            var memAllocator = new MemoryAllocator(input);
            memAllocator.DistributeUntilSameState();
            Console.WriteLine(memAllocator.interationCount);
            Console.WriteLine(memAllocator.LoopSize());
            }

        private class MemoryAllocator
        {
            private Dictionary<int, int> _memoryBlocks = new Dictionary<int, int>();
            private Dictionary<string, int> _memoryState = new Dictionary<string, int>();
            public int interationCount = 0;

            public MemoryAllocator(string input)
            {
                //parse the initial input into the different memory locations
                var memblockNumber = 0;
                foreach (int blockCount in input.Split(new[] { "\t" }, StringSplitOptions.None).Select(x => int.Parse(x)).ToList())
                {
                    _memoryBlocks.Add(memblockNumber, blockCount);
                    memblockNumber++;
                }
            }

            public void DistributeUntilSameState()
            {
                //save the current state, if it can save (no dupes) we carry on distributing
                while (!CurrMemStateExists())
                {
                    SaveCurrMemState();
                    //find the largest memory block, and empty it
                    KeyValuePair<int, int> largestMemBlock = _memoryBlocks.OrderByDescending(x => x.Value).First();
                    _memoryBlocks[largestMemBlock.Key] = 0;
                    //now, from the largest block, we cycle over the blocks adding 1 in sequence

                    var currBlockAllocate = (largestMemBlock.Key + 1) % _memoryBlocks.Count;
                    for (int ii = 1; ii <= largestMemBlock.Value; ii++)
                    {
                        //take MOD of the blocks, so we start at the beginning again
                        
                        _memoryBlocks[currBlockAllocate] += 1;
                        currBlockAllocate = (currBlockAllocate + 1) % _memoryBlocks.Count;
                    }
                    interationCount++;
                }
            }

            public int LoopSize()
            {
                //currently at the repeated step in the process
                //can find the iteration that this started at, and subtract from the currentIteration to find the size
                var loopStart = _memoryState[GetMemState()];
                return interationCount - loopStart;
            }

            private bool CurrMemStateExists()
            {
                return _memoryState.Keys.Contains(GetMemState());
            }

            private void SaveCurrMemState()
            {
                _memoryState.Add(GetMemState(), interationCount);
            }

            private string GetMemState()
            {
                //copy the states of the memoryblocks into the memstate
                StringBuilder sb = new StringBuilder();
                foreach (KeyValuePair<int, int> kvp in _memoryBlocks)
                {
                    sb.Append("-");
                    sb.Append(kvp.Value.ToString());
                    sb.Append("-");
                }
                return sb.ToString();
            }
        }
    }
}
