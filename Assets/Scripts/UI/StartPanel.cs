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
        [SerializeField] private UnityAction _startGameAction;

        public UnityAction StartGameAction => _startGameAction;

        private void OnEnable() {
            _startGameButton.onClick.AddListener(_startGameAction);
        }

        private void OnDisable() {
            _startGameButton.onClick.RemoveListener(_startGameAction);
        }
    }
}
