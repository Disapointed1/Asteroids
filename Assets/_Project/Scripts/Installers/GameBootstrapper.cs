using UnityEngine;
using Zenject;

public class GameBootstrapper : IInitializable
{

    private readonly BulletFactory _bulletFactory;
    private readonly ShipView _shipView;
    private readonly LaserView _laserView;
    private readonly AsteroidFactory _asteroidFactory;



    public GameBootstrapper(ShipView shipView,  BulletFactory bulletFactory, LaserView laserView, AsteroidFactory asteroidFactory)
    {
        _bulletFactory = bulletFactory;
        _shipView = shipView;
        _laserView = laserView;
        _asteroidFactory = asteroidFactory;
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
        ShipController shipController = new ShipController(ship, inputProvider, 180f, 5f, worldBoundary, shipWeapon, 10f);
        _laserView.Initialize(ship);

        CollisionSystem

        _shipView.Initialize(ship,shipController);
    }
}
