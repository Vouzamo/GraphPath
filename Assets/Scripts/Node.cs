using UnityEngine;
public class Node : MonoBehaviour {

	private NodeGraph Graph;
	public float Normal;
	public bool Enabled;

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

	public void OnDrawGizmos()
	{
		if(Enabled)
		{
			Gizmos.DrawSphere(transform.position, 0.25f);
		}
	}
}

