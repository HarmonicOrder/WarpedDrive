using UnityEngine;
using System.Collections;

public static class ToastLog {

    public delegate void ToastHandler(string message);

    public static event ToastHandler OnToast;

	public static void Toast(string message)
    {
        if (OnToast != null)
        {
            OnToast(message);
        }
    }
}
