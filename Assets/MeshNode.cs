using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
public class MeshNode : MonoBehaviour, IMalware {

	public List<MeshNode> TargetNodes = new List<MeshNode>();
	public List<LineRenderer> TargetEdges = new List<LineRenderer>();
	public short AttackPriority {get{return 1;}}

	// Use this for initialization
	void Start () {
		ActiveSubroutines.MalwareList.Add(this);		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision col)
	{
		LazerBeam beam = col.collider.GetComponent<LazerBeam>();
		if (beam != null){
			TakeDamage(1);
		}
	}

	public void TakeDamage(float amount)
	{
		RemoveEdges();
		this.transform.parent.GetComponent<MeshMalware>().RemoveNode(this);
		ActiveSubroutines.MalwareList.Remove(this);		
	}

	public void RemoveEdges(){
		for (int i = TargetEdges.Count-1; i > -1; i--) {
			RemoveEdge(TargetEdges[i]);
		}
	}

	private void RemoveEdge(LineRenderer liner){
		TargetEdges.Remove(liner);
		UnityEngine.Object.Destroy( liner.gameObject );
	}
	
	public void RemoveEdgesWithSibling(MeshNode sibling){
		int index = TargetNodes.IndexOf(sibling);
		if (index > -1) 
		{
			if (index < TargetEdges.Count)
				RemoveEdge(TargetEdges[index]);

			TargetNodes.Remove(sibling);
		}
	}


}
