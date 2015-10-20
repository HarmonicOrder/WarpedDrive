using UnityEngine;
using System.Collections;

public class MachineLabel : MonoBehaviour {

	private Transform Root;
	private Machine myMachine;
	private TextMesh myText;

	// Use this for initialization
	void Start () {
		this.Root = this.transform.root;
		this.myMachine = CyberspaceBattlefield.Current.FindByName(this.Root.name);
		this.myText = this.GetComponent<TextMesh>();
		UpdateText();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void UpdateText()
	{
		myText.text = string.Format("{0}\r\n{1} CPU Cores", this.myMachine.HumanSubnetAddress, this.myMachine.CPUCores);
	}
}
