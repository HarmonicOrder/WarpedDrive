using UnityEngine;
using System.Collections;
using System;
using System.Text;

public static class HarmonicUtils {
    private static int _targetLayerMask = -1;

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
        if (obj.Days != 0)
        {
            if (obj.Days > 365)
            {
                double years = Math.Floor(obj.Days / 365f);
                sb.Append(years);
                sb.Append(" ");
                sb.Append("year");
                if (years > 1)
                    sb.Append("s");
                sb.Append(" ");
            }
            sb.Append(obj.Days % 365);
            sb.Append(" ");
            sb.Append("day");
            if (obj.Days % 365 > 1)
                sb.Append("s");
            sb.Append(" ");
        }

        if (obj.Hours != 0)
        {
            sb.Append(obj.Hours);
            sb.Append(" ");
            sb.Append("hour");
            if (obj.Hours > 1)
                sb.Append("s");
            sb.Append(" ");
        }
        if (obj.Minutes != 0 || sb.Length != 0)
        {
            sb.Append(obj.Minutes);
            sb.Append(" ");
            sb.Append("minute");
            if (obj.Minutes > 1)
                sb.Append("s");
            sb.Append(" ");
        }

        sb.Append(Math.Round((float)obj.Seconds));
        sb.Append(" ");
        sb.Append("seconds");

        return sb.ToString();
    }

    public static string HumanizeKilograms(float kilograms)
    {
        if (kilograms < 1000)
        {
            float grams = kilograms * 1000;
            return (grams - grams % .01) + " g";
        }
        else
        {
            return kilograms - kilograms % .01 + " kg";
        }
    }

    public static string ClockFormat(float seconds)
    {
        return string.Format("{0}:{1}", Mathf.Floor(seconds / 60).ToString("00"), (Mathf.Floor(seconds % 60)).ToString("00"));
    }
    public static string ClockFormatWithDecisecond(float seconds)
    {
        return string.Format("{0}:{1}.{2}", Mathf.Floor(seconds / 60).ToString("00"), (Mathf.Floor(seconds % 60)).ToString("00"), (((int)(seconds * 10))%10).ToString());
    }

    internal static Vector3 RandomVector(float min, float max)
    {
        return new Vector3(UnityEngine.Random.Range(min, max), UnityEngine.Random.Range(min, max), UnityEngine.Random.Range(min, max));
    }

    public class LerpContext
    {
        public LerpContext(float duration)
        {
            this.Duration = duration;
        }

        public bool IsLerping { get; set; }
        public Vector3 From { get; set; }
        public Vector3 To { get; set; }
        public Quaternion FromQ { get; set; }
        public Quaternion ToQ { get; set; }
        public float CurrentTime { get; set; }
        public float Duration { get; set; }

        public void Reset(Vector3 from, Vector3 to)
        {
            From = from;
            To = to;
            CurrentTime = 0f;
            IsLerping = true;
        }

        public void Reset(Quaternion fQ, Quaternion tQ)
        {
            FromQ = fQ;
            ToQ = tQ;
            CurrentTime = 0f;
            IsLerping = true;
        }

        public Vector3 Finalize()
        {
            CurrentTime = 0f;
            IsLerping = false;
            return To;
        }

        public Quaternion FinalizeQ()
        {
            CurrentTime = 0f;
            IsLerping = false;
            return ToQ;
        }

        public Vector3 Hermite()
        {
            return Mathfx.Hermite(From, To, CurrentTime / Duration);
        }

        public Quaternion LerpQ()
        {
            return Quaternion.Lerp(FromQ, ToQ, CurrentTime / Duration);
        }
    }

    /// <summary>
    /// I read somewhere that this was expensive so this is a cached getter
    /// </summary>
    public static int TargetLayerMask
    {
        get
        {
            if (_targetLayerMask < 0)
                _targetLayerMask = 1 << LayerMask.NameToLayer("TargetRaycast");
            return _targetLayerMask;
        }
    }
}
