using UnityEngine;
using System.Collections;

public class Generator : MonoBehaviour {

    public TextMesh OxygenReadout;
    public Light StatusLight;

    private TextMesh myText;
	// Use this for initialization
	void Start () {
        this.myText = this.GetComponent<TextMesh>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void PrintStatus(string generatorName)
    {
        float amount = StarshipEnvironment.Instance.GeneratorAmount(generatorName);

        if (amount > 0)
        {
            OxygenReadout.text = @"\r\n\r\n\r\n\r\nOutput: " + amount;
            StatusLight.color = Color.green;
            myText.text = "ON";
        }
        else
        {
            OxygenReadout.text = "";
            StatusLight.color = Color.red;
            myText.text = "OFF";
        }
    }
}
