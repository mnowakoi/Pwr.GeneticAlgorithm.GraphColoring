using System;
using System.Linq;

namespace Pwr.GeneticAlgorithm.GraphColoring
{
    class BruteForceSolver
    {
        private readonly Graph _graph;
        private readonly int _colors;
        private readonly int[] _result;
        private int _fitness;

        public BruteForceSolver(Graph graph, int colors)
        {
            _graph = graph;
            var nodes = graph.GraphNodes.Count;
            _colors = colors;
            _result = new int[nodes];
        }

        public BruteForceSolver(Graph graph)
        {
            _graph = graph;
            var nodes = graph.GraphNodes.Count;
            _colors = graph.GetInitialNumberOfColors();
            _result = new int[nodes];
        }

        public int Resolve()
        {
            int i;
            var isFound = true;
            var colors = _colors;
            for (i = colors - 1; i > 0 && isFound; i--)
            {
                var bf = new BruteForceSolver(_graph, i + 1);
                if (bf.TrySolve() > 0)
                {
                    isFound = false;
                }
            }
            return i;
        }

        public int TrySolve()
        {
            _fitness = 100;
            for (var i = 0; i < 1000000 && _fitness != 0; i++)
            {
                GenerateResult();
                EvaluateResult(_graph);
            }
            return _fitness;
        }

        public void GenerateResult()
        {
            var sr = new Random();
            for (var j = 0; j < _result.Length; j++)
                _result[j] = sr.Next(_colors);
        }

        public void EvaluateResult(Graph graph)
        {
            var conflicts = 0;
            for (var i = 0; i < _result.Length; i++)
            {
                var current = _result[i];
                var edgesArr = graph.GraphNodes[i];
                conflicts += edgesArr.Count(t => _result[t] == current);
            }
            _fitness = conflicts / 2;
        }
    }
}
