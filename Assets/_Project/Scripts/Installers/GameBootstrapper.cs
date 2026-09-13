using UnityEngine;
using Zenject;

public class GameBootstrapper : IInitializable
{
    private const float OrthographicSizeToHeightMultiplier = 2f;
    private readonly Camera _camera;
    private readonly EnemySystemBuilder _enemySystemBuilder;
    private readonly FirebaseAnalyticsService _firebaseAnalyticsService;
    private readonly FullScreenAdService _fullScreenAdService;
    private readonly GameConfigProvider _gameConfigProvider;
    private readonly GameOverView _gameOverView;
    private readonly GameplaySystemBuilder _gameplaySystemBuilder;
    private readonly GameScoreView _gameScoreView;
    private readonly InputProviderFactory _inputProviderFactory;
    private readonly PauseService _pauseService;

    private readonly PlayerBuilder _playerBuilder;
    private readonly SceneLoader _sceneLoader;
    private readonly ShipStatusView _shipStatusView;
    private readonly SignalBus _signalBus;

    public GameBootstrapper(PlayerBuilder playerBuilder, EnemySystemBuilder enemySystemBuilder,
        GameplaySystemBuilder gameplaySystemBuilder,
        InputProviderFactory inputProviderFactory, ShipStatusView shipStatusView, GameScoreView gameScoreView,
        GameOverView gameOverView,
        FullScreenAdService fullScreenAdService, SignalBus signalBus, GameConfigProvider gameConfigProvider,
        Camera camera,
        FirebaseAnalyticsService firebaseAnalyticsService, SceneLoader sceneLoader, PauseService pauseService)
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
        _gameConfigProvider = gameConfigProvider;
        _camera = camera;
        _firebaseAnalyticsService = firebaseAnalyticsService;
        _sceneLoader = sceneLoader;
        _pauseService = pauseService;
    }

    public void Initialize()
    {
        var enemyCounterTracker = new EnemyCounterTracker(_gameConfigProvider.World.MaxEnemiesOnMap);

        var inputProvider = _inputProviderFactory.Create();

        var height = _camera.orthographicSize * OrthographicSizeToHeightMultiplier;
        var width = height * _camera.aspect;
        var worldBoundary = new WorldBoundary(width, height);

        var ship = _playerBuilder.Build(inputProvider, worldBoundary, out var shipWeapon,
            out var laserWeapon, out var shipController);

        var asteroidSpawner = _enemySystemBuilder.BuildAsteroidSpawner(worldBoundary, enemyCounterTracker);
        var ufoSpawner = _enemySystemBuilder.BuildUfoSpawner(worldBoundary, enemyCounterTracker);

        var score = _gameplaySystemBuilder.Build(ship, shipWeapon.BulletPool, asteroidSpawner.AsteroidPool,
            ufoSpawner.UfoPool, laserWeapon, out var collisionSystem);

        var gameScoreViewModel = new GameScoreViewModel(score);
        _gameScoreView.Initialize(gameScoreViewModel);

        var shipStatusViewModel = new ShipStatusViewModel(ship, laserWeapon);
        _shipStatusView.Initialize(shipStatusViewModel);

        var gameOverViewModel = new GameOverViewModel(ship, shipWeapon, score, _firebaseAnalyticsService,
            _fullScreenAdService, _signalBus, laserWeapon, collisionSystem, gameScoreViewModel);
        _gameOverView.Initialize(gameOverViewModel, _sceneLoader, _pauseService);
    }
}