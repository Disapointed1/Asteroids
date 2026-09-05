using UnityEngine;
using Zenject;

public class GameBootstrapper : IInitializable
{
    private readonly BulletFactory _bulletFactory;
    private readonly ShipView _shipView;
    private readonly LaserView _laserView;
    private readonly AsteroidFactory _asteroidFactory;
    private readonly CollisionSystemTicker _ticker;
    private readonly UfoFactory _ufoFactory;
    private readonly ShipStatusView _shipStatusView;
    private readonly GameScoreView _scoreView;
    private readonly GameOverView _gameOverView;
    private readonly Joystick _joystick;
    private readonly TouchButton _fireButton;
    private readonly TouchButton _laserButton;
    private readonly FullScreenAdService _fullScreenAdService;
    private readonly SignalBus _signalBus;
    private readonly GameConfigFacade _configFacade;

    public GameBootstrapper(ShipView shipView, BulletFactory bulletFactory, LaserView laserView, AsteroidFactory asteroidFactory, CollisionSystemTicker ticker,
        UfoFactory ufoFactory, ShipStatusView shipStatusView, GameScoreView gameScoreView,
        GameOverView gameOverView, Joystick joystick, [Inject(Id = "Fire")] TouchButton fireButton, [Inject(Id = "Laser")] TouchButton laserButton,
        FullScreenAdService fullScreenAdService, SignalBus signalBus, GameConfigFacade configFacade)
    {
        _bulletFactory = bulletFactory;
        _shipView = shipView;
        _laserView = laserView;
        _asteroidFactory = asteroidFactory;
        _ticker = ticker;
        _ufoFactory = ufoFactory;
        _shipStatusView = shipStatusView;
        _scoreView = gameScoreView;
        _gameOverView = gameOverView;
        _joystick = joystick;
        _fireButton = fireButton;
        _laserButton = laserButton;
        _fullScreenAdService = fullScreenAdService;
        _signalBus = signalBus;
        _configFacade = configFacade;
    }

    public void Initialize()
    {
        FirebaseAnalyticsService analyticsService = new FirebaseAnalyticsService();
        analyticsService.LogEvent("game_started");

        SceneLoader sceneLoader = new SceneLoader();

        EnemyCounterTracker enemyCounterTracker = new EnemyCounterTracker(_configFacade.World.MaxEnemiesOnMap);

        IInputProvider inputProvider;

        if (Application.isMobilePlatform)
        {
            inputProvider = new TouchInputProvider(_joystick, _fireButton, _laserButton);
        }
        else
        {
            inputProvider = new KeyboardInputProvider();
            _joystick.gameObject.SetActive(false);
            _fireButton.gameObject.SetActive(false);
            _laserButton.gameObject.SetActive(false);
        }

        GameScore score = new GameScore();
        GameScoreViewModel gameScoreViewModel = new GameScoreViewModel(score);
        _scoreView.Initialize(gameScoreViewModel);

        RewardService rewardService = new RewardService();

        float height = Camera.main.orthographicSize * 2f;
        float width = height * Camera.main.aspect;

        ShipWeapon shipWeapon = new ShipWeapon(_bulletFactory, _configFacade.Player.FireRate);
        WorldBoundary worldBoundary = new WorldBoundary(width, height);

        AsteroidSpawner asteroidSpawner = new AsteroidSpawner(_asteroidFactory, worldBoundary, _configFacade.Enemy.AsteroidSpeed, enemyCounterTracker, _signalBus);
        asteroidSpawner.StartSpawning();

        Ship ship = new Ship(_configFacade.Player.ShipRadius, _configFacade.Player.ShipMass, _configFacade.Player.ShipDragCoefficient, _configFacade.Player.MaxSpeed, _configFacade.Player.MaxHealth);

        ShipStatusViewModel shipStatusViewModel = new ShipStatusViewModel(ship);
        _shipStatusView.Initialize(shipStatusViewModel);

        GameOverViewModel gameOverViewModel = new GameOverViewModel(ship, score, analyticsService, _fullScreenAdService, _signalBus);
        _gameOverView.Initialize(gameOverViewModel, sceneLoader);

        ShipController shipController = new ShipController(ship, inputProvider, _configFacade.Player.RotationSpeed, _configFacade.Player.ThrustPower, worldBoundary, shipWeapon, _configFacade.Player.BulletSpeed);
        _laserView.Initialize(ship);

        UfoSpawner ufoSpawner = new UfoSpawner(_ufoFactory, worldBoundary, _configFacade.Enemy.UfoSpeed, ship, enemyCounterTracker, _signalBus);
        CollisionSystem collisionSystem =
            new CollisionSystem(ship, shipWeapon.BulletPool, asteroidSpawner.AsteroidPool, ufoSpawner.UfoPool, score, rewardService);

        _ticker.Initialize(collisionSystem);

        _shipView.Initialize(ship, shipController);

        ufoSpawner.StartSpawning();
    }
}