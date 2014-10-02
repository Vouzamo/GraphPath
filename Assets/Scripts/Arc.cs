using UnityEngine;

public class Arc : MonoBehaviour {
	
	private NodeGraph Graph { get; set; }
	public Node From;
	public Node To;

	public float Distance { get { return Vector3.Distance(From.transform.position, To.transform.position); } }

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

	public void OnDrawGizmos()
	{
		if(From != null && From.Enabled && To != null && To.Enabled)
		{
			Gizmos.DrawLine(From.transform.position, To.transform.position);
		}
	}
}

