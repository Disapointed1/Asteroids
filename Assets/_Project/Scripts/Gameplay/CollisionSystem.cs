using System.Collections.Generic;
using UnityEngine;

public class CollisionSystem
{
    private readonly Ship _ship;
    private readonly ObjectPool<Bullet> _bulletPool;
    private readonly ObjectPool<Asteroid> _asteroidPool;
    private readonly ObjectPool<Ufo> _ufoPool;
    private readonly CollisionDetector _collisionDetector;
    private readonly CollisionResolver _collisionResolver;


    public CollisionSystem(Ship ship, ObjectPool<Bullet> bulletPool, ObjectPool<Asteroid> asteroidPool,  ObjectPool<Ufo> ufoPool)
    {
        _ship = ship;
        _ufoPool  = ufoPool;
        _bulletPool = bulletPool;
        _asteroidPool = asteroidPool;
        _collisionDetector = new CollisionDetector();
        _collisionResolver = new CollisionResolver();
        _ship.OnLaserFired += HandleLaserFired;
    }

    public void CheckCollisions()
    {
        List<Bullet> bulletsToReturn = new List<Bullet>();
        List<Asteroid> asteroidsToHit = new List<Asteroid>();
        List<Ufo> ufosToHit = new List<Ufo>();

        foreach (Bullet bullet in _bulletPool.InUseObjects)
        {
            foreach (Asteroid asteroid in _asteroidPool.InUseObjects)
            {
                if (_collisionDetector.CheckCollision(bullet.PhysicsMovement, asteroid.Physics))
                {
                   asteroidsToHit.Add(asteroid);
                   bulletsToReturn.Add(bullet);
                }
            }

            foreach (Ufo ufo in _ufoPool.InUseObjects)
            {
                if (_collisionDetector.CheckCollision(bullet.PhysicsMovement, ufo.Physics))
                {
                    ufosToHit.Add(ufo);
                    bulletsToReturn.Add(bullet);
                }
            }

        }

        foreach (Bullet bullet in bulletsToReturn)
        {
            _bulletPool.Return(bullet);
        }

        foreach (Asteroid asteroid in asteroidsToHit)
        {
            asteroid.TakeHit();
        }

        foreach (Ufo ufo in ufosToHit)
        {
            ufo.TakeHit();
        }

        foreach (Asteroid asteroid in _asteroidPool.InUseObjects)
        {
            if (!_ship.IsInvulnerable && _collisionDetector.CheckCollision(_ship.Physics, asteroid.Physics))
            {
                _collisionResolver.ResolveCollision(_ship.Physics, asteroid.Physics);
                _ship.TakeDamage(1);
            }
        }
        foreach (Ufo ufo in _ufoPool.InUseObjects)
        {
            if (!_ship.IsInvulnerable && _collisionDetector.CheckCollision(_ship.Physics, ufo.Physics))
            {
                _collisionResolver.ResolveCollision(_ship.Physics, ufo.Physics);
                _ship.TakeDamage(1);
            }
        }
    }

    private void HandleLaserFired()
    {
        Vector2 direction = new Vector2(-Mathf.Sin(_ship.Rotation * Mathf.Deg2Rad), Mathf.Cos(_ship.Rotation * Mathf.Deg2Rad));
        Vector2 start = _ship.Physics.Position;
        Vector2 end = start + direction * 20f;

        List<Asteroid> asteroidsToHit = new List<Asteroid>();
        foreach (Asteroid asteroid in _asteroidPool.InUseObjects)
        {
            if (_collisionDetector.CheckLaserHit(start, end, asteroid.Physics))
            {
                asteroidsToHit.Add(asteroid);
            }
        }

        List <Ufo> ufosToHit = new List<Ufo>();
        foreach (Ufo ufo in _ufoPool.InUseObjects)
        {
            if (_collisionDetector.CheckLaserHit(start, end, ufo.Physics))
            {
                ufosToHit.Add(ufo);
            }
        }

        foreach (Asteroid asteroid in asteroidsToHit)
        {
            asteroid.TakeHit();
        }

        foreach (Ufo ufo in ufosToHit)
        {
            ufo.TakeHit();
        }
    }
}
