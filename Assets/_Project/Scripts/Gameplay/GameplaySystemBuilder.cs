public class GameplaySystemBuilder
{
    private readonly CollisionSystemTicker _ticker;
    private readonly GameConfigFacade _configFacade;
    private readonly CollisionDetector _detector;
    private readonly CollisionResolver _resolver;

    public GameplaySystemBuilder(CollisionSystemTicker ticker, GameConfigFacade configFacade,  CollisionDetector detector, CollisionResolver resolver)
    {
        _ticker = ticker;
        _configFacade = configFacade;
        _detector = detector;
        _resolver = resolver;
    }

    public GameScore Build(Ship ship, ObjectPool<Bullet> bulletPool, ObjectPool<Asteroid> asteroidPool,
        ObjectPool<Ufo> ufoPool, LaserWeapon laserWeapon)
    {
        GameScore score = new GameScore();

        RewardService rewardService = new RewardService();

        CollisionSystem collisionSystem = new CollisionSystem(ship, bulletPool, asteroidPool, ufoPool, score,
            rewardService, _configFacade.Enemy.UfoSpeed, laserWeapon, _detector, _resolver);
        _ticker.Initialize(collisionSystem);

        return score;

    }

}