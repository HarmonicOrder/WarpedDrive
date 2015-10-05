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

		NetworkLocation netloc = NetworkMap.GetLocationByLocationName(name);
		if (netloc != null)
		{
			InfectedSpriteRenderer.enabled = netloc.IsInfected;

			if (netloc.IsInfected)
			{
				ServerName.color = infectedColor;
			}
			else
			{
				ServerName.color = cleanColor;
			}
		}
	}

	public void OnHoverOver()
	{
		ServerName.color = hoverColor;
	}

	public void OnHoverOff()
	{
		ServerName.color = initialColor;
	}


}
