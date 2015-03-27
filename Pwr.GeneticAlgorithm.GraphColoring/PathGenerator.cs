using System;

namespace Pwr.GeneticAlgorithm.GraphColoring
{
    public class PathGenerator
    {
        private const string FilePath =
            @"c:\users\monika\documents\visual studio 2013\Projects\Pwr.GeneticAlgorithm.GraphColoring\Pwr.GeneticAlgorithm.GraphColoring\GraphSamples\";
        
        public static string GetPath(string fileName)
        {
            return String.Format("{0}{1}", FilePath, fileName);
        }
    }
}
