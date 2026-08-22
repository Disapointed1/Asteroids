using UnityEngine;
using Zenject;

public class GameBootstrapper : IInitializable
{

    private readonly BulletFactory _bulletFactory;
    private readonly ShipView _shipView;
    private readonly LaserView _laserView;
    private readonly AsteroidFactory _asteroidFactory;
    private readonly CollisionSystemTicker _ticker;



    public GameBootstrapper(ShipView shipView,  BulletFactory bulletFactory, LaserView laserView, AsteroidFactory asteroidFactory, CollisionSystemTicker ticker)
    {
        _bulletFactory = bulletFactory;
        _shipView = shipView;
        _laserView = laserView;
        _asteroidFactory = asteroidFactory;
        _ticker = ticker;
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
        KeyboardInputProvider inputProvider  =  new KeyboardInputProvider();
        ShipController shipController = new ShipController(ship, inputProvider, 180f, 5f, worldBoundary, shipWeapon, 15f);
        _laserView.Initialize(ship);

        CollisionSystem collisionSystem =
            new CollisionSystem(ship, shipWeapon.BulletPool, asteroidSpawner.AsteroidPool);

        _ticker.Initialize(collisionSystem);

        _shipView.Initialize(ship,shipController);
    }
}
