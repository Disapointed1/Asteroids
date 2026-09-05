using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class ShipWeapon
{
    private const float BulletLifetime = 3f;

    private readonly ObjectPool<Bullet> _bulletPool;
    private readonly float _fireRate;
    private float _lastFireTime;

    public ObjectPool<Bullet> BulletPool => _bulletPool;

    public ShipWeapon(BulletFactory bulletFactory, float fireRate)
    {
        _bulletPool = new ObjectPool<Bullet>(bulletFactory.Create);
        _fireRate = fireRate;
    }

    public void Fire(Vector2 position, Vector2 direction, float speed, float rotation)
    {
        if (Time.time - _lastFireTime < 1f / _fireRate)
            return;

        _lastFireTime = Time.time;

        Bullet bullet = _bulletPool.Get();
        bullet.Fire(position, direction * speed, rotation);
        ReturnBulletAfterDelay(bullet, BulletLifetime).Forget();
    }

    private async UniTaskVoid ReturnBulletAfterDelay(Bullet bullet, float delay)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(delay));
        _bulletPool.Return(bullet);
    }
}