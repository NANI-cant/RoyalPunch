using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour {
    [SerializeField] private bool _enableOnAwake;
    [SerializeField] private Rigidbody _hips;
    [SerializeField] private bool _freezeHorizontal;
    [SerializeField] private List<Rigidbody> _bones;

    private bool _isActive = false;

    private List<Transform> _savedBones = new List<Transform>();

    private void Start() {
        _isActive = _enableOnAwake;
        if (_enableOnAwake) {
            Enable();
        }
        else {
            Disable();
        }
    }

    private void Update() {
        if (!_isActive) return;
        if (!_freezeHorizontal) return;

        _hips.transform.position = new Vector3(transform.position.x, _hips.transform.position.y, transform.position.z);
    }

    public void Enable() {
        _isActive = true;
        foreach (var bone in _bones) {
            bone.isKinematic = false;
        }
    }

    public void Disable() {
        _isActive = false;
        foreach (var bone in _bones) {
            bone.isKinematic = true;
        }
    }

    public void SaveBones() {
        _savedBones = new List<Transform>();
        foreach (var bone in _bones) {
            _savedBones.Add(bone.transform);
        }
    }

    public void LoadBones() {
        if (_bones.Count != _savedBones.Count) return;

        for (int i = 0; i < _bones.Count; i++) {
            _bones[i].transform.position = _savedBones[i].transform.position;
            _bones[i].transform.rotation = _savedBones[i].transform.rotation;
            _bones[i].transform.localScale = _savedBones[i].transform.localScale;
        }
    }

    public void LoadBonesSmooth(float duration) {
        if (_bones.Count != _savedBones.Count) return;

        StartCoroutine(ExecuteSmoothLoading(duration));
    }

    private IEnumerator ExecuteSmoothLoading(float duration) {
        float passedTime = 0;

        while (passedTime < duration) {
            for (int i = 0; i < _bones.Count; i++) {
                Vector3 transitionVelocity = (_savedBones[i].position - _bones[i].transform.position) / duration;
                float rotationVelocity = 1 / duration;

                _bones[i].transform.Translate(transitionVelocity * Time.deltaTime);
                _bones[i].transform.rotation = Quaternion.Lerp(_bones[i].transform.rotation, _savedBones[i].rotation, rotationVelocity * Time.deltaTime);
            }
            passedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        LoadBones();
    }
}
