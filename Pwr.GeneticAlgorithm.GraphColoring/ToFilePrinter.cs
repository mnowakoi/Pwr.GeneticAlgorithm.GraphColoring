using System.IO;

namespace Pwr.GeneticAlgorithm.GraphColoring
{
    public class ToFilePrinter : StreamWriter
    {
        private const string FolderPath =
            @"c:\users\monika\documents\visual studio 2013\Projects\Pwr.GeneticAlgorithm.GraphColoring\Pwr.GeneticAlgorithm.GraphColoring\GraphSamples\results.txt";
        
        public ToFilePrinter(Stream stream)
            : base(stream)
        {
        }

        public ToFilePrinter(string filename = FolderPath)
            : base(filename)
        {
        }
    }
}

