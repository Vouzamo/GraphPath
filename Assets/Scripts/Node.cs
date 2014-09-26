using UnityEngine;
public class Node : ScriptableObject {

	private NodeGraph Graph { get; set; }
	public Vector3 Position { get; set; }
	public float Normal { get; set; }

	public void Init(NodeGraph graph, Vector3 position, float normal) {
		Graph = graph;
		Position = position;
		Normal = normal;
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

