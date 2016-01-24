using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class SubnetSceneSelector : MonoBehaviour {

	public MeshRenderer SubnetLabelRenderer;
	public Transform FiberGateway;

	private Color BasicColor;
	private TextMesh subnetTextMesh;
    private Collider myCollider;

	// Use this for initialization
	void Start () {
        myCollider = this.GetComponent<Collider>();
		SubnetLabelRenderer.enabled = true;	
		subnetTextMesh = SubnetLabelRenderer.GetComponent<TextMesh>();
		BasicColor = subnetTextMesh.color;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnHoverOn(){
		//todo: change color, not set renderer enabled
		SubnetLabelRenderer.enabled = true;
		subnetTextMesh.color = HomeBase.SubnetTextHighlightColor;
	}

	public void OnHoverOff(){
		SubnetLabelRenderer.enabled = true;
		subnetTextMesh.color = BasicColor;
	}

    public void OnSelect()
    {
        myCollider.enabled = false;
        OnHoverOn();
    }

    public void OnDeselect()
    {
        myCollider.enabled = true;
        OnHoverOff();
    }
}
