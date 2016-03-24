using UnityEngine;
using System.Collections;

public class MachineLabel : MonoBehaviour {

	public Color CleanColor, InfectedColor, ReinfectingColor;
	public SpriteRenderer FenceRenderer;
    public SpriteRenderer FenceRenderer2;

    private Transform Root;
	public Machine myMachine;
	private TextMesh myText;

	// Use this for initialization
	void Start () {
		this.Root = this.transform.root;
		this.myMachine = CyberspaceBattlefield.Current.FindByName(this.Root.name);
		this.myMachine.OnMachineClean += OnMachineStatusChange;
        this.myMachine.OnMachineReInfectionStart += OnMachineStatusChange;
        this.myMachine.OnMachineReInfectionFailure += OnMachineStatusChange;
        this.myMachine.OnMachineReInfectionSuccess += OnMachineStatusChange;
		this.myText = this.GetComponent<TextMesh>();
		UpdateText();
		UpdateFence();
	}

	private void OnMachineStatusChange() {
		UpdateText();
		UpdateFence();
	}

	private void UpdateText()
	{
        myText.color = GetAppropriateColor();
		myText.text = string.Format("{0}\r\n{1} CPU Cores", this.myMachine.HumanSubnetAddress, this.myMachine.CPUCores);
	}

	private void UpdateFence()
	{
		FenceRenderer.color = GetAppropriateColor(.5f);
        if (FenceRenderer2 != null)
            FenceRenderer2.color = GetAppropriateColor(.5f);
	}

    private Color GetAppropriateColor(float alpha = 1)
    {
        if (this.myMachine.IsInfected)
        {
            return HarmonicUtils.ColorWithAlpha(InfectedColor, alpha);
        }
        else if (this.myMachine.IsBeingReinfected)
        {
            return HarmonicUtils.ColorWithAlpha(ReinfectingColor, alpha);
        }
        else
        {
            return HarmonicUtils.ColorWithAlpha(CleanColor, alpha);
        }
    }

    void OnDestroy()
    {
        this.myMachine.OnMachineClean -= OnMachineStatusChange;
        this.myMachine.OnMachineReInfectionSuccess -= OnMachineStatusChange;
    }
}
