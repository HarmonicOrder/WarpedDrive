using UnityEngine;
using System.Collections;

public class MachineLabel : MonoBehaviour {

	public Color CleanColor;
	public Color InfectedColor;
	public SpriteRenderer FenceRenderer;

	private Transform Root;
	private Machine myMachine;
	private TextMesh myText;

	// Use this for initialization
	void Start () {
		this.Root = this.transform.root;
		this.myMachine = CyberspaceBattlefield.Current.FindByName(this.Root.name);
		this.myMachine.OnSystemClean += OnSystemClean;
		this.myText = this.GetComponent<TextMesh>();
		UpdateText();
		UpdateFence();
	}

	private void OnSystemClean() {

		UpdateText();
		UpdateFence();
	}

	private void UpdateText()
	{
		if (this.myMachine.IsInfected)
		{
			myText.color = InfectedColor;
		}
		else
		{
			myText.color = CleanColor;
		}
		myText.text = string.Format("{0}\r\n{1} CPU Cores", this.myMachine.HumanSubnetAddress, this.myMachine.CPUCores);
	}

	private void UpdateFence()
	{
		if (this.myMachine.IsInfected)
		{
			FenceRenderer.color = HarmonicUtils.ColorWithAlpha(InfectedColor, .5f);
		}
		else
		{
			FenceRenderer.color = HarmonicUtils.ColorWithAlpha(CleanColor, .5f);
		}
	}
}
