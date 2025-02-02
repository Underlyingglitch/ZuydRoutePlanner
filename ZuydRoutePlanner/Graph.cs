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

        /// <summary>
        /// Add a node to the graph.
        /// </summary>
        /// <param name="_node"></param>
        /// <returns>The added Node object</returns>
        public Node AddNode(Node _node)
        {
            Nodes.Add(_node);
            return _node;
        }

        /// <summary>
        /// Create a new node and add it to the graph.
        /// </summary>
        /// <param name="_name"></param>
        /// <param name="_isEmergencyOnly"></param>
        /// <param name="_hidden"></param>
        /// <param name="_isParking"></param>
        /// <param name="_requireAccessible"></param>
        /// <returns></returns>
        public Node AddNode(string _name, bool _hidden = false, bool _isParking = false, bool _requireAccessible = false)
        {
            return this.AddNode(new Node(_name, _hidden:_hidden, _isParking:_isParking, _requireAccessible:_requireAccessible));
        }

        /// <summary>
        /// Add an edge between two nodes.
        /// </summary>
        /// <param name="_from"></param>
        /// <param name="_to"></param>
        /// <param name="_distance"></param>
        /// <param name="_isAccessible"></param>
        /// <param name="_isEmergencyBlocked"></param>
        /// <param name="_isAccessibleByCar"></param>
        public void AddEdge(Node _from, Node _to, double _distance, bool _isAccessible = true, bool _isEmergencyBlocked = false, bool _isEmergencyOnly = false, bool _isAccessibleByCar = false)
        {
            _from.Edges.Add(new Edge(_to, _distance, _isAccessible: _isAccessible, _isEmergencyBlocked: _isEmergencyBlocked, _isAccessibleByCar: _isAccessibleByCar, _isEmergencyOnly: _isEmergencyOnly));
            _to.Edges.Add(new Edge(_from, _distance, _isAccessible: _isAccessible, _isEmergencyBlocked: _isEmergencyBlocked, _isAccessibleByCar: _isAccessibleByCar, _isEmergencyOnly: _isEmergencyOnly)); // Voor ongerichte graaf
        }

        /// <summary>
        /// Add an edge between two nodes by their names.
        /// </summary>
        /// <param name="_from"></param>
        /// <param name="_to"></param>
        /// <param name="_distance"></param>
        /// <param name="_isAccessible"></param>
        /// <param name="_isEmergencyBlocked"></param>
        /// <param name="_isAccessibleByCar"></param>
        public void AddEdge(string _from, string _to, double _distance, bool _isAccessible = true, bool _isEmergencyBlocked = false, bool _isEmergencyOnly = false, bool _isAccessibleByCar = false)
        {
            Node from = Nodes.Find(n => n.Name == _from);
            Node to = Nodes.Find(n => n.Name == _to);
            this.AddEdge(from, to, _distance, _isAccessible: _isAccessible, _isEmergencyBlocked: _isEmergencyBlocked, _isAccessibleByCar: _isAccessibleByCar, _isEmergencyOnly: _isEmergencyOnly);
        }

        /// <summary>
        /// Find a node by its name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>The Node object</returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public Node FindNode(string name)
        {
            var node = Nodes.Find(n => n.Name == name);
            if (node == null)
            {
                throw new KeyNotFoundException($"Node with name '{name}' not found.");
            }
            return node;
        }
    }
}
