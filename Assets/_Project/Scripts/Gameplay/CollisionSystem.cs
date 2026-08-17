public class CollisionSystem
{
    private readonly Ship _ship;
    private readonly ObjectPool<Bullet> _bulletPool;
    private readonly ObjectPool<Asteroid> _asteroidPool;
    private readonly CollisionDetector _collisionDetector;
    private readonly CollisionResolver _collisionResolver;

    public CollisionSystem(Ship ship, ObjectPool<Bullet> bulletPool, ObjectPool<Asteroid> asteroidPool)
    {
        _ship = ship;
        _bulletPool = bulletPool;
        _asteroidPool = asteroidPool;
        _collisionDetector = new CollisionDetector();
        _collisionResolver = new CollisionResolver();
    }

    public void CheckCollisions()
    {
        foreach (Bullet bullet in _bulletPool.InUseObjects)
        {
            foreach (Asteroid asteroid in _asteroidPool.InUseObjects)
            {
                if (_collisionDetector.CheckCollision(bullet.PhysicsMovement, asteroid.Physics))
                {
                    asteroid.TakeHit();
                    _bulletPool.Return(bullet);
                }
            }

        }

        foreach (Asteroid asteroid in _asteroidPool.InUseObjects)
        {
            if (!_ship.IsInvulnerable && _collisionDetector.CheckCollision(_ship.Physics, asteroid.Physics))
            {
                _collisionResolver.ResolveCollision(_ship.Physics, asteroid.Physics);
                _ship.TakeDamage(1);
            }
        }
    }
}
