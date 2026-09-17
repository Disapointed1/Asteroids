using Zenject;
using UnityEngine;

public class LevelFactory
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

   private readonly AsteroidFactory _asteroidFactory;

   public LevelFactory(Camera camera, EnemySystemBuilder enemySystemBuilder,
       FirebaseAnalyticsService firebaseAnalyticsService, FullScreenAdService fullScreenAdService,
       GameConfigProvider gameConfigProvider,
       GameOverView gameOverView, GameplaySystemBuilder gameplaySystemBuilder,
       GameScoreView gameScoreView, InputProviderFactory inputProviderFactory,
       PauseService pauseService, PlayerBuilder playerBuilder, SceneLoader sceneLoader
       , ShipStatusView shipStatusView, SignalBus signalBus, AsteroidFactory asteroidFactory)
   {
       _camera = camera;
       _enemySystemBuilder = enemySystemBuilder;
       _firebaseAnalyticsService = firebaseAnalyticsService;
       _fullScreenAdService = fullScreenAdService;
       _gameConfigProvider = gameConfigProvider;
       _gameOverView = gameOverView;
       _gameplaySystemBuilder = gameplaySystemBuilder;
       _gameScoreView = gameScoreView;
       _inputProviderFactory = inputProviderFactory;
       _pauseService = pauseService;
       _playerBuilder = playerBuilder;
       _sceneLoader = sceneLoader;
       _shipStatusView = shipStatusView;
       _signalBus = signalBus;
       _asteroidFactory = asteroidFactory;
   }

   public void BuildLevel()
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

       var score = _gameplaySystemBuilder.Build(ship, shipWeapon.BulletPool, _asteroidFactory,
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
