using UnityEngine;

public class Arc : ScriptableObject {
	
	private NodeGraph Graph { get; set; }
	public Node From { get; set; }
	public Node To { get; set; }

	public float Distance { get { return Vector3.Distance(From.Position,To.Position); } }

	public void Init(NodeGraph graph, Node from, Node to) {
		Graph = graph;
		From = from;
		To = to;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

