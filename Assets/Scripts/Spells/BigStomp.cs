using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "BigStomp", menuName = "ScriptableObjects/Spells/BigStomp")]
public class BigStomp : Spell {
    [Min(0)]
    [SerializeField] private float _maxDistanceToCast;
    [Min(0)]
    [SerializeField] private float _minDistanceToCast;
    [Min(0)]
    [SerializeField] private float _radius;
    [Min(0)]
    [SerializeField] private float _timeForCircleExpansion;
    [SerializeField] private ParticleSystem _castParticles;
    [SerializeField] private ParticleSystem _poofParticles;
    [SerializeField] private SpriteRenderer _circleTemplate;

    public override UnityAction OnPerformed { get; set; }

    private float _savedPerformingTime;
    private bool _shouldFlushGizmos = false;

    private bool isEnoughTimePassed => (Time.time - _savedPerformingTime) >= _duration;

#if UNITY_EDITOR
    private void OnValidate() {
        if (_timeForCircleExpansion > _damageDelay) _timeForCircleExpansion = _damageDelay;
        if (_minDistanceToCast > _maxDistanceToCast) _minDistanceToCast = _maxDistanceToCast;
    }
#endif

    public override void Perform(MonoBehaviour performer) {
        _savedPerformingTime = Time.time;
        performer.StartCoroutine(ExecutePerforming(performer));
    }

    private IEnumerator ExecutePerforming(MonoBehaviour performer) {
        performer.GetComponent<EnemyAvatar>().PlayBigStomp();

        InstantiateEffects(performer, out Transform staticCircle, out Transform expandedCircle, out ParticleSystem castParticles);

        float currentRadius = 0f;
        float circleExpansionSpeed = _radius / _timeForCircleExpansion;
        while (currentRadius < _radius) {
            currentRadius += circleExpansionSpeed * Time.deltaTime; ;
            expandedCircle.localScale = Vector3.one * currentRadius;

            yield return new WaitForEndOfFrame();
        }
        currentRadius = _radius;
        expandedCircle.localScale = Vector3.one * currentRadius;

        yield return new WaitForSeconds(_damageDelay - _timeForCircleExpansion);
        DestroyEffects(staticCircle, expandedCircle, castParticles);
        Instantiate(_poofParticles, performer.transform);
        _shouldFlushGizmos = true;
        DoDamageTo(GetEnemiesTypesAround<IDamageable>(performer));
        DoStunTo(GetEnemiesTypesAround<IStunnable>(performer));

        yield return new WaitForSeconds(_duration - _damageDelay);
        _shouldFlushGizmos = false;
        OnPerformed?.Invoke();
    }

    private void DestroyEffects(Transform staticCircle, Transform expandedCircle, ParticleSystem castParticles) {
        Destroy(castParticles.gameObject);
        Destroy(staticCircle.gameObject);
        Destroy(expandedCircle.gameObject);
    }

    private void InstantiateEffects(MonoBehaviour performer, out Transform staticCircle, out Transform expandedCircle, out ParticleSystem castParticles) {
        staticCircle = Instantiate(_circleTemplate, performer.transform.position, Quaternion.Euler(-90, 0, 0), performer.transform).transform;
        expandedCircle = Instantiate(_circleTemplate, performer.transform.position, Quaternion.Euler(-90, 0, 0), performer.transform).transform;
        castParticles = Instantiate(_castParticles, performer.transform.position + Vector3.up, Quaternion.identity, performer.transform);
        staticCircle.localScale = Vector3.one * _radius;
    }

    private void DoDamageTo(List<IDamageable> damageables) {
        foreach (var damageable in damageables) {
            damageable.TakeDamage(_damage);
        }
    }

    private void DoStunTo(List<IStunnable> stunnables) {
        foreach (var stunnable in stunnables) {
            stunnable.Stun();
        }
    }

    private List<T> GetEnemiesTypesAround<T>(MonoBehaviour performer) where T : class {
        List<T> types = new List<T>();

        Collider[] enemiesColliders = Physics.OverlapSphere(
            performer.transform.position,
            _radius, _whatIsDamageable,
            QueryTriggerInteraction.Ignore
            );
        if (enemiesColliders.Length <= 0) {
            return types;
        }

        foreach (var collider in enemiesColliders) {
            if (collider.TryGetComponent<T>(out T type)) {
                types.Add(type);
            }
        }
        return types;
    }

    public override bool CheckCouldPerform(MonoBehaviour performer) {
        if (isEnoughTimePassed == false) return false;

        Collider[] enemiesColliders = Physics.OverlapSphere(
            performer.transform.position,
            _radius, _whatIsDamageable,
            QueryTriggerInteraction.Ignore
            );
        if (enemiesColliders.Length <= 0) {
            return false;
        }

        foreach (var collider in enemiesColliders) {
            float distance = Mathf.Abs((collider.transform.position - performer.transform.position).magnitude);
            if (distance <= _maxDistanceToCast && distance >= _minDistanceToCast) {
                return true;
            }
        }

        return false;
    }

    public override void DrawGizmos(MonoBehaviour performer) {
        base.DrawGizmos(performer);
        if (!_shouldDrawGizmos) return;

        Gizmos.matrix = Matrix4x4.TRS(performer.transform.position, performer.transform.rotation, performer.transform.lossyScale);
        Gizmos.color = Color.red;
        if (_shouldFlushGizmos) {
            Gizmos.DrawSphere(Vector3.zero, _radius);
        }
        else {
            Gizmos.DrawWireSphere(Vector3.zero, _radius);
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(Vector3.zero, _minDistanceToCast);
        Gizmos.DrawWireSphere(Vector3.zero, _maxDistanceToCast);
    }
}