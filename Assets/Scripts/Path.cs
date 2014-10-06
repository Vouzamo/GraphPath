using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Path : ScriptableObject {
	
	private NodeGraph Graph { get; set; }
	private Arc Step { get; set; }
	public List<Path> ChildPaths { get; set; }

	public void Init(NodeGraph graph, Arc step) {
		Graph = graph;
		Step = step;
		ChildPaths = new List<Path>();
	}

	public void Render()
	{
		Debug.Log("From " + Step.From + " to " + Step.To + " at a cost of " + Step.Distance);

		if(ChildPaths.Any())
		{
			Debug.Log("Children Start");
			foreach(Path path in ChildPaths)
			{
				path.Render();
			}
			Debug.Log("Children End");
		}
	}

	public float Cost
	{
		get
		{
			if(ChildPaths.Any())
			{
				float minValue = float.MaxValue;

				foreach(Path path in ChildPaths)
				{
					minValue = Mathf.Min(minValue, path.Cost);
				}

				return (minValue + Step.Distance);
			}
			else
			{
				return Step.Distance;
			}
		}
	}

	public Stack<Arc> BestPath()
	{
		if(ChildPaths.Any())
		{
			Stack<Arc> bestPath = ChildPaths.OrderBy(x => x.Cost).First().BestPath();
			bestPath.Push(Step);
			return bestPath;
		}
		else
		{
			Stack<Arc> path = new Stack<Arc>();
			path.Push(Step);
			return path;
		}
	}
}

