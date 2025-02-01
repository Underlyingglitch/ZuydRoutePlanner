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
			string from = "Bus";
			string to = "Centrale Hal";
			bool requireAccessible = true;
			bool emergencyMode = false;
			bool startByCar = true;

			// == CODE STARTS HERE == 
			Node start = graph.FindNode(from);
			Node end = graph.FindNode(to);

			// == Print Input Config ==
			Console.WriteLine($"Van: {from}");
			Console.WriteLine($"Naar: {to}");
			Console.WriteLine($"Toegankelijkheid: {(requireAccessible ? "Rolstoel" : "Normaal")}");
			Console.WriteLine($"Noodmodus: {(emergencyMode ? "Ja" : "Nee")}");
			Console.WriteLine($"Start met auto: {(startByCar ? "Ja" : "Nee")}");
			Console.WriteLine("\n---");

			// Time non-recursive method
			Stopwatch stopwatch = Stopwatch.StartNew();
			var (shortestPathNonRecursive, totalDistanceNonRecursive) = FindShortestPathNonRecursive(graph, start, end, requireAccessible, emergencyMode, startByCar);
			stopwatch.Stop();
			long nonRecursiveTime = stopwatch.ElapsedTicks; // Get time in ticks

			// Time recursive method
			stopwatch.Restart();
			var (shortestPathRecursive, totalDistanceRecursive) = FindShortestPathRecursive(graph, start, end, requireAccessible, emergencyMode, startByCar);
			stopwatch.Stop();
			long recursiveTime = stopwatch.ElapsedTicks; // Get time in ticks

			// == Output Results ==
			Console.WriteLine($"\nNon-recursive method completed in: {nonRecursiveTime} ticks");
			Console.WriteLine($"Recursive method completed in: {recursiveTime} ticks");

			// == Path and Distance Output
			
			Console.WriteLine($"Time difference: {Math.Abs(nonRecursiveTime - recursiveTime)} ticks");

			Console.WriteLine("\n---");
			Console.WriteLine($"\nTotal distance: {totalDistanceNonRecursive}");


		}

		// Non-recursive Dijkstra
		static (List<Node> path, double totalDistance) FindShortestPathNonRecursive(Graph graph, Node start, Node end, bool requireAccessible, bool emergencyMode, bool startByCar)
		{
			var (shortestPath, totalDistance) = Dijkstra.FindShortestPath(graph, start, end, requireAccessible, emergencyMode, startByCar);
			return (shortestPath, totalDistance);
		}

		// Recursive Dijkstra
		static (List<Node> path, double totalDistance) FindShortestPathRecursive(Graph graph, Node start, Node end, bool requireAccessible, bool emergencyMode, bool startByCar)
		{
			var (shortestPath, totalDistance) = Dijkstra.FindShortestPathRec(graph, start, end, requireAccessible, emergencyMode, startByCar);
			return (shortestPath, totalDistance);
		}
	}
}
