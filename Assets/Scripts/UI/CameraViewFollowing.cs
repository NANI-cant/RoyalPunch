using UnityEngine;
using Zenject;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Canvas))]
public class CameraViewFollowing : MonoBehaviour {
    private Camera _camera;
    private RectTransform _rectTransform;

    [Inject]
    private void Constructor(Camera camera) {
        _camera = camera;
        _rectTransform = GetComponent<RectTransform>();
        GetComponent<Canvas>().worldCamera = camera;
    }

    private void Update() {
        _rectTransform.LookAt(_camera.transform, Vector3.up);
        _rectTransform.Rotate(new Vector3(0, 180, 0), Space.Self);
    }
}
