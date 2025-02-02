using System;
using System.Collections.Generic;

namespace ZuydRoutePlanner
{
	internal class Program
    {
        static void Main(string[] args)
        {
            var graph = Generator.GenerateGraph();

            // == CONFIG == 
            string from = "C.3.2"; // Name of the node to start from
            string to = "Hoofdingang"; // Name of the node to go to
            bool requireAccessible = false; // Does this route need to be wheelchair accessible?
            bool emergencyMode = true; // Is this route in emergency mode? (Blocks routes such as elevators)
            bool startByCar = false; // Does the user start in a car? (Will not allow non-car-accessible routes until a parking lot is found)
            bool isOrganiser = true; // Is the user an organiser? (Allows for routes that require accessibility, but will not force it)
            List<Node> via = new List<Node>(); // List of nodes to pass through
            List<Node> exclude = new List<Node>(); // List of nodes to exclude

            //via.Add(graph.FindNode("C.0.100"));
            //via.Add(graph.FindNode("C.0.103"));
            //exclude.Add(graph.FindNode("Hoofdingang"));


            // == APPLICATION CODE == 
            Node start = graph.FindNode(from);
            Node end = graph.FindNode(to);
            

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

            //// == TEST CASES == 
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
                // If the node is part of the start, end or via nodes, color it green
                if (_nodes.Contains(node))
                {
                    // Change color
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                Console.Write(node.Name);
                // If the node is hidden, add (hidden) to the name (for debugging purposes)
                // these nodes should not be part of the route shown to the user, but are useful for debugging and front-end development
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
