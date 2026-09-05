using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverView : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private TextMeshProUGUI _finalScoreText;
    [SerializeField] private Button _restartButton;

    private GameOverViewModel _viewModel;
    private SceneLoader _sceneLoader;

    public void Initialize(GameOverViewModel viewModel, SceneLoader sceneLoader)
    {
        _viewModel = viewModel;
        _sceneLoader = sceneLoader;
        _viewModel.OnGameOver += ShowGameOver;
        _restartButton.onClick.AddListener(HandleRestartClicked);
        _panel.SetActive(false);
    }

    private void ShowGameOver()
    {
        _finalScoreText.text = _viewModel.FinalScoreText;
        _panel.SetActive(true);
        Time.timeScale = 0;
    }
    private void HandleRestartClicked()
    {
        Time.timeScale = 1f;
        _sceneLoader.ReloadCurrentScene();
    }

}
