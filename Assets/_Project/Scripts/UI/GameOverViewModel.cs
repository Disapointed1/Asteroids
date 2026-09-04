using System;

public class GameOverViewModel
{
   private readonly GameScore _gameScore;

   public string FinalScoreText => $"Game over! Score: {_gameScore.Score}";

   public event Action OnGameOver;

   public GameOverViewModel(Ship ship, GameScore gameScore)
   {
      _gameScore = gameScore;
      ship.OnDied += HandleShipDied;
   }

   private void HandleShipDied()
   {
      OnGameOver?.Invoke();
   }
}
