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



    public GameBootstrapper(ShipView shipView,  BulletFactory bulletFactory, LaserView laserView, AsteroidFactory asteroidFactory, CollisionSystemTicker ticker,  UfoFactory ufoFactory,  ShipStatusView shipStatusView)
    {
        _bulletFactory = bulletFactory;
        _shipView = shipView;
        _laserView = laserView;
        _asteroidFactory = asteroidFactory;
        _ticker = ticker;
        _ufoFactory = ufoFactory;
        _shipStatusView = shipStatusView;
    }

    public void Initialize()
    {
        float height = Camera.main.orthographicSize * 2f;
        float width = height * Camera.main.aspect;

        ShipWeapon shipWeapon = new ShipWeapon(_bulletFactory);
        WorldBoundary worldBoundary = new WorldBoundary(width, height);

        AsteroidSpawner asteroidSpawner = new AsteroidSpawner(_asteroidFactory,worldBoundary, 2f);
        asteroidSpawner.StartSpawning();

        Ship ship = new Ship(0.5f, 1f, 0.5f);

        ShipStatusViewModel shipStatusViewModel = new ShipStatusViewModel(ship);
        _shipStatusView.Initialize(shipStatusViewModel);

        KeyboardInputProvider inputProvider  =  new KeyboardInputProvider();
        ShipController shipController = new ShipController(ship, inputProvider, 180f, 5f, worldBoundary, shipWeapon, 15f);
        _laserView.Initialize(ship);

        UfoSpawner ufoSpawner = new UfoSpawner(_ufoFactory, worldBoundary, 3f, ship);
        CollisionSystem collisionSystem =
            new CollisionSystem(ship, shipWeapon.BulletPool, asteroidSpawner.AsteroidPool, ufoSpawner.UfoPool);

        _ticker.Initialize(collisionSystem);

        _shipView.Initialize(ship,shipController);


        ufoSpawner.StartSpawning();
    }
}
