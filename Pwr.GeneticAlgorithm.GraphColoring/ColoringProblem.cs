using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Pwr.GeneticAlgorithm.GraphColoring
{
    class ColoringProblem
    {
        private Chromosome[] _population;
        private readonly Graph _graph;
        private readonly int _nodes;
        private readonly int _colors;
        private readonly int _maxIterations;
        private readonly Random _randomGenerator;
        private readonly double _crossoverProbability;
        private readonly double _mutationProbability;
        // private readonly ToFilePrinter _printer;
        
        public ColoringProblem(Graph graph, int populationSize, double crossoverProbability, double mutationProbability, int maxIterations)
        {
            _graph = graph;
            _crossoverProbability = crossoverProbability;
            _mutationProbability = mutationProbability;
            _maxIterations = maxIterations;
            _nodes = graph.GraphNodes.Count;
            _colors = graph.GetInitialNumberOfColors();
            _population = new Chromosome[populationSize];
            _randomGenerator = new Random();
            // _printer = new ToFilePrinter();
        }

        public int ColorGraph()
        {
            var minimumColorsCount = 35;
            while (TryColor(minimumColorsCount))
            {
                minimumColorsCount--;
            }
            return minimumColorsCount + 1;
        }

        private bool TryColor(int colorsCount)
        {
            var iterationsLeft = _maxIterations;
            GenerateFirstPopulation(colorsCount);
            EvaluatePopulation();
            var fittest = Evolve();
            var fit = new Chromosome(100);
            while (fittest.Fitness != 0 && iterationsLeft > 0)
            {
                fittest = Evolve();
                fit = fittest.GetTwin();
                //if (iterationsLeft%20 == 0)
                //{
                Console.WriteLine("najlepszy {0}; pozostalo {1}; najgorszy {2}; srednia {3};   {4}", FindBestFitness(), iterationsLeft, FindWorstFitness(), FindAvgFitness(),colorsCount);
           //}
                iterationsLeft--;
            }
            fit = fit;
         //   Console.WriteLine("najlepszy {0}; pozostalo {1}; najgorszy {2}; {3}", FindBestFitness(), iterationsLeft, FindWorstFitness(), colorsCount);
            return fittest.Fitness == 0;
        }

        private Chromosome Evolve()
        {
            Select();
            CrossoverPopulation();
            MutatePopulation();
            EvaluatePopulation();
            var fittest = FindFittest();
            return fittest;
        }

        private int FindBestFitness()
        {
            return FindFittest().Fitness;
        }

        private Chromosome FindFittest()
        {
            var fittest = _population[0];
            return _population.Aggregate(fittest, (current, chromosome) => current.Fitness <= chromosome.Fitness ? current : chromosome);
        }

        private int FindWorstFitness()
        {
            var worst = _population[0];
            worst = _population.Aggregate(worst, (current, chromosome) => current.Fitness >= chromosome.Fitness ? current : chromosome);
            return worst.Fitness;
        }

        private double FindAvgFitness()
        {
            double sum = _population.Aggregate<Chromosome, double>(0, (current, chromosome) => current + chromosome.Fitness);
            return sum/_population.Length;
        }

        private void EvaluatePopulation()
        {
            foreach (var chromosome in _population)
            {
                chromosome.Evaluate(_graph);
            }
        }

        private void Select()
        {
            var range = _population.Length;
            var selected = new Chromosome[range];

            var best = _population[0];
            for (var i = 0; i < range; i++)
            {
                best = _population[i].Fitness < best.Fitness ? _population[i] : best;
            }

            selected[0] = best.GetTwin();

            for (var i = 1; i < range; i++)
            {
                var candidateIndex1 = _randomGenerator.Next(range);
                var candidateIndex2 = _randomGenerator.Next(range - 1);
                candidateIndex2 = candidateIndex2 >= candidateIndex1 ? candidateIndex2 + 1 : candidateIndex2;
                Assert.AreNotEqual(candidateIndex1, candidateIndex2);

                var selectedChromosome = _population[candidateIndex1].Fitness <= _population[candidateIndex2].Fitness
                    ? _population[candidateIndex1]
                    : _population[candidateIndex2];

                selected[i] = selectedChromosome.GetTwin();
            }

            _population = selected;
        }
        
        private void CrossoverPopulation()
        {
            var range = _population.Length;
            var children = new Chromosome[range];
            for (var i = 0; i < _population.Length; i++)
            {
                if (_randomGenerator.NextDouble() < _crossoverProbability)
                {
                    var candidateIndex2 = _randomGenerator.Next(range - 1);
                    candidateIndex2 = candidateIndex2 >= i ? candidateIndex2 + 1 : candidateIndex2;

                    Assert.AreNotEqual(candidateIndex2, i);

                    children[i] = Crossover(_population[i], _population[candidateIndex2]);
                }
                else
                {
                    children[i] = _population[i].GetTwin();
                }
            }
            _population = children;
        }

        private Chromosome Crossover(Chromosome mother, Chromosome father)
        {
            Assert.AreEqual(mother.Colors, father.Colors);
            var leftChild = new Chromosome(_nodes, mother.Colors);
            var rightChild = new Chromosome(_nodes, mother.Colors);
            var cut = _randomGenerator.Next(mother.Genes.Length - 1);
            for (var i = 0; i < _nodes; i++)
            {
                leftChild.Genes[i] = i < cut / 2 ? mother.Genes[i] : father.Genes[i];
                rightChild.Genes[i] = i < cut / 2 ? father.Genes[i] : mother.Genes[i];
            }
            return _randomGenerator.NextDouble() < 0.5 ? leftChild : rightChild;
        }

        private void MutatePopulation()
        {
            foreach (var chromosome in _population)
            {
                chromosome.Mutate(_graph, _mutationProbability, _randomGenerator);
            }
        }

        private void GenerateFirstPopulation(int colorsCount)
        {
            for (var i = 0; i < _population.Length; i++)
            {
                _population[i] = new Chromosome(_nodes);
                _population[i].GenerateGenes(colorsCount, _randomGenerator);
            }
        }
    }
}
