using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class NodeGraph : MonoBehaviour {

	public GameObject Node;
	public GameObject Arc;
	public bool ShowGizmos;
	public Node From;
	public Node To;

	private List<Node> Nodes;
	private List<Arc> Arcs;
		
	public NodeGraph () {
		Nodes = new List<Node>();
		Arcs = new List<Arc>();
	}

	public void Start()
	{
		//GenerateGraph();
		DetermineGraph();
	}

	public void Update() {
		if(Input.GetKeyUp(KeyCode.Space)) {
			if(From != null && To != null)
			{
				List<Path> paths = FindPaths(From, To);
				foreach(Path path in paths)
				{
					path.Render();
				}
		
				if(paths.Any())
				{
					Stack<Arc> bestPath = paths.OrderBy(x => x.Cost).First().BestPath();

					Debug.Log ("BEST PATH");

					while(bestPath.Any())
					{
						Arc arc = bestPath.Pop();

						Debug.Log(arc.From + " to " + arc.To + " at a cost of " + arc.Distance);
					}
				}
			}
			else
			{
				Debug.Log("Assign a from and to node!");
			}
		}
	}

	public void DetermineGraph()
	{
		ClearGraph();

		foreach(Node node in FindObjectsOfType(typeof(Node)))
		{
			node.Init(this);
			Nodes.Add(node);
		}

		foreach(Arc arc in FindObjectsOfType(typeof(Arc)))
		{
			arc.Init(this, arc.From, arc.To);
			Arcs.Add(arc);
		}
	}

	public void GenerateGraph()
	{
		ClearGraph();

//		node1 = RegisterNode(new Vector3(0,0,0), 1.0f);
//      node2 = RegisterNode(new Vector3(0,0,1), 1.0f);
//      node3 = RegisterNode(new Vector3(0,1,1), 1.0f);
//      node4 = RegisterNode(new Vector3(0,1,0), 1.0f);
//
//		RegisterArc(node1, node2);
//		RegisterArc(node2, node3);
//		RegisterArc(node3, node4);
//		RegisterArc(node4, node1);
//
//	    RegisterArc(node2, node4);
	}

	public void ClearGraph() {

		// Destroy GameObjects

		Nodes.Clear();
		Arcs.Clear();
	}

    public Node RegisterNode(Vector3 position, float normal)
    {
		GameObject node = (GameObject)Instantiate(Node, position, Quaternion.identity);
		node.name = "Node " + position;
		node.transform.parent = this.transform;

		Node component = node.GetComponent<Node>();
		component.Init(this);
		
		Nodes.Add(component);

		return component;
	}

	public Arc RegisterArc(Node from, Node to) {
		Vector3 position = (from.transform.position);
		Quaternion rotation = Quaternion.FromToRotation(Vector3.up, (from.transform.position - to.transform.position));

		GameObject arc = (GameObject)Instantiate(Arc, position, rotation);
		arc.name = "Arc " + from.name + " -> " + to.name;
		arc.transform.parent = this.transform;

		Arc component = arc.GetComponent<Arc>();
		component.Init(this, from, to);
		
		Arcs.Add(component);

		return component;
	}

	public List<Path> FindPaths(Node fromNode, Node toNode)
	{
		return TryGetPath(fromNode, toNode, new List<Arc>());
	}

	public List<Path> TryGetPath(Node fromNode, Node toNode, List<Arc> testedArcs)
	{
		List<Path> paths = new List<Path>();

		// Find all the outbound arcs for the fromNode
		foreach(Arc arc in Arcs.Where(x => x.From == fromNode && !testedArcs.Contains(x)))
		{
			testedArcs.Add(arc);

			Path path = ScriptableObject.CreateInstance<Path>();
			path.Init(this, arc);

			if(arc.To == toNode)
			{
				paths.Add(path);
			}
			else
			{
				path.ChildPaths = TryGetPath(arc.To, toNode, testedArcs);

				if(path.ChildPaths.Any())
				{
					paths.Add(path);
				}
			}
		}

		return paths;
	}
}
