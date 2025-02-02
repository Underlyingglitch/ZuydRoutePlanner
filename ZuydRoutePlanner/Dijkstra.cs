using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ZuydRoutePlanner
{
    public class Dijkstra
    {
		public static (List<Node> path, double totalDistance) FindShortestPath(Graph graph, Node start, Node end, bool requireAccessible, bool emergencyMode, bool startByCar)
		{
			var distances = new Dictionary<Node, double>();
			var previous = new Dictionary<Node, Node>();
			var priorityQueue = new SortedSet<(Node node, double distance)>(Comparer<(Node node, double distance)>.Create((x, y) =>
				x.distance == y.distance ? x.node.Name.CompareTo(y.node.Name) : x.distance.CompareTo(y.distance)));

			foreach (var node in graph.Nodes)
			{
				distances[node] = double.PositiveInfinity;
				previous[node] = null;
			}

			distances[start] = 0;
			priorityQueue.Add((start, 0));

			bool isCarMode = startByCar;

			while (priorityQueue.Count > 0)
			{
				var (currentNode, currentDistance) = priorityQueue.Min;
				priorityQueue.Remove(priorityQueue.Min);

				if (currentNode == end)
					break;

				foreach (var edge in currentNode.Edges)
				{
					if (!IsPathAllowed(edge, currentNode, emergencyMode, isCarMode, requireAccessible))
                        continue;

                    double newDist = currentDistance + edge.Distance;
					if (newDist < distances[edge.Destination])
					{
						priorityQueue.Remove((edge.Destination, distances[edge.Destination]));
						distances[edge.Destination] = newDist;
						previous[edge.Destination] = currentNode;
						priorityQueue.Add((edge.Destination, newDist));
					}
				}

				if (isCarMode && currentNode.IsParking && (!requireAccessible || currentNode.RequireAccessible))
				{
					isCarMode = false;
				}
			}

			var path = new List<Node>();
			for (var at = end; at != null; at = previous[at])
			{
				path.Insert(0, at);
			}

			return (path, distances[end]);
		}


		public static (List<Node> path, double totalDistance) FindShortestPathRec(Graph graph, Node start, Node end, bool requireAccessible, bool emergencyMode, bool startByCar)
        {
            var distances = new Dictionary<Node, double>();
            var previous = new Dictionary<Node, Node>();

            foreach (var node in graph.Nodes)
            {
                distances[node] = double.PositiveInfinity;
                previous[node] = null;
            }

            distances[start] = 0;

            FindShortestPathRecursive(graph, start, end, requireAccessible, emergencyMode, startByCar, distances, previous);

            // Reconstruct the path
            var path = new List<Node>();
            for (var at = end; at != null; at = previous[at])
            {
                path.Insert(0, at);
            }

            return (path, distances[end]);
        }

        private static void FindShortestPathRecursive(Graph graph, Node currentNode, Node end, bool requireAccessible, bool emergencyMode, bool isCarMode, Dictionary<Node, double> distances, Dictionary<Node, Node> previous)
        {
            if (currentNode == end)
                return;

            foreach (var edge in currentNode.Edges)
            {
                if (!IsPathAllowed(edge, currentNode, emergencyMode, isCarMode, requireAccessible))
                    continue;

                double newDist = distances[currentNode] + edge.Distance;
                if (newDist < distances[edge.Destination])
                {
                    distances[edge.Destination] = newDist;
                    previous[edge.Destination] = currentNode;

                    // Switch to foot mode if a parking node is reached and it is accessible if required
                    bool nextIsCarMode = isCarMode;
                    if (isCarMode && currentNode.IsParking && (!requireAccessible || currentNode.RequireAccessible))
                    {
                        nextIsCarMode = false;
                    }

                    FindShortestPathRecursive(graph, edge.Destination, end, requireAccessible, emergencyMode, nextIsCarMode, distances, previous);
                }
            }
        }

        public static (List<Node> path, double totalDistance) FindShortestPathViaPoints(Graph graph, Node start, Node end, List<Node> viaPoints, bool requireAccessible, bool emergencyMode, bool startByCar)
        {
            var fullPath = new List<Node>();
            double totalDistance = 0.0;

            Node currentStart = start;
            bool isCarMode = startByCar;

            foreach (var point in viaPoints)
            {
                var (pathSegment, segmentDistance) = FindShortestPathRec(graph, currentStart, point, requireAccessible, emergencyMode, isCarMode);
                if (fullPath.Count > 0)
                {
                    // Remove the last node to avoid duplication
                    fullPath.RemoveAt(fullPath.Count - 1);
                }
                fullPath.AddRange(pathSegment);
                totalDistance += segmentDistance;
                currentStart = point;

                // Update isCarMode if the point is a parking node
                if (isCarMode && point.IsParking && (!requireAccessible || point.RequireAccessible))
                {
                    isCarMode = false;
                }
            }

            var (finalPathSegment, finalSegmentDistance) = FindShortestPathRec(graph, currentStart, end, requireAccessible, emergencyMode, isCarMode);
            if (fullPath.Count > 0)
            {
                // Remove the last node to avoid duplication
                fullPath.RemoveAt(fullPath.Count - 1);
            }
            fullPath.AddRange(finalPathSegment);
            totalDistance += finalSegmentDistance;

            return (fullPath, totalDistance);
        }

        private static bool IsPathAllowed(Edge _edge, Node _currentNode, bool _emergencyMode, bool _isCarMode, bool _requireAccessible)
        {
            // Skip edges that are blocked during emergency
            if ((_emergencyMode && _edge.IsEmergencyBlocked) ||
                        // Skip doors that are emergency only when not in emergency mode
                        (!_emergencyMode && _edge.Destination.IsEmegencyOnly) ||
                        // Skip edges that are not accessible when accessibility is required
                        (_requireAccessible && !_edge.IsAccessible) ||
                        // Skip edges that need accessibility required (such as the elevator)
                        (!_requireAccessible && _edge.Destination.RequireAccessible) ||
                        // Skip edges that are not accessible by car when in car mode
                        (_isCarMode && !_edge.IsAccessibleByCar && !_currentNode.IsParking))
                return false;
            return true;
        }
    }
}
