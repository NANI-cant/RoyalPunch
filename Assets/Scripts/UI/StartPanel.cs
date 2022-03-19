using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI {
    public class StartPanel : MonoBehaviour {
        [SerializeField] private Button _healthButton;
        [SerializeField] private Button _strengthButton;
        [SerializeField] private Button _startGameButton;

        public UnityAction OnHitPlayButton;

        private void OnEnable() {
            _startGameButton.onClick.AddListener(InvokeStartAction);
        }

        private void OnDisable() {
            _startGameButton.onClick.RemoveListener(InvokeStartAction);
        }

        private void InvokeStartAction() {
            OnHitPlayButton?.Invoke();
        }
    }
}
