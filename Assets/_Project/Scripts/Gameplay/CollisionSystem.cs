using System.Collections.Generic;
using UnityEngine;

public class CollisionSystem
{
    private const float LaserRange = 20f;
    private readonly ObjectPool<Asteroid> _asteroidPool;
    private readonly List<Asteroid> _asteroidsToHit = new();
    private readonly ObjectPool<Bullet> _bulletPool;
    private readonly List<Bullet> _bulletsToReturn = new();
    private readonly CollisionDetector _collisionDetector;
    private readonly CollisionResolver _collisionResolver;
    private readonly List<Asteroid> _laserAsteroidsToHit = new();
    private readonly List<Ufo> _laserUfosToHit = new();
    private readonly LaserWeapon _laserWeapon;
    private readonly RewardService _rewardService;
    private readonly GameScore _score;

    private readonly Ship _ship;
    private readonly float _ufoChaseSpeed;
    private readonly ObjectPool<Ufo> _ufoPool;
    private readonly List<Ufo> _ufosToHit = new();

    public CollisionSystem(Ship ship, ObjectPool<Bullet> bulletPool, ObjectPool<Asteroid> asteroidPool,
        ObjectPool<Ufo> ufoPool, GameScore score, RewardService rewardService, float ufoChaseSpeed,
        LaserWeapon laserWeapon,
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
        _bulletsToReturn.Clear();
        _asteroidsToHit.Clear();
        _ufosToHit.Clear();

        var deltaTime = Time.fixedDeltaTime;

        foreach (var bullet in _bulletPool.InUseObjects)
            bullet.Physics.UpdatePosition(deltaTime);

        foreach (var asteroid in _asteroidPool.InUseObjects)
            asteroid.Physics.UpdatePosition(deltaTime);

        foreach (var ufo in _ufoPool.InUseObjects)
        {
            ufo.Chase(_ship.Physics.Position, _ufoChaseSpeed, deltaTime);
            ufo.Physics.UpdatePosition(deltaTime);
        }

        foreach (var bullet in _bulletPool.InUseObjects)
        {
            foreach (var asteroid in _asteroidPool.InUseObjects)
                if (_collisionDetector.CheckCollision(bullet.Physics, asteroid.Physics))
                {
                    _asteroidsToHit.Add(asteroid);
                    _bulletsToReturn.Add(bullet);
                }

            foreach (var ufo in _ufoPool.InUseObjects)
                if (_collisionDetector.CheckCollision(bullet.Physics, ufo.Physics))
                {
                    _ufosToHit.Add(ufo);
                    _bulletsToReturn.Add(bullet);
                }
        }

        foreach (var bullet in _bulletsToReturn) _bulletPool.Return(bullet);

        DestroyEnemies(_asteroidsToHit);
        DestroyEnemies(_ufosToHit);

        foreach (var asteroid in _asteroidPool.InUseObjects)
            if (!_ship.IsInvulnerable && _collisionDetector.CheckCollision(_ship.Physics, asteroid.Physics))
            {
                _collisionResolver.ResolveCollision(_ship.Physics, asteroid.Physics);
                _ship.TakeDamage(1);
            }

        foreach (var ufo in _ufoPool.InUseObjects)
            if (!_ship.IsInvulnerable && _collisionDetector.CheckCollision(_ship.Physics, ufo.Physics))
            {
                _collisionResolver.ResolveCollision(_ship.Physics, ufo.Physics);
                _ship.TakeDamage(1);
            }
    }

    public void Dispose()
    {
        _laserWeapon.OnFired -= HandleLaserFired;
    }

    private void HandleLaserFired()
    {
        var direction = DirectionMath.FromAngle(_ship.Rotation);
        var start = _ship.Physics.Position;
        var end = start + direction * LaserRange;

        _laserAsteroidsToHit.Clear();
        _laserUfosToHit.Clear();

        foreach (var asteroid in _asteroidPool.InUseObjects)
            if (_collisionDetector.CheckLaserHit(start, end, asteroid.Physics))
                _laserAsteroidsToHit.Add(asteroid);

        foreach (var ufo in _ufoPool.InUseObjects)
            if (_collisionDetector.CheckLaserHit(start, end, ufo.Physics))
                _laserUfosToHit.Add(ufo);

        DestroyEnemies(_laserAsteroidsToHit);
        DestroyEnemies(_laserUfosToHit);
    }

    private void ApplyReward(IRewardable rewardable)
    {
        var reward = _rewardService.GetReward(rewardable.Type);
        _score.AddScore(reward);
    }

    private void DestroyEnemies<T>(List<T> enemiesToHit) where T : IEnemy
    {
        foreach (var enemy in enemiesToHit)
        {
            ApplyReward(enemy);
            enemy.TakeHit();
        }
    }
}