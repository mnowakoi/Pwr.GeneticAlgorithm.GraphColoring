using System;

namespace Pwr.GeneticAlgorithm.GraphColoring
{
    class Program
    {
        static void Main(string[] args)
        {
            const string myciel3 = "myciel3.col";
            const string queen5_5 = "queen5_5.col";
            const string queen12_12 = "queen12_12.col";
            const string queen13_13 = "queen13_13.col";
            const string queen9_9 = "queen9_9.col";
            const string david = "david.col";
            const string homer = "homer.col";
            const string le45025a = "le450_25a.col";
            const string le4505c = "le450_5c.col";
            const string grafMaciek = "graf125736.col";

            var graph = new Graph(PathGenerator.GetPath(queen13_13));
            var problem = new ColoringProblem(graph, 100, 1, 0.001, 1000);
            Console.WriteLine("5: " + problem.ColorGraph());

            //graph = new Graph(PathGenerator.GetPath(queen13_13));
            //problem = new ColoringProblem(graph, 100, 0.5, 0.01, 1000);
            //Console.WriteLine("9: " + problem.ColorGraph());

            //graph = new Graph(PathGenerator.GetPath(homer));
            //problem = new ColoringProblem(graph, 100, 0.8, 0.2, 1000);
            //Console.WriteLine("13: " + problem.ColorGraph());

            //GraphRunner(queen12_12, 100, 0.9, 0.2, 5000);
            //GraphRunner(le45025a, 100, 0.9, 0.2, 5000);
            //   GraphRunner(queen12_12, 100, 0.3, 0.8, 5000);
            //   GraphRunner(le45025a, 100, 0.3, 0.8, 5000);

            //   GraphRunner(queen12_12, 50, 0.8, 0.8, 10000);
            //   GraphRunner(le45025a, 50, 0.8, 0.8, 10000);
            //   GraphRunner(queen12_12, 200, 0.8, 0.8, 1000);
            //   GraphRunner(le45025a, 200, 0.8, 0.8, 1000);

            //var testParams = new[] {myciel3, david, homer, queen12_12, le45025a, grafMaciek};

            //foreach (var param in testParams)
            //{
            //    GraphRunner(param, 100, 0.8, 0.8, 5000);
            //    GraphRunner(param, 100, 0.3, 0.8, 5000);
            //    GraphRunner(param, 100, 0.3, 0.2, 5000);
            //    GraphRunner(param, 100, 0.9, 0.2, 5000);

            //    GraphRunner(param, 50, 0.8, 0.8, 10000);
            //    GraphRunner(param, 20, 0.8, 0.8, 10000);
            //    GraphRunner(param, 200, 0.8, 0.8, 1000);
            //    GraphRunner(param, 300, 0.8, 0.8, 500);
            //}
            
            //var bruteSolver = new BruteForceSolver(graph);
            //Console.WriteLine(bruteSolver.Resolve());
            Console.ReadLine();
        }

        private static void GraphRunner(string fileName, int populationSize, double crossProb, double mutProb, int maxIterations)
        {
            var raportName = string.Format("{0}p{1}k{2}m{3}i{4}.txt", fileName, populationSize, crossProb, mutProb, maxIterations);
            using (var printer = new ToFilePrinter(PathGenerator.GetPath(raportName)))
            {
                var graph = new Graph(PathGenerator.GetPath(fileName));
                var problem = new ColoringProblem(graph, populationSize, crossProb, mutProb, maxIterations);
                double sum = 0;
                for (var i = 0; i < 10; i++)
                {
                    var colors = problem.ColorGraph();
                    printer.WriteLine("iteration: {0}  result:  {1}", i, colors);
                    sum += colors;
                }
                printer.WriteLine("AvgResult: {0}", sum/10);
            }
        }
    }
}
