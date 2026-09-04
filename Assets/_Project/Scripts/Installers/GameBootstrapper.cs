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

    public GameBootstrapper(ShipView shipView,  BulletFactory bulletFactory, LaserView laserView, AsteroidFactory asteroidFactory, CollisionSystemTicker ticker,
        UfoFactory ufoFactory,  ShipStatusView shipStatusView, GameScoreView gameScoreView,
        GameOverView gameOverView, Joystick joystick,[Inject(Id = "Fire")]TouchButton fireButton, [Inject(Id = "Laser")]TouchButton laserButton)
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
    }

    public void Initialize()
    {
        PlayerConfig playerConfig = ConfigLoader.Load<PlayerConfig>("player_config");
        EnemyConfig enemyConfig = ConfigLoader.Load<EnemyConfig>("enemy_config");
        WorldConfig worldConfig = ConfigLoader.Load<WorldConfig>("world_config");

        EnemyCounterTracker enemyCounterTracker = new EnemyCounterTracker(worldConfig.MaxEnemiesOnMap);


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

        ShipWeapon shipWeapon = new ShipWeapon(_bulletFactory);
        WorldBoundary worldBoundary = new WorldBoundary(width, height);

        AsteroidSpawner asteroidSpawner = new AsteroidSpawner(_asteroidFactory,worldBoundary, enemyConfig.AsteroidSpeed, enemyCounterTracker);
        asteroidSpawner.StartSpawning();

        Ship ship = new Ship(0.5f, 1f, 0.5f, playerConfig.MaxSpeed);

        ShipStatusViewModel shipStatusViewModel = new ShipStatusViewModel(ship);
        _shipStatusView.Initialize(shipStatusViewModel);


        GameOverViewModel gameOverViewModel = new GameOverViewModel(ship, score);
        _gameOverView.Initialize(gameOverViewModel);

        ShipController shipController = new ShipController(ship, inputProvider, playerConfig.RotationSpeed, playerConfig.ThrustPower, worldBoundary, shipWeapon, playerConfig.BulletSpeed);
        _laserView.Initialize(ship);

        UfoSpawner ufoSpawner = new UfoSpawner(_ufoFactory, worldBoundary, enemyConfig.UfoSpeed, ship, enemyCounterTracker);
        CollisionSystem collisionSystem =
            new CollisionSystem(ship, shipWeapon.BulletPool, asteroidSpawner.AsteroidPool, ufoSpawner.UfoPool, score, rewardService);

        _ticker.Initialize(collisionSystem);

        _shipView.Initialize(ship,shipController);

        ufoSpawner.StartSpawning();
    }
}
