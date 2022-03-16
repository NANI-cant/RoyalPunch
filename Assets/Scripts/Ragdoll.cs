using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour {
    [SerializeField] private bool _enableOnAwake;
    [SerializeField] private Rigidbody _hips;
    [SerializeField] private bool _freezeHorizontal;
    [SerializeField] private List<Rigidbody> _bones;

    private bool _isActive = false;
    private Coroutine _loadingBones;

    private List<Vector3> _savedBonesPositions = new List<Vector3>();
    private List<Quaternion> _savedBonesRotations = new List<Quaternion>();

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
        _savedBonesPositions = new List<Vector3>();
        _savedBonesRotations = new List<Quaternion>();
        foreach (var bone in _bones) {
            _savedBonesPositions.Add(bone.transform.position);
            _savedBonesRotations.Add(bone.transform.rotation);
        }
    }

    public void LoadBones() {
        if (_bones.Count != _savedBonesPositions.Count) return;

        if (_loadingBones != null) {
            StopCoroutine(_loadingBones);
        }
        for (int i = 0; i < _bones.Count; i++) {
            _bones[i].transform.position = _savedBonesPositions[i];
            _bones[i].transform.rotation = _savedBonesRotations[i];
        }
    }

    public void LoadBonesSmooth(float duration) {
        if (_bones.Count != _savedBonesPositions.Count) return;

        if (_loadingBones != null) {
            StopCoroutine(_loadingBones);
        }
        _loadingBones = StartCoroutine(ExecuteSmoothLoading(duration));
    }

    private IEnumerator ExecuteSmoothLoading(float duration) {
        float passedTime = 0;
        float velocity = 1 / duration;

        List<Vector3> startPositions = new List<Vector3>();
        List<Quaternion> startRotations = new List<Quaternion>();
        for (int i = 0; i < _bones.Count; i++) {
            startPositions.Add(_bones[i].transform.position);
            startRotations.Add(_bones[i].transform.rotation);
        }

        while (passedTime < duration) {
            for (int i = 0; i < _bones.Count; i++) {
                _bones[i].transform.position = Vector3.Lerp(startPositions[i], _savedBonesPositions[i], velocity * passedTime);
                _bones[i].transform.rotation = Quaternion.Lerp(startRotations[i], _savedBonesRotations[i], velocity * passedTime);
            }
            passedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        LoadBones();
    }
}
