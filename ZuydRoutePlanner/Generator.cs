using System;
using System.Collections.Generic;

namespace ZuydRoutePlanner
{
    public class Generator
    {
        public static Graph GenerateGraph()
        {
            var g = new Graph();

            g.AddNode("Hoofdingang");
            g.AddNode("Centrale Hal");
            g.AddEdge("Hoofdingang", "Centrale Hal", CD(10.5, 10.5));

            g.AddNode("Trappen Centrale Hal");
            g.AddEdge("Hoofdingang", "Trappen Centrale Hal", CD(10.5, 2));
            g.AddEdge("Centrale Hal", "Trappen Centrale Hal", 9);

            g.AddNode("1", _hidden:true);
            g.AddEdge("Centrale Hal", "1", CD(17.5, 10.5));
            g.AddEdge("Trappen Centrale Hal", "1", CD(7.5, 10.5));
            g.AddEdge("Hoofdingang", "1", CD(21, 7));

            g.AddNode("C.0.1"); //Trappenhuis
            g.AddEdge("1", "C.0.1", 4);

            g.AddNode("B.0.5"); // Gang naar B vleugel
            g.AddEdge("1", "B.0.5", 7);

            g.AddNode("C.0.103"); // Collegezaal
            g.AddEdge("1", "C.0.103", CD(10.5, 6));
            g.AddNode("2", _hidden: true);
            g.AddEdge("C.0.103", "2", CD(5, 4));
            g.AddEdge("1", "2", CD(14, 1));

            g.AddNode("3", _hidden: true);
            g.AddEdge("2", "3", 5);
            g.AddEdge("2", "Centrale Hal", CD(2, 12));
            g.AddEdge("3", "Centrale Hal", CD(4, 13));

            g.AddNode("C.0.100"); // Toiletten
            g.AddEdge("C.0.100", "3", 5);
            g.AddNode("C.0.102"); // Toiletten
            g.AddEdge("C.0.102", "3", 7);

            g.AddNode("C.0.110"); // Kantine (hoek)
            g.AddEdge("C.0.110", "3", CD(4, 10));

            g.AddNode("Nieuw Eyckholt");
            g.AddNode("Bus");
            g.AddNode("Brug");
            g.AddNode("Pad");
            g.AddNode("P1", _isParking: true);
            g.AddNode("PG", _isParking:true, _requireAccessible:true);
            g.AddNode("Fietsenstalling");

            g.AddNode("4", _hidden: true);
            g.AddEdge("4", "Hoofdingang", 5);

            g.AddEdge("Bus", "Brug", 20);
            g.AddEdge("Brug", "4", 30, _isAccessible:false);
            g.AddEdge("Brug", "Pad", 15);
            g.AddEdge("Pad", "P1", 30);
            g.AddEdge("Nieuw Eyckholt", "P1", 40, _isAccessibleByCar:true);
            g.AddEdge("Nieuw Eyckholt", "PG", 40, _isAccessibleByCar: true);
            g.AddEdge("Nieuw Eyckholt", "Pad", 10);
            g.AddEdge("PG", "Centrale Hal", 10); // TODO
            g.AddEdge("Fietsenstalling", "Centrale Hal", 10); // TODO
            g.AddEdge("P1", "4", 10, _isAccessible:false);
            g.AddEdge("P1", "PG", 10, _isAccessibleByCar: true);
            g.AddEdge("P1", "Fietsenstalling", 10);
            g.AddEdge("Bus", "Nieuw Eyckholt", 50, _isAccessibleByCar: true);

            g.AddNode("P2", _isParking:true);
            g.AddNode("P3", _isParking:true);
            g.AddEdge("Nieuw Eyckholt", "P2", 100, _isAccessibleByCar: true);
            g.AddEdge("Nieuw Eyckholt", "P3", 150, _isAccessibleByCar: true);
            g.AddEdge("P2", "P3", 50, _isAccessibleByCar: true);
            g.AddEdge("P3", "PG", 100, _isAccessibleByCar: true);
            g.AddEdge("P3", "Fietsenstalling", 100);
            g.AddEdge("P2", "PG", 30);
            g.AddEdge("P2", "Fietsenstalling", 30);

            g.AddNode("C.0.123"); // Kantine
            g.AddNode("5", _hidden: true);
            g.AddEdge("C.0.123", "5", 7);
            g.AddEdge("5", "3", CD(8,8));
            g.AddEdge("5", "Centrale Hal", CD(12,6));
            g.AddEdge("5", "C.0.110", CD(11,4));

            g.AddNode("6", _hidden: true);
            g.AddNode("C.0.2"); // Trappenhuis naast lift
            List<string> levels = new List<string> { "L00", "L0", "L1", "L2", "L3" };
            foreach (var L in levels)
            {
                g.AddNode(L, _requireAccessible:true);
            }
            foreach (var L in levels)
            {
                // Make a connection between L and all other L's
                foreach (var L2 in levels)
                {
                    if (L != L2)
                    {
                        g.AddEdge(L, L2, 4, _isEmergencyBlocked: true);
                    }
                }
            }

            g.AddEdge("L0", "6", CD(4,2));
            g.AddEdge("6", "C.0.2", 4);
            g.AddEdge("6", "Centrale Hal", CD(16, 10));

            g.AddNode("C.1.2"); // Trappenhuis naast lift
            g.AddEdge("C.1.2", "L1", 4);
            g.AddEdge("C.1.2", "C.0.2", 10, _isAccessible:false);
            g.AddNode("C.2.2"); // Trappenhuis naast lift
            g.AddEdge("C.2.2", "L2", 4);
            g.AddEdge("C.2.2", "C.1.2", 10, _isAccessible: false);
            g.AddNode("C.3.2"); // Trappenhuis naast lift
            g.AddEdge("C.3.2", "L3", 4);
            g.AddEdge("C.3.2", "C.2.2", 10, _isAccessible: false);


            return g;
        }

        /// <summary>
        /// Calculate the distance using Pythagoras' theorem.
        /// </summary>
        /// <param name="_a"></param>
        /// <param name="_b"></param>
        /// <returns>Distance</returns>
        public static double CalculateDistance(double _a, double _b)
        {
            return Math.Sqrt(Math.Pow(_a, 2) + Math.Pow(_b, 2));
        }

        /// <summary>
        /// Short for CalculateDistance.
        /// </summary>
        /// <param name="_a"></param>
        /// <param name="_b"></param>
        /// <returns></returns>
        public static double CD(double _a, double _b)
        {
            return CalculateDistance(_a, _b);
        }
    }
}
