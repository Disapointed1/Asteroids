using TMPro;
using UnityEngine;

public class GameScoreView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;

    private GameScoreViewModel _viewModel;

    public void Initialize(GameScoreViewModel viewModel)
    {
        _viewModel = viewModel;
        _viewModel.OnScoreChanged += UpdateDisplay;
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        _scoreText.text = _viewModel.ScoreText;
    }


}
