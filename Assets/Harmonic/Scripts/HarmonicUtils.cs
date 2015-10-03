using UnityEngine;
using System.Collections;

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
}
