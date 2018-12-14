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
            Console.WriteLine(MarbleGame.CalculateHighScore(21, 6111));
            Console.WriteLine(MarbleGame.CalculateHighScore(448, 7162800));

        }

        private static class MarbleGame
        {
            public static long CalculateHighScore(int players, int finalMarble)
            {
                var scores = new Dictionary<int, long>();
                for (int playerIndex = 0; playerIndex <= players - 1; playerIndex++)
                {
                    scores.Add(playerIndex, 0);
                }
                var currPlayer = 0;
                DoubleLinkNode currNode = new DoubleLinkNode() { Value = 0 };
                currNode.Prev = currNode;
                currNode.Next = currNode;
                currNode.AddAfter(1);
                for (int marbleCount = 2; marbleCount <= finalMarble; marbleCount++)
                {
                    if (marbleCount % 23 == 0)
                    {
                        scores[currPlayer] += marbleCount;

                        currNode = currNode.MoveBackward(7);
                        scores[currPlayer] += currNode.Value;
                        currNode = currNode.Remove();
                    }
                    else
                    {
                        currNode = currNode.MoveForward(1);
                        currNode = currNode.AddAfter(marbleCount);
                    }
                    currPlayer = (currPlayer + 1) % players;
                }

                return scores.Max(x => x.Value);
            }
        }

        private class DoubleLinkNode
        {
            public DoubleLinkNode Prev;
            public DoubleLinkNode Next;
            public int Value;

            public DoubleLinkNode AddAfter(int value)
            {
                var node = new DoubleLinkNode() { Value = value };
                node.Prev = this;
                node.Next = this.Next;

                this.Next.Prev = node;
                this.Next = node;
                return node;
            }

            public DoubleLinkNode Remove()
            {
                this.Prev.Next = this.Next;
                this.Next.Prev = this.Prev;
                return this.Next;
            }

            public DoubleLinkNode MoveForward(int moveDistance)
            {
                var currNode = this;
                for(int distance = 0; distance < moveDistance; distance++)
                {
                    currNode = currNode.Next;
                }
                return currNode;
            }

            public DoubleLinkNode MoveBackward(int moveDistance)
            {
                var currNode = this;
                for (int distance = 0; distance < moveDistance; distance++)
                {
                    currNode = currNode.Prev;
                }
                return currNode;
            }
        }
    }
}
