using TMPro;
using UnityEngine;

public class GameOverView : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private TextMeshProUGUI _finalScoreText;

    private GameOverViewModel _viewModel;

    public void Initialize(GameOverViewModel viewModel)
    {
        _viewModel = viewModel;
        _viewModel.OnGameOver += ShowGameOver;
        _panel.SetActive(false);
    }

    private void ShowGameOver()
    {
        _finalScoreText.text = _viewModel.FinalScoreText;
        _panel.SetActive(true);
        Time.timeScale = 0;
    }

}
