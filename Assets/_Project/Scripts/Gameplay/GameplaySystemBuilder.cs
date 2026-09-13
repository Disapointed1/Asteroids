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

    public GameScore Build(Ship ship, ObjectPool<Bullet> bulletPool, ObjectPool<Asteroid> asteroidPool,
        ObjectPool<Ufo> ufoPool, LaserWeapon laserWeapon, out CollisionSystem collisionSystem)
    {
        var score = new GameScore();
        var rewardService = new RewardService(
            _configProvider.Enemy.AsteroidLargeReward,
            _configProvider.Enemy.AsteroidMediumReward,
            _configProvider.Enemy.AsteroidSmallReward,
            _configProvider.Enemy.UfoReward);


        collisionSystem = new CollisionSystem(ship, bulletPool, asteroidPool, ufoPool, score,
            rewardService, _configProvider.Enemy.UfoSpeed, laserWeapon, _detector, _resolver);
        _ticker.Initialize(collisionSystem);

        return score;
    }
}