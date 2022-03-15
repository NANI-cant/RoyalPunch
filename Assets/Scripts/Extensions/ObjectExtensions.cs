using UnityEngine;

public static class ObjectExtensions {

    public static object Log(this object obj, string message, bool when) {
        if (when) {
            Debug.Log(message);
        }
        return obj;
    }
}