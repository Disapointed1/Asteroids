using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ShipWeapon
{
    private readonly float _bulletLifetime;

    private readonly CancellationTokenSource _cts = new();
    private readonly float _fireRate;

    private bool _isDisposed;
    private float _lastFireTime;

    public ShipWeapon(BulletFactory bulletFactory, float fireRate, float bulletLifetime)
    {
        BulletPool = new ObjectPool<Bullet>(bulletFactory.Create);
        _fireRate = fireRate;
        _bulletLifetime = bulletLifetime;
    }

    public ObjectPool<Bullet> BulletPool { get; }

    public void Fire(Vector2 position, Vector2 direction, float speed, float rotation)
    {
        if (_isDisposed)
            return;

        if (Time.time - _lastFireTime < 1f / _fireRate)
            return;

        _lastFireTime = Time.time;

        var bullet = BulletPool.Get();
        bullet.Fire(position, direction * speed, rotation);
        ReturnBulletAfterDelay(bullet, _bulletLifetime, _cts.Token).Forget();
    }

    public void Dispose()
    {
        _isDisposed = true;
        _cts.Cancel();
        _cts.Dispose();
    }

    private async UniTask ReturnBulletAfterDelay(Bullet bullet, float delay, CancellationToken token)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: token);
        BulletPool.Return(bullet);
    }
}