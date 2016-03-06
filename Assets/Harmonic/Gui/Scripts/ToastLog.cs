using UnityEngine;
using System.Collections;
using System;
using System.ComponentModel;

public static class ToastLog {
    
    public class ToastEventArgs : EventArgs
    {
        public bool Handled { get; set; }
        public float Progess { get; set; }
        public string Message { get; set; }
        public Guid? ID { get; set; }
    }

    public static event EventHandler OnToast;

	public static void Toast(string message)
    {
        if (OnToast != null)
        {
            OnToast(null, new ToastEventArgs() { Message = message });
        }
    }

    public static void ToastSticky(string message, float progress, Guid id)
    {
        if (OnToast != null)
        {
            OnToast(null, new ToastEventArgs() {
                Message = message,
                Progess = progress,
                ID = id
            });
        }
    }
}
