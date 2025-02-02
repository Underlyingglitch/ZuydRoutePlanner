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
            string from = "C.3.2";
            string to = "Hoofdingang";
            bool requireAccessible = false;
            bool emergencyMode = true;
            bool startByCar = false;
            bool isOrganiser = true;
            List<Node> via = new List<Node>();
            List<Node> exclude = new List<Node>();

            Node start = graph.FindNode(from);
            Node end = graph.FindNode(to);
            //via.Add(graph.FindNode("C.0.100"));
            //via.Add(graph.FindNode("C.0.103"));
            //exclude.Add(graph.FindNode("Hoofdingang"));

            var (shortestPathRecursive, totalDistanceRecursive) = Dijkstra.FindShortestPathViaPoints(graph, start, end, via, requireAccessible, emergencyMode, startByCar, isOrganiser, exclude);

            Console.WriteLine($"Van: {from}");
            Console.WriteLine($"Naar: {to}");
            Console.WriteLine($"Toegankelijkheid: {(requireAccessible ? "Rolstoel" : "Normaal")}");
            Console.WriteLine($"Noodmodus: {(emergencyMode ? "Ja" : "Nee")}");
            Console.WriteLine($"Start met auto: {(startByCar ? "Ja" : "Nee")}");
            Console.WriteLine($"Organisator: {(isOrganiser ? "Ja" : "Nee")}");
            List<Node> nodes = new List<Node>();
            nodes.Add(start);
            nodes.AddRange(via);
            nodes.Add(end);
            PrintRoute(shortestPathRecursive, totalDistanceRecursive, nodes);

            //// == Test different from and to's == 
            //var testCases = new List<(string from, string to)>
            //{
            //    ("Bus", "Centrale Hal"),
            //    ("Hoofdingang", "Nieuw Eyckholt"),
            //    ("Centrale Hal", "Brug"),
            //    ("P1", "C.0.100"),
            //    ("Bus", "Pad"),
            //    ("Brug", "Trappen Centrale Hal")
            //};

            //foreach (var (from, to) in testCases)
            //{
            //    Console.WriteLine($"Testen van: {from} naar {to}");
            //    Node start = graph.FindNode(from);
            //    Node end = graph.FindNode(to);

            //    // == Print Input Config == 
            //    Console.WriteLine($"Van: {from}");
            //    Console.WriteLine($"Naar: {to}");
            //    Console.WriteLine($"Toegankelijkheid: {(requireAccessible ? "Rolstoel" : "Normaal")}");
            //    Console.WriteLine($"Noodmodus: {(emergencyMode ? "Ja" : "Nee")}");
            //    Console.WriteLine($"Start met auto: {(startByCar ? "Ja" : "Nee")}");
            //    Console.WriteLine("\n---");

            //    // Time non-recursive method
            //    Stopwatch stopwatch = Stopwatch.StartNew();
            //    var (shortestPathNonRecursive, totalDistanceNonRecursive) = Dijkstra.FindShortestPath(graph, start, end, requireAccessible, emergencyMode, startByCar, isOrganiser);
            //    stopwatch.Stop();
            //    long nonRecursiveTime = stopwatch.ElapsedTicks; // Get time in ticks

            //    // Time recursive method
            //    stopwatch.Restart();
            //    var (shortestPathRecursive, totalDistanceRecursive) = Dijkstra.FindShortestPathRecursive(graph, start, end, requireAccessible, emergencyMode, startByCar, isOrganiser);
            //    stopwatch.Stop();
            //    long recursiveTime = stopwatch.ElapsedTicks; // Get time in ticks

            //    // == Output Results == 
            //    Console.WriteLine($"\nNon-recursive method completed in: {nonRecursiveTime} ticks");
            //    Console.WriteLine($"Recursive method completed in: {recursiveTime} ticks");

            //    // == Path and Distance Output
            //    Console.WriteLine($"Time difference: {Math.Abs(nonRecursiveTime - recursiveTime)} ticks");

            //    Console.WriteLine("\n---");
            //    Console.WriteLine($"\nTotal distance: {totalDistanceNonRecursive}");
            //}
        }

        static void PrintRoute(List<Node> _path, double _totalDistance, List<Node> _nodes)
        {
            Console.WriteLine("\nRoute:");
            if (_path.Count == 0)
            {
                Console.WriteLine("No route found");
                return;
            }
            foreach (Node node in _path)
            {
                if (_nodes.Contains(node))
                {
                    // Change color
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                Console.Write(node.Name);
                if (node.Hidden)
                {
                    Console.Write(" (hidden)");
                }
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.WriteLine(_totalDistance.ToString());
        }
    }
}
