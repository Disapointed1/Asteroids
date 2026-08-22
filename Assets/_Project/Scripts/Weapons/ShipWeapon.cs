using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class ShipWeapon
{
    private readonly ObjectPool<Bullet> _bulletPool;

    public ObjectPool<Bullet> BulletPool => _bulletPool;

    private async UniTaskVoid ReturnBulletAfterDelay(Bullet bullet, float delay)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(delay));
        _bulletPool.Return(bullet);
    }

    public ShipWeapon(BulletFactory bulletFactory)
    {
        _bulletPool =new ObjectPool<Bullet>(bulletFactory.Create);
    }


    public void Fire(Vector2 position, Vector2 direction, float speed,  float rotation)
    {
        Bullet bullet = _bulletPool.Get();
        bullet.Fire(position, direction * speed, rotation);
        ReturnBulletAfterDelay(bullet, 3f).Forget();
    }


}
