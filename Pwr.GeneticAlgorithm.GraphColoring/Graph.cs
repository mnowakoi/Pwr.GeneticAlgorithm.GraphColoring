using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Pwr.GeneticAlgorithm.GraphColoring
{
    public class Graph
    {
        public Graph()
        {
            GraphNodes = new List<List<int>>();
            EdgesCount = 0;
        }

        public Graph(string filePath)
        {
            GraphNodes = new List<List<int>>();
            ReadFromFile(filePath);
        }

        public int EdgesCount { get; private set; }

        public List<List<int>> GraphNodes { get; private set; }

        public void ReadFromFile(string filePath)
        {
            try
            {
                using (var fileReader = new StreamReader(filePath))
                {
                    var line = "init";
                    while (!line.Substring(0, 1).Equals("p"))
                    {
                        line = fileReader.ReadLine();
                    }
                    var subStrings = Regex.Split(line, "[ ]");
                    var nodesCount = int.Parse(subStrings[2]);
                    EdgesCount = int.Parse(subStrings[3]);

                    GraphNodes = new List<List<int>>(nodesCount);
                    for (var i = 0; i < nodesCount; i++)
                    {
                        GraphNodes.Add(new List<int>());
                    }

                    line = fileReader.ReadLine();
                    while (!string.IsNullOrEmpty(line))
                    {
                        subStrings = Regex.Split(line, "[ ]");
                        var node = int.Parse(subStrings[1]) - 1;
                        var neighbour = int.Parse(subStrings[2]) - 1;
                        if (!GraphNodes[node].Contains(neighbour))
                        {
                            GraphNodes[node].Add(neighbour);
                        }
                        if (!GraphNodes[neighbour].Contains(node))
                        {
                            GraphNodes[neighbour].Add(node);
                        }
                        line = fileReader.ReadLine();
                    }
                }
            }
            catch (FileNotFoundException)
            {
            }
        }

        public int GetInitialNumberOfColors()
        {
            var orderedNodesIndexes = GetOrderedNodesIndexes();
            var result = PrepareResultArray();
            ColorLargestFirst(orderedNodesIndexes, result);
            return result.Max();
        }

        private List<int> GetOrderedNodesIndexes()
        {
            var cache = new List<int>(GraphNodes.Count);
            while (cache.Count < GraphNodes.Count)
            {
                var indexSmallest = 0;
                var smallestNeighbour = 0;
                for (var i = 0; i < GraphNodes.Count; ++i)
                {
                    if (cache.Contains(i)) continue;
                    if (smallestNeighbour == 0)
                    {
                        indexSmallest = i;
                        smallestNeighbour = GraphNodes[i].Count;
                    }
                    else if (GraphNodes[i].Count < smallestNeighbour)
                    {
                        indexSmallest = i;
                        smallestNeighbour = GraphNodes[i].Count;
                    }
                }
                cache.Add(indexSmallest);
            }
            return cache;
        }

        private void ColorLargestFirst(List<int> orderedNodesIndexes, int[] result)
        {
            for (var indexLargest = orderedNodesIndexes.Count - 1; indexLargest > 0; indexLargest--)
            {
                var j = 0;
                var color = 1;
                while (j < GraphNodes[indexLargest].Count)
                {
                    if (result[GraphNodes[indexLargest][j]] == color)
                    {
                        j = 0;
                        color++;
                    }
                    else
                    {
                        ++j;
                    }
                }
                result[indexLargest] = color;
            }
        }

        private int[] PrepareResultArray()
        {
            var result = new int[GraphNodes.Count];
            for (var i = 0; i < result.Length; ++i)
            {
                result[i] = -1;
            }
            return result;
        }
    }
}
