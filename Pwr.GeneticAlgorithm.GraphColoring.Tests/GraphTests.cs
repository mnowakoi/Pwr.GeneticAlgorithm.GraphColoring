using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Pwr.GeneticAlgorithm.GraphColoring.Tests
{
    [TestClass]
    public class GraphTests
    {
        [TestMethod]
        public void LoadFromFileTest()
        {
            var graph = new Graph();
            graph.ReadFromFile(@"c:\users\monika\documents\visual studio 2013\Projects\Pwr.GeneticAlgorithm.GraphColoring\Pwr.GeneticAlgorithm.GraphColoring\GraphSamples\david.col");

            Assert.IsNotNull(graph.GraphNodes);
            Assert.IsNotNull(graph.GraphNodes[0]);
            Assert.IsTrue(graph.GraphNodes.Count == 87);
            Assert.IsTrue(graph.GraphNodes[0].Count == 6);
        }
    }
}
