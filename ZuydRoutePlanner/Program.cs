using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ZuydRoutePlanner
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var graph = Generator.GenerateGraph();

            string from = "Nieuw Eyckholt";
            string to = "Centrale Hal";
            bool requireAccessible = true;
            bool emergencyMode = false;
            bool startByCar = true;
            Node start = graph.FindNode(from);
            Node end = graph.FindNode(to);

            // Zoek naar een pad
            var (shortestPath, totalDistance) = Dijkstra.FindShortestPath(graph, start, end, requireAccessible: requireAccessible, emergencyMode: emergencyMode, startByCar:startByCar);

            Console.WriteLine($"Van: {from}");
            Console.WriteLine($"Naar: {to}");
            Console.WriteLine($"Toegankelijkheid: {(requireAccessible ? "Rolstoel" : "Normaal")}");
            Console.WriteLine($"Noodmodus: {(emergencyMode ? "Ja" : "Nee")}");
            Console.WriteLine($"Start met auto: {(startByCar ? "Ja" : "Nee")}");
            Console.WriteLine("---");

            Console.WriteLine("Kortste pad:");
            foreach (var node in shortestPath)
            {
                if (node.Hidden)
                    // Debug only
                    Console.WriteLine($"(verborgen) {node.Name}");
                else
                    Console.WriteLine(node.Name);
            }
            Console.WriteLine($"Totale afstand: {totalDistance}");
        }
    }
}
