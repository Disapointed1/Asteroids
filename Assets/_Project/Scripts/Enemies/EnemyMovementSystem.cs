public class EnemyMovementSystem
{
    private readonly Ship _ship;
    private readonly AsteroidFactory _asteroidFactory;
    private readonly ObjectPool<Ufo> _ufoPool;
    private readonly float _ufoChaseSpeed;

    public EnemyMovementSystem(Ship ship, AsteroidFactory asteroidFactory, ObjectPool<Ufo> ufoPool, float ufoChaseSpeed)
    {
        _ship = ship;
        _asteroidFactory = asteroidFactory;
        _ufoPool = ufoPool;
        _ufoChaseSpeed = ufoChaseSpeed;
    }

    public void UpdateMovement(float deltaTime)
    {
        foreach (var asteroid in _asteroidFactory.GetAllInUseObjects())
            asteroid.Physics.UpdatePosition(deltaTime);

        foreach (var ufo in _ufoPool.InUseObjects)
        {
            ufo.Chase(_ship.Physics.Position, _ufoChaseSpeed, deltaTime);
            ufo.Physics.UpdatePosition(deltaTime);
        }
    }
}