using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

public static class MonoBehaviourExtensions {
    // Позволяет удобно запускать функции с задержкой
    public static Coroutine Invoke(this MonoBehaviour mb, Action f, float delay = 0f) {
        return mb.StartCoroutine(InvokeRoutine(f, delay));
    }

    // Позволяет удобно запускать корутины с задержкой
    public static Coroutine InvokeCoroutine(this MonoBehaviour mb, Func<IEnumerator> f, float delay = 0f) {
        return mb.StartCoroutine(InvokeRoutine(f, delay));
    }

    private static IEnumerator InvokeRoutine(Action f, float delay = 0f) {
        yield return new WaitForSeconds(delay);
        f();
    }

    private static IEnumerator InvokeRoutine(Func<IEnumerator> f, float delay = 0f) {
        yield return new WaitForSeconds(delay);
        yield return f();
    }


}

