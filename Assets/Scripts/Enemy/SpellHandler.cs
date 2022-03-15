using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class SpellHandler : MonoBehaviour {
    [SerializeField] private List<Spell> _spells;
    [Min(0)]
    [SerializeField] private float _reloadTime = 0f;

    public UnityAction OnCastEnd;

    private float _savedCastTime = float.NegativeInfinity;

    private bool _isEnoughTimePassed => (Time.time - _savedCastTime) >= _reloadTime;

    private void Awake() {
        List<Spell> _spellsInstances = new List<Spell>();
        foreach (var spell in _spells) {
            _spellsInstances.Add(Instantiate(spell));
        }
        _spells = _spellsInstances;
    }

    private void OnEnable() {
        foreach (var spell in _spells) {
            spell.OnPerformed += EndCast;
        }
    }

    private void OnDisable() {
        foreach (var spell in _spells) {
            spell.OnPerformed -= EndCast;
        }
    }

    public bool CouldCast() {
        if (_isEnoughTimePassed == false) return false;

        foreach (var spell in _spells) {
            if (spell.CheckCouldPerform(this)) {
                return true;
            }
        }
        return false;
    }

    public void CastRandom() {
        List<Spell> availableSpells = _spells.Where<Spell>(s => s.CheckCouldPerform(this)).ToList();
        int randomId = Random.Range(0, availableSpells.Count());
        _savedCastTime = Time.time;
        availableSpells[randomId].Perform(this);
    }

    private void OnDrawGizmos() {
        foreach (var spell in _spells) {
            spell.DrawGizmos(this);
        }
    }

    private void EndCast() {
        OnCastEnd?.Invoke();
    }
}
