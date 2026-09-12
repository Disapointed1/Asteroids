using System.Collections.Generic;
using UnityEngine;

public class CollisionSystem
{
    private const float LaserRange = 20f;

    private readonly Ship _ship;
    private readonly ObjectPool<Bullet> _bulletPool;
    private readonly ObjectPool<Asteroid> _asteroidPool;
    private readonly ObjectPool<Ufo> _ufoPool;
    private readonly CollisionDetector _collisionDetector;
    private readonly CollisionResolver _collisionResolver;
    private readonly GameScore _score;
    private readonly RewardService _rewardService;
    private readonly float _ufoChaseSpeed;
    private readonly LaserWeapon _laserWeapon;

    public CollisionSystem(Ship ship, ObjectPool<Bullet> bulletPool, ObjectPool<Asteroid> asteroidPool,
        ObjectPool<Ufo> ufoPool, GameScore score, RewardService rewardService, float ufoChaseSpeed, LaserWeapon laserWeapon,
        CollisionDetector collisionDetector, CollisionResolver collisionResolver)
    {
        _ship = ship;
        _ufoPool = ufoPool;
        _bulletPool = bulletPool;
        _asteroidPool = asteroidPool;
        _score = score;
        _laserWeapon = laserWeapon;
        _rewardService = rewardService;
        _laserWeapon.OnFired += HandleLaserFired;
        _ufoChaseSpeed = ufoChaseSpeed;
        _collisionDetector = collisionDetector;
        _collisionResolver = collisionResolver;
    }

    public void CheckCollisions()
    {
        List<Bullet> bulletsToReturn = new List<Bullet>();
        List<Asteroid> asteroidsToHit = new List<Asteroid>();
        List<Ufo> ufosToHit = new List<Ufo>();

        float deltaTime = Time.fixedDeltaTime;

        foreach (Bullet bullet in _bulletPool.InUseObjects)
            bullet.Physics.UpdatePosition(deltaTime);

        foreach (Asteroid asteroid in _asteroidPool.InUseObjects)
            asteroid.Physics.UpdatePosition(deltaTime);

        foreach (Ufo ufo in _ufoPool.InUseObjects)
        {
            ufo.Chase(_ship.Physics.Position, _ufoChaseSpeed, deltaTime);
            ufo.Physics.UpdatePosition(deltaTime);
        }

        foreach (Bullet bullet in _bulletPool.InUseObjects)
        {
            foreach (Asteroid asteroid in _asteroidPool.InUseObjects)
            {
                if (_collisionDetector.CheckCollision(bullet.Physics, asteroid.Physics))
                {
                    asteroidsToHit.Add(asteroid);
                    bulletsToReturn.Add(bullet);
                }
            }

            foreach (Ufo ufo in _ufoPool.InUseObjects)
            {
                if (_collisionDetector.CheckCollision(bullet.Physics, ufo.Physics))
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
            ApplyReward(asteroid);
            asteroid.TakeHit();
        }

        foreach (Ufo ufo in ufosToHit)
        {
            ApplyReward(ufo);
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
        Vector2 direction = DirectionMath.FromAngle(_ship.Rotation);
        Vector2 start = _ship.Physics.Position;
        Vector2 end = start + direction * LaserRange;

        List<Asteroid> asteroidsToHit = new List<Asteroid>();
        foreach (Asteroid asteroid in _asteroidPool.InUseObjects)
        {
            if (_collisionDetector.CheckLaserHit(start, end, asteroid.Physics))
                asteroidsToHit.Add(asteroid);
        }

        List<Ufo> ufosToHit = new List<Ufo>();
        foreach (Ufo ufo in _ufoPool.InUseObjects)
        {
            if (_collisionDetector.CheckLaserHit(start, end, ufo.Physics))
                ufosToHit.Add(ufo);
        }

        foreach (Asteroid asteroid in asteroidsToHit)
        {
            ApplyReward(asteroid);
            asteroid.TakeHit();
        }

        foreach (Ufo ufo in ufosToHit)
        {
            ApplyReward(ufo);
            ufo.TakeHit();
        }
    }

    private void ApplyReward(IRewardable rewardable)
    {
        int reward = _rewardService.GetReward(rewardable.Type);
        _score.AddScore(reward);
    }
}