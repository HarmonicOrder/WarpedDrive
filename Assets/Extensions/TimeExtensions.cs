using UnityEngine;
using System.Collections;

public static class InterruptTime {

	public static float InterruptScale { get; set; }

    static InterruptTime()
    {
        InterruptScale = 1f;
    }

    public static float deltaTime
    {
        get
        {
            return Time.deltaTime * InterruptScale;
        }
    }
}
