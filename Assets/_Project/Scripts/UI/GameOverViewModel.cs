using System;
using Zenject;

public class GameOverViewModel
{
    private readonly FirebaseAnalyticsService _analyticsService;
    private readonly CollisionSystem _collisionSystem;
    private readonly FullScreenAdService _fullScreenAdService;
    private readonly GameScore _gameScore;
    private readonly GameScoreViewModel _gameScoreViewModel;
    private readonly LaserWeapon _laserWeapon;
    private readonly Ship _ship;
    private readonly ShipWeapon _shipWeapon;
    private readonly SignalBus _signalBus;

    public GameOverViewModel(Ship ship, ShipWeapon shipWeapon, GameScore gameScore,
        FirebaseAnalyticsService analyticsService,
        FullScreenAdService fullScreenAdService, SignalBus signalBus, LaserWeapon laserWeapon,
        CollisionSystem collisionSystem, GameScoreViewModel gameScoreViewModel)
    {
        _gameScore = gameScore;
        _analyticsService = analyticsService;
        _ship = ship;
        _shipWeapon = shipWeapon;
        ship.OnDied += HandleShipDied;
        _signalBus = signalBus;
        _fullScreenAdService = fullScreenAdService;
        _laserWeapon = laserWeapon;
        _collisionSystem = collisionSystem;
        _gameScoreViewModel = gameScoreViewModel;
    }

    public string FinalScoreText => $"Game over! Score: {_gameScore.Score}";

    public event Action OnGameOver;

    private void HandleShipDied()
    {
        _ship.OnDied -= HandleShipDied;
        _analyticsService.LogGameOver();
        _fullScreenAdService.ShowAd();
        _signalBus.Fire<GameOverSignal>();
        OnGameOver?.Invoke();
        _ship.Dispose();
        _shipWeapon.Dispose();
        _laserWeapon.Dispose();
        _collisionSystem.Dispose();
        _gameScoreViewModel.Dispose();
    }
}