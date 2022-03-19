using System;
using UnityEngine;

public static class ObjectExtensions {

    public static object Log(this object obj, string message, bool when) {
        if (when) {
            Debug.Log(message);
        }
        return obj;
    }

    public static object Do(this object obj, Action action, bool when) {
        if (when) {
            action.Invoke();
        }
        return obj;
    }
}