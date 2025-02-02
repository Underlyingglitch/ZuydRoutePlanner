using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ZuydRoutePlanner
{
    public class Dijkstra
    {
		public static (List<Node> path, double totalDistance) FindShortestPath(Graph _graph, Node _start, Node _end, bool _requireAccessible, bool _emergencyMode, bool _startByCar, bool _isOrganiser, List<Node> _exclude = null)
		{
			var distances = new Dictionary<Node, double>();
			var previous = new Dictionary<Node, Node>();
			var priorityQueue = new SortedSet<(Node node, double distance)>(Comparer<(Node node, double distance)>.Create((x, y) =>
				x.distance == y.distance ? x.node.Name.CompareTo(y.node.Name) : x.distance.CompareTo(y.distance)));

			foreach (var node in _graph.Nodes)
			{
				distances[node] = double.PositiveInfinity;
				previous[node] = null;
			}

			distances[_start] = 0;
			priorityQueue.Add((_start, 0));

			bool isCarMode = _startByCar;

			while (priorityQueue.Count > 0)
			{
				var (currentNode, currentDistance) = priorityQueue.Min;
				priorityQueue.Remove(priorityQueue.Min);

				if (currentNode == _end)
					break;

				foreach (var edge in currentNode.Edges)
				{
					if (!IsPathAllowed(edge, currentNode, _emergencyMode, isCarMode, _requireAccessible, _isOrganiser, _exclude))
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

				if (isCarMode && currentNode.IsParking && (!_requireAccessible || currentNode.RequireAccessible))
				{
					isCarMode = false;
				}
			}

			var path = new List<Node>();
			for (var at = _end; at != null; at = previous[at])
			{
				path.Insert(0, at);
			}

            if (distances[_end] == double.PositiveInfinity)
            {
                path = null;
            }

            return (path, distances[_end]);
		}


		public static (List<Node> path, double totalDistance) FindShortestPathRecursive(Graph _graph, Node _start, Node _end, bool _requireAccessible, bool _emergencyMode, bool _startByCar, bool _isOrganiser, List<Node> _exclude = null)
        {
            var distances = new Dictionary<Node, double>();
            var previous = new Dictionary<Node, Node>();

            foreach (var node in _graph.Nodes)
            {
                distances[node] = double.PositiveInfinity;
                previous[node] = null;
            }

            distances[_start] = 0;

            FindShortestPathRec(_graph, _start, _end, _requireAccessible, _emergencyMode, _startByCar, _isOrganiser, _exclude, distances, previous);

            // Reconstruct the path
            var path = new List<Node>();
            for (var at = _end; at != null; at = previous[at])
            {
                path.Insert(0, at);
            }

            if (distances[_end] == double.PositiveInfinity)
            {
                path.Clear();
            }

            return (path, distances[_end]);
        }

        private static void FindShortestPathRec(Graph _graph, Node _currentNode, Node _end, bool _requireAccessible, bool _emergencyMode, bool _isCarMode, bool _isOrganiser, List<Node> _exclude, Dictionary<Node, double> _distances, Dictionary<Node, Node> _previous)
        {
            if (_currentNode == _end)
                return;

            foreach (var edge in _currentNode.Edges)
            {
                if (!IsPathAllowed(edge, _currentNode, _emergencyMode, _isCarMode, _requireAccessible, _isOrganiser, _exclude))
                    continue;

                double newDist = _distances[_currentNode] + edge.Distance;
                if (newDist < _distances[edge.Destination])
                {
                    _distances[edge.Destination] = newDist;
                    _previous[edge.Destination] = _currentNode;

                    // Switch to foot mode if a parking node is reached and it is accessible if required
                    bool nextIsCarMode = _isCarMode;
                    if (_isCarMode && _currentNode.IsParking && (!_requireAccessible || _currentNode.RequireAccessible))
                    {
                        nextIsCarMode = false;
                    }

                    FindShortestPathRec(_graph, edge.Destination, _end, _requireAccessible, _emergencyMode, nextIsCarMode, _isOrganiser, _exclude, _distances, _previous);
                }
            }
        }

        public static (List<Node> path, double totalDistance) FindShortestPathViaPoints(Graph _graph, Node _start, Node _end, List<Node> _viaPoints, bool _requireAccessible, bool _emergencyMode, bool _startByCar, bool _isOrganiser, List<Node> _exclude = null)
        {
            var fullPath = new List<Node>();
            double totalDistance = 0.0;

            Node currentStart = _start;
            bool isCarMode = _startByCar;

            foreach (var point in _viaPoints)
            {
                var (pathSegment, segmentDistance) = FindShortestPathRecursive(_graph, currentStart, point, _requireAccessible, _emergencyMode, isCarMode, _isOrganiser, _exclude);
                if (fullPath.Count > 0)
                {
                    // Remove the last node to avoid duplication
                    fullPath.RemoveAt(fullPath.Count - 1);
                }
                fullPath.AddRange(pathSegment);
                totalDistance += segmentDistance;
                currentStart = point;

                // Update isCarMode if the point is a parking node
                if (isCarMode && point.IsParking && (!_requireAccessible || point.RequireAccessible))
                {
                    isCarMode = false;
                }
            }

            var (finalPathSegment, finalSegmentDistance) = FindShortestPathRecursive(_graph, currentStart, _end, _requireAccessible, _emergencyMode, isCarMode, _isOrganiser);
            if (finalPathSegment.Count == 0)
            {
                return (new List<Node>(), double.PositiveInfinity);
            }
            if (fullPath.Count > 0)
            {
                // Remove the last node to avoid duplication
                fullPath.RemoveAt(fullPath.Count - 1);
            }
            fullPath.AddRange(finalPathSegment);
            totalDistance += finalSegmentDistance;

            return (fullPath, totalDistance);
        }

        private static bool IsPathAllowed(Edge _edge, Node _currentNode, bool _emergencyMode, bool _isCarMode, bool _requireAccessible, bool _isOrganiser, List<Node> _exclude = null)
        {
            // Block edges that are blocked during emergency
            if ((_emergencyMode && _edge.IsEmergencyBlocked) ||
                // Block edges that are emergency only when not in emergency mode
                (!_emergencyMode && _edge.IsEmegencyOnly) ||
                // Block edges that are not accessible when accessibility is required
                (_requireAccessible && !_edge.IsAccessible) ||
                // Block edges that need accessibility required (such as the elevator) when user does not require it and is not the organiser
                (_edge.Destination.RequireAccessible && (!_requireAccessible && !_isOrganiser)) ||
                // Block edges that are not accessible by car when in car mode
                (_isCarMode && !_edge.IsAccessibleByCar && !_currentNode.IsParking) ||
                // Block nodes that are excluded
                (_exclude != null && _exclude.Count > 0 && _exclude.Contains(_edge.Destination))
                )
                return false;
            return true;
        }
    }
}
