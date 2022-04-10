using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGamePanel : MonoBehaviour {
    [SerializeField] private Button _restartButton;

    private void OnEnable() {
        _restartButton.onClick.AddListener(RestartGame);
    }

    private void OnDisable() {
        _restartButton.onClick.RemoveListener(RestartGame);
    }

    private void RestartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
