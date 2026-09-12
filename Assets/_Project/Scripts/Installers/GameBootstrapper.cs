using UnityEngine;
using Zenject;

public class GameBootstrapper : IInitializable
{
    private readonly PlayerBuilder _playerBuilder;
    private readonly EnemySystemBuilder _enemySystemBuilder;
    private readonly GameplaySystemBuilder _gameplaySystemBuilder;
    private readonly InputProviderFactory _inputProviderFactory;
    private readonly ShipStatusView _shipStatusView;
    private readonly GameScoreView _gameScoreView;
    private readonly GameOverView _gameOverView;
    private readonly FullScreenAdService _fullScreenAdService;
    private readonly SignalBus _signalBus;
    private readonly GameConfigFacade _gameConfigFacade;

    public GameBootstrapper(PlayerBuilder playerBuilder, EnemySystemBuilder enemySystemBuilder,
        GameplaySystemBuilder gameplaySystemBuilder,
        InputProviderFactory inputProviderFactory, ShipStatusView shipStatusView, GameScoreView gameScoreView,
        GameOverView gameOverView,
        FullScreenAdService fullScreenAdService, SignalBus signalBus, GameConfigFacade gameConfigFacade)
    {
        _playerBuilder = playerBuilder;
        _enemySystemBuilder = enemySystemBuilder;
        _gameplaySystemBuilder = gameplaySystemBuilder;
        _inputProviderFactory = inputProviderFactory;
        _shipStatusView = shipStatusView;
        _gameScoreView = gameScoreView;
        _gameOverView = gameOverView;
        _fullScreenAdService = fullScreenAdService;
        _signalBus = signalBus;
        _gameConfigFacade = gameConfigFacade;
    }

    public void Initialize()
    {
        FirebaseAnalyticsService analyticsService = new FirebaseAnalyticsService();
        analyticsService.LogEvent("game_started");
        SceneLoader sceneLoader = new SceneLoader();
        EnemyCounterTracker enemyCounterTracker = new EnemyCounterTracker(_gameConfigFacade.World.MaxEnemiesOnMap);

        IInputProvider inputProvider = _inputProviderFactory.Create();

        float height = Camera.main.orthographicSize * 2f;
        float width = height * Camera.main.aspect;
        WorldBoundary worldBoundary = new WorldBoundary(width, height);

        Ship ship = _playerBuilder.Build(inputProvider, worldBoundary, out ShipWeapon shipWeapon, out LaserWeapon laserWeapon, out ShipController shipController);

        AsteroidSpawner asteroidSpawner = _enemySystemBuilder.BuildAsteroidSpawner(worldBoundary,enemyCounterTracker);
        UfoSpawner ufoSpawner = _enemySystemBuilder.BuildUfoSpawner(worldBoundary, enemyCounterTracker);

        GameScore score = _gameplaySystemBuilder.Build(ship, shipWeapon.BulletPool, asteroidSpawner.AsteroidPool, ufoSpawner.UfoPool, laserWeapon);

        GameScoreViewModel gameScoreViewModel = new GameScoreViewModel(score);
        _gameScoreView.Initialize(gameScoreViewModel);

        ShipStatusViewModel shipStatusViewModel = new ShipStatusViewModel(ship, laserWeapon);
        _shipStatusView.Initialize(shipStatusViewModel);

        GameOverViewModel gameOverViewModel = new GameOverViewModel(ship,shipWeapon,score, analyticsService, _fullScreenAdService, _signalBus, laserWeapon);
        _gameOverView.Initialize(gameOverViewModel, sceneLoader);
    }

}