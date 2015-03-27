using System;
using System.Linq;

namespace Pwr.GeneticAlgorithm.GraphColoring
{
    public class Chromosome
    {
        private int[] _genes;

        public int[] Genes
        {
            get { return _genes; }
            set { _genes = value; }
        }

        public int Fitness { get; private set; }

        public int Colors { get; private set; }

        public Chromosome(int nodesCount)
        {
            this.Fitness = 0;
            _genes = new int[nodesCount];
        }

        public Chromosome(int nodesCount, int colorsCount)
        {
            this.Fitness = 0;
            _genes = new int[nodesCount];
            this.Colors = colorsCount;
        }

        public Chromosome GetTwin()
        {
            var range = this.Genes.Length;
            var twin = new Chromosome(range);
            for (var i = 0; i < range; i++)
            {
                twin.Genes[i] = this.Genes[i];
            }
            twin.Colors = this.Colors;
            return twin;
        }

        public void GenerateGenes(int colorsCount, Random randomGenerator)
        {
            for (var j = 0; j < _genes.Length; j++)
                _genes[j] = randomGenerator.Next(colorsCount);
            this.Colors = colorsCount;
        }

        /*
         * Cost function - count of edges with conflicted coloring
         */

        public void Evaluate(Graph graph)
        {
            var conflicts = 0;
            for (var i = 0; i < _genes.Length; i++)
            {
                var currentColor = _genes[i];
                var currentNode = graph.GraphNodes[i];
                conflicts += currentNode.Count(neighbour => _genes[neighbour] == currentColor);
            }
            this.Fitness = conflicts/2;
        }

        // nie wiazac mutacji z konfliktem
        public void Mutate(Graph graph, double mutationProbability, Random randomGenerator)
        {
            for (var i = 0; i < _genes.Length; i++)
            {
                if (randomGenerator.NextDouble() < mutationProbability)
                    _genes[i] = randomGenerator.Next(Colors); 
                //var currentColor = _genes[i];
                // var currentNode = graph.GraphNodes[i];
                //var isConflicted = currentNode.Exists(neighbour => _genes[neighbour] == currentColor);
            }
        }
    }
}
