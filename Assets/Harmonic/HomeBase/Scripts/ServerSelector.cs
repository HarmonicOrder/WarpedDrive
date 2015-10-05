using UnityEngine;
using System.Collections;

public class ServerSelector : MonoBehaviour {
	public TextMesh ServerName;
	public Color infectedColor;
	public Color cleanColor;
	public Color hoverColor;
	public SpriteRenderer InfectedSpriteRenderer;

	private Color initialColor;
	void Start()
	{
		initialColor = ServerName.color;
	}

	public void OnHoverOver()
	{
		ServerName.color = hoverColor;
	}

	public void OnHoverOff()
	{
		print ("hover off");
		ServerName.color = initialColor;
	}


}
