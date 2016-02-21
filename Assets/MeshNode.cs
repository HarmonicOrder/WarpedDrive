using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(Collider))]
public class MeshNode : MonoBehaviour, IMalware {

	public List<MeshNode> TargetNodes = new List<MeshNode>();
	public List<LineRenderer> TargetEdges = new List<LineRenderer>();
	public short AttackPriority {get{return 1;}}
    public bool IsHead = false;
    
	public virtual VirusAI.VirusType Type {get{return VirusAI.VirusType.Ransomware;}}

    public float KillChance { get { return 0; } }

    public float SaveChance { get { return 0; } }

    public float Reboots { get  { return 0;  } }

    public Transform MiniExplosionPrefab;

	// Use this for initialization
	void Start () {
		ActiveSubroutines.AddVirus(this);		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision col)
	{
		LazerBeam beam = col.collider.GetComponent<LazerBeam>();
		if (beam != null){
			Kill(null);
		}
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

	private void SelfDestruct()
	{
		Instantiate(MiniExplosionPrefab, this.transform.position, Quaternion.identity);
		GameObject.Destroy(this.gameObject);
	}
	
	
	void OnDestroy()
    {
        //print("removing meshnode from virus list");
        ActiveSubroutines.RemoveVirus(this);
	}

    public void DoAttack(ICombatant target) { }

    public bool TrySave(ICombatant attacker)
    {
        return false;
    }

    public void Kill(ICombatant attacker)
    {
        RemoveEdges();
        this.transform.parent.GetComponent<MeshMalware>().RemoveNode(this);
        ActiveSubroutines.RemoveVirus(this);
        SelfDestruct();
    }

    public void DoOnSave(ICombatant attacker)
    {
        
    }

    public void DoOnBlock(ICombatant attacker)
    {
        //noop
    }
}
