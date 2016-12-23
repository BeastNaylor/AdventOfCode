using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    class AdventThree
    {
        public static void Run()
        {
            var instructions = Properties.Resources.AdventThree;
            var validTriangles = 0;
            foreach (string set in instructions.Split(new string[] { Environment.NewLine }, StringSplitOptions.None))
            {
                //for each line of the file, we need to get all the triangle edges, and sum the two smallest and compare to the longest
                var lEdges = new List<int>();
                foreach (string edge in set.Trim().Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries))
                {
                    var edgeLength = Int32.Parse(edge);
                    lEdges.Add(edgeLength);
                }
                lEdges.Sort();
                if (lEdges[0] + lEdges[1] > lEdges[2]) { validTriangles += 1; }
            }
            Console.WriteLine(String.Format("{0} Valid Triangles", validTriangles));
        }

        public static void RunTwo()
        {
            var instructions = Properties.Resources.AdventThree;
            var validTriangles = 0;
            var triangles = 0;
            var columns = new Dictionary<int, Dictionary<int, int>>();
            for (int ii = 1; ii < 4; ii++)
            {
                columns.Add(ii, new Dictionary<int, int>());
            }
            var rowCount = 1;
            foreach (string set in instructions.Split(new string[] { Environment.NewLine }, StringSplitOptions.None))
            {
                var columnCount = 1;
                var columnRows = new Dictionary<int, int>();
                foreach (string edge in set.Trim().Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries))
                {
                    var edgeLength = Int32.Parse(edge);
                    columns[columnCount].Add(rowCount, edgeLength);
                    columnCount += 1;
                }
                rowCount += 1;
            }
            foreach (Dictionary<int, int> columnData in columns.Values)
            {
                var trianglesLeft = true;
                var rowNum = 1;
                do
                {

                    if (columnData.ContainsKey(rowNum))
                    {
                        var lEdges = new List<int>() { columnData[rowNum], columnData[rowNum + 1], columnData[rowNum + 2] };
                        lEdges.Sort();
                        if (lEdges[0] + lEdges[1] > lEdges[2]) { 
                            validTriangles += 1; 
                        }
                        triangles += 1;
                    }
                    else
                    {
                        trianglesLeft = false;
                    }

                    rowNum += 3;
                } while (trianglesLeft);
            }
            Console.WriteLine(String.Format("{0} Valid Triangles out of {1}", validTriangles, triangles));
        }
    }
}
