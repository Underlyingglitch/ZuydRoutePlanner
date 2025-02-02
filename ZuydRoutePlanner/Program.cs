using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ZuydRoutePlanner
{
	internal class Program
    {
        static void Main(string[] args)
        {
            var graph = Generator.GenerateGraph();

            // == CONFIG == 
            bool requireAccessible = false;
            bool emergencyMode = false;
            bool startByCar = false;
            List<Node> via = new List<Node>();

            Node start = graph.FindNode("Bus");
            Node end = graph.FindNode("Centrale Hal");
            via.Add(graph.FindNode("P1"));

            var (shortestPathRecursive, totalDistanceRecursive) = Dijkstra.FindShortestPathViaPoints(graph, start, end, via, requireAccessible, emergencyMode, startByCar);

            foreach ( Node node in shortestPathRecursive)
            {
                Console.WriteLine(node.Name);
            }
            Console.WriteLine();
            Console.WriteLine(totalDistanceRecursive.ToString());

            // == Test different from and to's == 
            //var testCases = new List<(string from, string to)>
            //{
            //	("Bus", "Centrale Hal"),
            //	("Hoofdingang", "Nieuw Eyckholt"),
            //	("Centrale Hal", "Brug"),
            //	("P1", "C.0.100"),
            //	("Bus", "Pad"),
            //	("Brug", "Trappen Centrale Hal")
            //};

            //foreach (var (from, to) in testCases)
            //{
            //             Console.WriteLine($"Testen van: {from} naar {to}");
            //             Node start = graph.FindNode(from);
            //             Node end = graph.FindNode(to);

            //             // == Print Input Config == 
            //             Console.WriteLine($"Van: {from}");
            //             Console.WriteLine($"Naar: {to}");
            //             Console.WriteLine($"Toegankelijkheid: {(requireAccessible ? "Rolstoel" : "Normaal")}");
            //             Console.WriteLine($"Noodmodus: {(emergencyMode ? "Ja" : "Nee")}");
            //             Console.WriteLine($"Start met auto: {(startByCar ? "Ja" : "Nee")}");
            //             Console.WriteLine("\n---");

            //             // Time non-recursive method
            //             Stopwatch stopwatch = Stopwatch.StartNew();
            //             var (shortestPathNonRecursive, totalDistanceNonRecursive) = Dijkstra.FindShortestPath(graph, start, end, requireAccessible, emergencyMode, startByCar);
            //             stopwatch.Stop();
            //             long nonRecursiveTime = stopwatch.ElapsedTicks; // Get time in ticks

            //             // Time recursive method
            //             stopwatch.Restart();
            //             var (shortestPathRecursive, totalDistanceRecursive) = Dijkstra.FindShortestPathRec(graph, start, end, requireAccessible, emergencyMode, startByCar);
            //             stopwatch.Stop();
            //             long recursiveTime = stopwatch.ElapsedTicks; // Get time in ticks

            //             // == Output Results == 
            //             Console.WriteLine($"\nNon-recursive method completed in: {nonRecursiveTime} ticks");
            //             Console.WriteLine($"Recursive method completed in: {recursiveTime} ticks");

            //             // == Path and Distance Output
            //             Console.WriteLine($"Time difference: {Math.Abs(nonRecursiveTime - recursiveTime)} ticks");

            //             Console.WriteLine("\n---");
            //             Console.WriteLine($"\nTotal distance: {totalDistanceNonRecursive}");
            //         }
        }
    }
}
