using System.Collections.Generic;

namespace ZuydRoutePlanner
{
    public class Node
    {
        public string Name { get; set; }
        // Are nodes hidden in the output shown to the user? (These nodes are used for routing, but are not shown to the user)
        public bool Hidden { get; set; }
        // Is this node a parking lot? (When starting in a car, the user will need to cross a parking lot to get to the building)
        public bool IsParking { get; set; }
        // Does this node require accessibility? (For example, elevator usage is only allowed for disabled users or organisers)
        public bool RequireAccessible { get; set; }
        // List of edges (containing the destination node and the distance to it, along with other parameters)
        public List<Edge> Edges { get; set; }

        public Node(string _name, bool _hidden = false, bool _isParking = false, bool _requireAccessible = false)
        {
            Name = _name;
            
            Hidden = _hidden;
            IsParking = _isParking;
            RequireAccessible = _requireAccessible;
            Edges = new List<Edge>();
        }
    }

    public class Edge
    {
        public Node Destination { get; set; }
        public double Distance { get; set; }
        // Is this edge accessible for wheelchair users? (For example, stairs are not accessible)
        public bool IsAccessible { get; set; }
        // Is this edge accessible by car?
        public bool IsAccessibleByCar { get; set; }
        // Is this edge blocked in emergency mode? (For example, elevators are blocked in emergency mode)
        public bool IsEmergencyBlocked { get; set; }
        // Is this edge only accessible in emergency mode? (For example, emergency exits)
        public bool IsEmegencyOnly { get; set; }

        public Edge(Node _destination, double _distance, bool _isAccessible = true, bool _isEmergencyBlocked = false, bool _isEmergencyOnly = false, bool _isAccessibleByCar = false)
        {
            Destination = _destination;
            Distance = _distance;
            IsAccessible = _isAccessible;
            IsEmergencyBlocked = _isEmergencyBlocked;
            IsEmegencyOnly = _isEmergencyOnly;
            IsAccessibleByCar = _isAccessibleByCar;
        }
    }
}
