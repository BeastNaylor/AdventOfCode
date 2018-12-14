using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2018
{
    class Day9 : DayProgram
    {
        public override void Run(string part)
        {
            Console.WriteLine(MarbleGame.CalculateHighScore(9, 25));
            Console.WriteLine(MarbleGame.CalculateHighScore(10, 1618));
            Console.WriteLine(MarbleGame.CalculateHighScore(448, 71628*100));
            
        }

        private static class MarbleGame
        {
            public static int CalculateHighScore(int players, int finalMarble)
            {
                var scores = new Dictionary<int, int>();
                for(int playerIndex = 0; playerIndex <= players - 1; playerIndex++)
                {
                    scores.Add(playerIndex, 0);
                }
                var currPlayer = 0;
                var marblePositions = new List<int>();
                marblePositions.Add(0);
                marblePositions.Add(1);
                var currMarbleIndex = 1;
                for (int marbleCount = 2; marbleCount <= finalMarble; marbleCount++)
                {
                    if (marbleCount % 100000 == 0)
                    {
                        Console.WriteLine($"{marbleCount} at {DateTime.Now.ToString()}");
                    }
                    if (marbleCount % 23 == 0)
                    {
                        scores[currPlayer] += marbleCount;

                        var removalIndex = ((currMarbleIndex - 7 + marblePositions.Count) % marblePositions.Count);
                        scores[currPlayer] += marblePositions[removalIndex];
                        marblePositions.RemoveAt(removalIndex);
                        currMarbleIndex = removalIndex;

                    } else
                    {
                        var newIndex = (currMarbleIndex + 2) % marblePositions.Count;
                        marblePositions.Insert(newIndex, marbleCount);
                        currMarbleIndex = newIndex;
                    }
                    currPlayer = (currPlayer + 1) % players;
                }

                return scores.Max(x => x.Value);
            }
        }
    }
}
