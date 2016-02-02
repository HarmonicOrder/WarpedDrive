using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ServerSelector : MonoBehaviour {
	public TextMesh ServerName;
	public Color infectedColor;
	public Color cleanColor;
	public Color hoverColor;
    public Color lineConnectedColor;
    public Color lineBlockedColor;
    public Material LineMaterial;
	public SpriteRenderer InfectedSpriteRenderer;
    public List<ServerSelector> Requirements;
    public bool CanBeSelected = true;

	private Color initialColor;
	public NetworkLocation MyNetworkLocation;

    void Awake()
    {
		MyNetworkLocation = NetworkMap.GetLocationByLocationName(name);
    }

    void Start()
	{
		initialColor = ServerName.color;

		if (MyNetworkLocation != null)
		{
			InfectedSpriteRenderer.enabled = MyNetworkLocation.IsInfected;

			SetUnHoveredColor();
		}

        ResolveDependencies();
	}

    private void ResolveDependencies()
    {
        if (this.Requirements != null && this.Requirements.Count > 0)
        {
            foreach(ServerSelector ss in this.Requirements)
            {
                GameObject g = new GameObject("requirement_line");
                g.transform.SetParent(this.transform);
                LineRenderer currentLine = g.AddComponent<LineRenderer>();
                currentLine.material = LineMaterial;
                currentLine.useWorldSpace = true;
                currentLine.SetWidth(100f, 100f);
                currentLine.SetVertexCount(2);
                currentLine.SetPosition(0, this.transform.position);
                currentLine.SetPosition(1, ss.transform.position);

                if (ss.MyNetworkLocation == null )
                {
                    UnityEngine.Debug.LogWarning("Dependency does not have network location");
                }
                else if (ss.MyNetworkLocation.IsInfected)
                {
                    currentLine.SetColors(lineBlockedColor, lineBlockedColor);
                }
                else
                {
                    currentLine.SetColors(lineConnectedColor, lineConnectedColor);
                }
            }

            this.CanBeSelected = this.Requirements.Exists(r => !r.MyNetworkLocation.IsInfected);
        }
    }

    private void SetUnHoveredColor()
	{
		if ((MyNetworkLocation != null) && MyNetworkLocation.IsInfected)
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
        if (this.CanBeSelected)
		    ServerName.color = hoverColor;
	}

	public void OnHoverOff()
	{
		SetUnHoveredColor();
	}


}
