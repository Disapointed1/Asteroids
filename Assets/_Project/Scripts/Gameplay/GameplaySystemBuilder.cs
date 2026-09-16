using System.Collections.Generic;

public class GameplaySystemBuilder
{
    private readonly GameConfigProvider _configProvider;
    private readonly CollisionDetector _detector;
    private readonly CollisionResolver _resolver;
    private readonly CollisionSystemTicker _ticker;

    public GameplaySystemBuilder(CollisionSystemTicker ticker, GameConfigProvider configProvider,
        CollisionDetector detector, CollisionResolver resolver)
    {
        _ticker = ticker;
        _configProvider = configProvider;
        _detector = detector;
        _resolver = resolver;
    }

    public GameScore Build(Ship ship, ObjectPool<Bullet> bulletPool, AsteroidFactory asteroidFactory,
        ObjectPool<Ufo> ufoPool, LaserWeapon laserWeapon, out CollisionSystem collisionSystem)
    {
        var score = new GameScore();
        var rewardService = new RewardService(
            _configProvider.Enemy.AsteroidLargeReward,
            _configProvider.Enemy.AsteroidMediumReward,
            _configProvider.Enemy.AsteroidSmallReward,
            _configProvider.Enemy.UfoReward);

        var bulletHandler =
            new BulletCollisionHandler(bulletPool, asteroidFactory, ufoPool, _detector, rewardService, score);
        var shipHandler = new ShipCollisionHandler(ship, asteroidFactory, ufoPool, _detector, _resolver);
        var laserHandler = new LaserCollisionHandler(ship, asteroidFactory, ufoPool, _detector, laserWeapon,
            rewardService, score);

        var handlers = new List<ICollisionHandler> { bulletHandler, shipHandler };
        collisionSystem = new CollisionSystem(handlers, laserHandler);

        var bulletMovementSystem = new BulletMovementSystem(bulletPool);
        var enemyMovementSystem =
            new EnemyMovementSystem(ship, asteroidFactory, ufoPool, _configProvider.Enemy.UfoSpeed);
        _ticker.Initialize(collisionSystem, bulletMovementSystem, enemyMovementSystem);

        return score;
    }
}