using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class NodeGraph : MonoBehaviour {

	public GameObject Node;
	public GameObject Arc;

	private List<Node> Nodes { get; set; }
	private List<Arc> Arcs { get; set; }

	public Node node1 { get; set; } 
	public Node node2 { get; set; }
	public Node node3 { get; set; }
	public Node node4 { get; set; }
		
	public NodeGraph () {
		Nodes = new List<Node>();
		Arcs = new List<Arc>();
	}

	public void Start()
	{
		GenerateGraph();
	}

	public void Update() {
		if(Input.GetKeyUp(KeyCode.Space)) {

			List<Queue<Arc>> paths = CalculatePaths(node1, node4, false);

			foreach(var path in paths) {
				Debug.Log("Path of " + path.Count + " steps");
				foreach(var arc in path) {
					Debug.Log("Step from " + arc.From.transform.position + " to " + arc.To.transform.position + " at a cost of " + arc.Distance);
				}
			}
		}
	}

	public void OnDrawGizmos()
	{
		foreach(var arc in Arcs)
		{
			Gizmos.DrawLine(arc.From.transform.position, arc.To.transform.position);
		}
		foreach(var node in Nodes)
		{
			Gizmos.DrawSphere(node.transform.position, 0.25f);
		}
	}

	public void GenerateGraph() {
		ClearGraph();

		node1 = RegisterNode(new Vector3(0,0,0), 1.0f);
        node2 = RegisterNode(new Vector3(0,0,1), 1.0f);
        node3 = RegisterNode(new Vector3(0,1,1), 1.0f);
        node4 = RegisterNode(new Vector3(0,1,0), 1.0f);

		RegisterArc(node1, node2);
		RegisterArc(node2, node3);
		RegisterArc(node3, node4);
		RegisterArc(node4, node1);

	    RegisterArc(node2, node4);
	}

	public void ClearGraph() {

		// Destroy GameObjects

		Nodes.Clear();
		Arcs.Clear();
	}

    public Node RegisterNode(Vector3 position, float normal)
    {
		GameObject node = (GameObject)Instantiate(Node, position, Quaternion.identity);
		Node component = node.GetComponent<Node>();
		component.Init(this);

		node.transform.parent = this.transform;

		Nodes.Add(component);

		return component;
	}

	public Arc RegisterArc(Node from, Node to) {
		Vector3 position = (from.transform.position);
		Quaternion rotation = Quaternion.FromToRotation(Vector3.up, (from.transform.position - to.transform.position));

		GameObject arc = (GameObject)Instantiate(Arc, position, rotation);
		Arc component = arc.GetComponent<Arc>();
		component.Init(this, from, to);

		arc.transform.parent = this.transform;

		Arcs.Add(component);

		return component;
	}

	public Dictionary<Node, float> GetOutboundNeighbours(Node node) {
		List<Node> testedNodes = new List<Node>();
		List<Arc> testedArcs = new List<Arc>();
		return GetOutboundNeighbours(node, testedNodes, testedArcs);
	}

	public Dictionary<Node, float> GetOutboundNeighbours(Node node, List<Node> testedNodes, List<Arc> testedArcs) {
		Dictionary<Node, float> neighbours = new Dictionary<Node,float>();

		foreach(Arc arc in Arcs.Where(x => x.From == node && !testedNodes.Contains(x.To) && !testedArcs.Contains(x))) {
			if(!neighbours.ContainsKey(arc.To)) {
				neighbours.Add(arc.To, arc.Distance);
			}
		}

		return neighbours;
	}

	public List<Queue<Arc>> CalculatePaths(Node source, Node target, bool heuristic)
	{
		List<Queue<Arc>> paths = new List<Queue<Arc>>();

		List<Node> testedNodes = new List<Node>();
		List<Arc> testedArcs = new List<Arc>();

		Queue<Arc> path = new Queue<Arc>();
		
 		while(path != null) {
			path = TryGetPath(source, target, testedNodes, testedArcs);
			if(path != null) {
				paths.Add (path);
				if(heuristic) {
					break;
				} else {
					UpdateTestedArcs(path, testedArcs);
				}
			}
		}

		return paths;
	}

	private void UpdateTestedArcs(Queue<Arc> path, List<Arc> testedArcs) {
		Stack<Arc> arcs = new Stack<Arc>();
		foreach(Arc arc in path) {
			arcs.Push(arc);
		}
		testedArcs.Add(arcs.Pop());
	}

	public Queue<Arc> TryGetPath(Node source, Node target, List<Node> testedNodes, List<Arc> testedArcs) {
		Dictionary<Node, float> neighbours = GetOutboundNeighbours(source, testedNodes, testedArcs);

		if(neighbours.Any()) {
			foreach(KeyValuePair<Node, float> kvp in neighbours.OrderBy(x => x.Value))
			{
				Node node = kvp.Key;

				Queue<Arc> path = new Queue<Arc>();

			 	if(node == target)
				{
					Arc arc = Arcs.First(x => x.From == source && x.To == node);
					if(arc != null) {
						path.Enqueue(arc);
						return path;
					}
					return null;
				}
				else {
					Queue<Arc> subPath = TryGetPath(node, target, testedNodes, testedArcs);
					if(subPath != null) {
						path.Enqueue(Arcs.First(x => x.From == source && x.To == node));
						while(subPath.Any()) {
							path.Enqueue(subPath.Dequeue());
						}
						return path;
					}
					else {
						testedNodes.Add(node);
					}
				}
			}
			testedNodes.Add(source);
		}

		return null;
	}
}
