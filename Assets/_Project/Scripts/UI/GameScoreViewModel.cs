using System;

public class GameScoreViewModel
{
    private readonly GameScore _gameScore;

    public string ScoreText => $"Score: {_gameScore.Score}";

    public event Action OnScoreChanged;

    public GameScoreViewModel(GameScore gameScore)
    {
        _gameScore = gameScore;
        _gameScore.OnScoreChanged += HandleScoreChanged;

    }

    private void HandleScoreChanged()
    {
        OnScoreChanged?.Invoke();
    }


}
