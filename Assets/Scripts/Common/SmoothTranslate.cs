using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class SmoothTranslate {
    private MonoBehaviour _coroutineRunner;
    private Coroutine _translationCoroutine;

    public UnityAction Translated;

    public SmoothTranslate(MonoBehaviour coroutineRunner) {
        _coroutineRunner = coroutineRunner;
    }

    public void Translate(Transform transform, float duration, Vector3 fromPoint, Vector3 toPoint, Quaternion fromRot, Quaternion toRot) {
        if (_translationCoroutine != null) {
            _coroutineRunner.StopCoroutine(_translationCoroutine);
        }
        _translationCoroutine = _coroutineRunner.StartCoroutine(ExecuteTranslation(transform, duration, fromPoint, toPoint, fromRot, toRot));
    }

    private IEnumerator ExecuteTranslation(Transform transform, float duration, Vector3 fromPoint, Vector3 toPoint, Quaternion fromRot, Quaternion toRot) {
        float speed = 1 / duration;
        float passedTime = 0f;
        while (passedTime <= duration) {
            passedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(fromPoint, toPoint, passedTime * speed);
            transform.rotation = Quaternion.Lerp(fromRot, toRot, passedTime * speed);
            yield return new WaitForFixedUpdate();
        }
        Translated?.Invoke();
    }
}