public class BulletMovementSystem
{
    private readonly ObjectPool<Bullet> _bulletPool;

    public BulletMovementSystem(ObjectPool<Bullet> bulletPool)
    {
        _bulletPool = bulletPool;
    }

    public void UpdateMovement(float deltaTime)
    {
        foreach (var bullet in _bulletPool.InUseObjects)
            bullet.Physics.UpdatePosition(deltaTime);
    }
}