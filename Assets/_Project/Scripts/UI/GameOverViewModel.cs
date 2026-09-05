   using System;
   using Zenject;
   public class GameOverViewModel
   {
      private readonly GameScore _gameScore;
      private readonly SignalBus _signalBus;
      private readonly FirebaseAnalyticsService _analyticsService;
      private readonly FullScreenAdService _fullScreenAdService;

      public string FinalScoreText => $"Game over! Score: {_gameScore.Score}";

      public event Action OnGameOver;

      public GameOverViewModel(Ship ship, GameScore gameScore,  FirebaseAnalyticsService analyticsService,  FullScreenAdService fullScreenAdService, SignalBus signalBus)
      {
         _gameScore = gameScore;
         _analyticsService = analyticsService;
         ship.OnDied += HandleShipDied;
         _signalBus = signalBus;
         _fullScreenAdService = fullScreenAdService;
      }

      private void HandleShipDied()
      {
         _analyticsService.LogEvent("game_over");
         _fullScreenAdService.ShowAd();
         _signalBus.Fire<GameOverSignal>();
         OnGameOver?.Invoke();
      }
   }
