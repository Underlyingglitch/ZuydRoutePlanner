using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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

            g.AddNode("C.0.1");
            g.AddEdge("1", "C.0.1", 4);

            g.AddNode("B.0.5");
            g.AddEdge("1", "B.0.5", 7);

            g.AddNode("C.0.103");
            g.AddEdge("1", "C.0.103", CD(10.5, 6));
            g.AddNode("2");
            g.AddEdge("C.0.103", "2", CD(5, 4));
            g.AddEdge("1", "2", CD(14, 1));

            g.AddNode("3");
            g.AddEdge("2", "3", 5);
            g.AddEdge("2", "Centrale Hal", CD(2, 12));
            g.AddEdge("3", "Centrale Hal", CD(4, 13));

            g.AddNode("C.0.100");
            g.AddEdge("C.0.100", "3", 5);
            g.AddNode("C.0.102");
            g.AddEdge("C.0.102", "3", 7);

            g.AddNode("C.0.110");
            g.AddEdge("C.0.110", "3", CD(4, 10));

            g.AddNode("Nieuw Eyckholt");
            g.AddNode("Bus");
            g.AddNode("Brug");
            g.AddNode("Pad");
            g.AddNode("P1", _isParking: true);
            g.AddNode("PG", _isParking:true, _requireAccessible:true);

            g.AddEdge("Bus", "Brug", 20);
            g.AddEdge("Brug", "Hoofdingang", 30, _isAccessible:false);
            g.AddEdge("Brug", "Pad", 10);
            g.AddEdge("Pad", "P1", 15);
            g.AddEdge("Nieuw Eyckholt", "P1", 40, _isAccessibleByCar:true);
            g.AddEdge("Nieuw Eyckholt", "PG", 40, _isAccessibleByCar: true);
            g.AddEdge("Nieuw Eyckholt", "Pad", 10);
            g.AddEdge("PG", "Centrale Hal", 10);
            g.AddEdge("P1", "Hoofdingang", 20, _isAccessible:false);
            g.AddEdge("P1", "PG", 10, _isAccessibleByCar: true);
            g.AddEdge("Bus", "Nieuw Eyckholt", 50, _isAccessibleByCar: true);

            return g;
        }

        public static double CalculateDistance(double _a, double _b)
        {
            return Math.Sqrt(Math.Pow(_a, 2) + Math.Pow(_b, 2));
        }

        public static double CD(double _a, double _b)
        {
            return CalculateDistance(_a, _b);
        }
    }
}
