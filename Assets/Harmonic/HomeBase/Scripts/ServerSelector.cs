using UnityEngine;
using System.Collections;

public class ServerSelector : MonoBehaviour {
	public TextMesh ServerName;
	public Color infectedColor;
	public Color cleanColor;
	public Color hoverColor;
	public SpriteRenderer InfectedSpriteRenderer;

	private Color initialColor;
	private NetworkLocation netloc;
	void Start()
	{
		initialColor = ServerName.color;

		netloc = NetworkMap.GetLocationByLocationName(name);
		if (netloc != null)
		{
			InfectedSpriteRenderer.enabled = netloc.IsInfected;

			SetUnHoveredColor();
		}
	}

	private void SetUnHoveredColor()
	{
		if ((netloc != null) && netloc.IsInfected)
		{
			ServerName.color = infectedColor;
		}
		else
		{
			ServerName.color = cleanColor;
		}
	}

	public void OnHoverOver()
	{
		ServerName.color = hoverColor;
	}

	public void OnHoverOff()
	{
		SetUnHoveredColor();
	}


}
