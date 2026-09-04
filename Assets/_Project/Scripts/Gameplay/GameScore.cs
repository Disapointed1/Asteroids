using System;

public class GameScore
{
    public int Score { get; private set; }

    public event Action OnScoreChanged;

    public void AddScore(int amount)
    {
        Score += amount;
        OnScoreChanged?.Invoke();
    }
}
