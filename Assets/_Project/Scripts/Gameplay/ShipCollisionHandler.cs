public class ShipCollisionHandler : ICollisionHandler
{
    private readonly Ship _ship;
    private readonly AsteroidFactory _asteroidFactory;
    private readonly ObjectPool<Ufo> _ufoPool;
    private readonly CollisionDetector _collisionDetector;
    private readonly CollisionResolver _collisionResolver;

    public ShipCollisionHandler(Ship ship, AsteroidFactory asteroidFactory, ObjectPool<Ufo> ufoPool,
        CollisionDetector collisionDetector, CollisionResolver collisionResolver)
    {
        _ship = ship;
        _asteroidFactory = asteroidFactory;
        _ufoPool = ufoPool;
        _collisionDetector = collisionDetector;
        _collisionResolver = collisionResolver;
    }

    public void CheckCollisions()
    {
        foreach (var asteroid in _asteroidFactory.GetAllInUseObjects())
        {
            if (!_ship.IsInvulnerable && _collisionDetector.CheckCollision(_ship.Physics, asteroid.Physics))
            {
                _collisionResolver.ResolveCollision(_ship.Physics, asteroid.Physics);
                _ship.TakeDamage(1);
            }
        }

        foreach (var ufo in _ufoPool.InUseObjects)
        {
            if (!_ship.IsInvulnerable && _collisionDetector.CheckCollision(_ship.Physics, ufo.Physics))
            {
                _collisionResolver.ResolveCollision(_ship.Physics, ufo.Physics);
                _ship.TakeDamage(1);
            }
        }
    }
}