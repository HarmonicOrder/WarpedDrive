using UnityEngine;
using System.Collections;
using System;
using System.Text;

public static class HarmonicUtils {

	/// <summary>
	/// Creates a new Color with given alpha.
	/// </summary>
	/// <returns>The color with alpha set to the alpha param.</returns>
	/// <param name="c">C.</param>
	/// <param name="alpha">Alpha.</param>
	public static Color ColorWithAlpha(Color c, float alpha)
	{
		return new Color(
			c.r,
			c.g,
			c.b,
			alpha
			);
	}

	public static Transform FindInChildren(Transform go, string name)
	{
		foreach (Transform x in go.GetComponentsInChildren<Transform>())
			if  (x.gameObject.name == name)
				return x;
		return null;
	}

    public static string HumanizeTimespan(TimeSpan obj)
    {
        StringBuilder sb = new StringBuilder();
        if (obj.Hours != 0)
        {
            sb.Append(obj.Hours);
            sb.Append(" ");
            sb.Append("hours");
            sb.Append(" ");
        }
        if (obj.Minutes != 0 || sb.Length != 0)
        {
            sb.Append(obj.Minutes);
            sb.Append(" ");
            sb.Append("minutes");
            sb.Append(" ");
        }

        sb.Append(obj.Seconds);
        sb.Append(" ");
        sb.Append("seconds");

        return sb.ToString();
    }
}
