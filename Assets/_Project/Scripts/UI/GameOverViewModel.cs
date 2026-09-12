   using System;
   using Zenject;
   public class GameOverViewModel
   {
      private readonly GameScore _gameScore;
      private readonly SignalBus _signalBus;
      private readonly FirebaseAnalyticsService _analyticsService;
      private readonly FullScreenAdService _fullScreenAdService;
      private readonly Ship _ship;
      private readonly ShipWeapon _shipWeapon;
      private readonly LaserWeapon _laserWeapon;

      public string FinalScoreText => $"Game over! Score: {_gameScore.Score}";

      public event Action OnGameOver;

      public GameOverViewModel(Ship ship, ShipWeapon shipWeapon, GameScore gameScore,  FirebaseAnalyticsService analyticsService,  FullScreenAdService fullScreenAdService, SignalBus signalBus, LaserWeapon laserWeapon)
      {
         _gameScore = gameScore;
         _analyticsService = analyticsService;
         _ship = ship;
         _shipWeapon = shipWeapon;
         ship.OnDied += HandleShipDied;
         _signalBus = signalBus;
         _fullScreenAdService = fullScreenAdService;
         _laserWeapon = laserWeapon;
      }

      private void HandleShipDied()
      {
         _analyticsService.LogEvent("game_over");
         _fullScreenAdService.ShowAd();
         _signalBus.Fire<GameOverSignal>();
         OnGameOver?.Invoke();
         _ship.Dispose();
         _shipWeapon.Dispose();
         _laserWeapon.Dispose();
      }
   }
