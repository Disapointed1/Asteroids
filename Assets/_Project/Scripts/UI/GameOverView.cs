using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverView : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private TextMeshProUGUI _finalScoreText;
    [SerializeField] private Button _restartButton;
    private PauseService _pauseService;
    private SceneLoader _sceneLoader;

    private GameOverViewModel _viewModel;

    private void OnDestroy()
    {
        _viewModel.OnGameOver -= ShowGameOver;
        _restartButton.onClick.RemoveListener(HandleRestartClicked);
    }

    public void Initialize(GameOverViewModel viewModel, SceneLoader sceneLoader, PauseService pauseService)
    {
        _viewModel = viewModel;
        _sceneLoader = sceneLoader;
        _pauseService = pauseService;
        _viewModel.OnGameOver += ShowGameOver;
        _restartButton.onClick.AddListener(HandleRestartClicked);
        _panel.SetActive(false);
    }

    private void ShowGameOver()
    {
        _finalScoreText.text = _viewModel.FinalScoreText;
        _panel.SetActive(true);
        _pauseService.Pause();
    }

    private void HandleRestartClicked()
    {
        _pauseService.Resume();
        _sceneLoader.ReloadCurrentScene();
    }
}