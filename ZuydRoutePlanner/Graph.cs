using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZuydRoutePlanner
{
    public class Graph
    {
        public List<Node> Nodes { get; set; }

        public Graph()
        {
            Nodes = new List<Node>();
        }

        public Node AddNode(Node _node)
        {
            Nodes.Add(_node);
            return _node;
        }

        public Node AddNode(string _name, bool _isDoor = false, bool _isEmergencyOnly = false, bool _hidden = false, bool _isParking = false, bool _requireAccessible = false)
        {
            return this.AddNode(new Node(_name, _isDoor, _isEmergencyOnly:_isEmergencyOnly, _hidden:_hidden, _isParking:_isParking, _requireAccessible:_requireAccessible));
        }

        public void AddEdge(Node _from, Node _to, double _distance, bool _isAccessible = true, bool _isEmergencyBlocked = false, bool _isAccessibleByCar = false)
        {
            _from.Edges.Add(new Edge(_to, _distance, _isAccessible, _isEmergencyBlocked, _isAccessibleByCar));
            _to.Edges.Add(new Edge(_from, _distance, _isAccessible, _isEmergencyBlocked, _isAccessibleByCar)); // Voor ongerichte graaf
        }

        public void AddEdge(string _from, Node _to, double _distance, bool _isAccessible = true, bool _isEmergencyBlocked = false, bool _isAccessibleByCar = false)
        {
            Node from = Nodes.Find(n => n.Name == _from);
            this.AddEdge(from, _to, _distance, _isAccessible, _isEmergencyBlocked, _isAccessibleByCar);
        }

        public void AddEdge(Node _from, string _to, double _distance, bool _isAccessible = true, bool _isEmergencyBlocked = false, bool _isAccessibleByCar = false)
        {
            Node to = Nodes.Find(n => n.Name == _to);
            this.AddEdge(_from, to, _distance, _isAccessible, _isEmergencyBlocked, _isAccessibleByCar);
        }

        public void AddEdge(string _from, string _to, double _distance, bool _isAccessible = true, bool _isEmergencyBlocked = false, bool _isAccessibleByCar = false)
        {
            Node from = Nodes.Find(n => n.Name == _from);
            Node to = Nodes.Find(n => n.Name == _to);
            this.AddEdge(from, to, _distance, _isAccessible, _isEmergencyBlocked, _isAccessibleByCar);
        }

        public Node FindNode(string name)
        {
            return Nodes.Find(n => n.Name == name);
        }
    }
}
