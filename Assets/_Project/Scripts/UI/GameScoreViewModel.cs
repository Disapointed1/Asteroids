using System;

public class GameScoreViewModel
{
    private readonly GameScore _gameScore;

    public GameScoreViewModel(GameScore gameScore)
    {
        _gameScore = gameScore;
        _gameScore.OnScoreChanged += HandleScoreChanged;
    }

    public string ScoreText => $"Score: {_gameScore.Score}";

    public event Action OnScoreChanged;

    private void HandleScoreChanged()
    {
        OnScoreChanged?.Invoke();
    }

    public void Dispose()
    {
        _gameScore.OnScoreChanged -= HandleScoreChanged;
    }
}