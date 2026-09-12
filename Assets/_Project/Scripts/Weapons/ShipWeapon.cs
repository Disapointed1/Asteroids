using System;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class ShipWeapon
{
    private const float BulletLifetime = 3f;

    private readonly ObjectPool<Bullet> _bulletPool;
    private readonly float _fireRate;
    private float _lastFireTime;
    private CancellationTokenSource _cts = new CancellationTokenSource();

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
        ReturnBulletAfterDelay(bullet, BulletLifetime, _cts.Token).Forget();
    }

    public void Dispose()
    {
        _cts.Cancel();
        _cts.Dispose();
    }

    private async UniTask ReturnBulletAfterDelay(Bullet bullet, float delay, CancellationToken token)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: token);
        _bulletPool.Return(bullet);
    }
}