using UnityEngine;
public class Node : MonoBehaviour {

	private NodeGraph Graph { get; set; }
	public float Normal { get; set; }
	public bool Enabled { get; set; }

	public void Init(NodeGraph graph)
	{
		Graph = graph;
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

