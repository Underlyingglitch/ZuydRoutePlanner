using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZuydRoutePlanner
{
    public class Node
    {
        public string Name { get; set; }
        public bool IsDoor { get; set; }
        public bool IsEmegencyOnly { get; set; }
        public bool Hidden { get; set; }
        public bool IsParking { get; set; }
        public bool RequireAccessible { get; set; }
        public List<Edge> Edges { get; set; }

        public Node(string _name, bool _isDoor = false, bool _isEmergencyOnly = false, bool _hidden = false, bool _isParking = false, bool _requireAccessible = false)
        {
            Name = _name;
            IsDoor = _isDoor;
            IsEmegencyOnly = _isEmergencyOnly;
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
        public bool IsAccessible { get; set; }
        public bool IsAccessibleByCar { get; set; }
        public bool IsEmergencyBlocked { get; set; }

        public Edge(Node _destination, double _distance, bool _isAccessible = true, bool _isEmergencyBlocked = false, bool _isAccessibleByCar = false)
        {
            Destination = _destination;
            Distance = _distance;
            IsAccessible = _isAccessible;
            IsEmergencyBlocked = _isEmergencyBlocked;
            IsAccessibleByCar = _isAccessibleByCar;
        }
    }
}
