using System;
using System.Collections.Generic;

public class LaserCollisionHandler : IDisposable
{
    private const float LaserRange = 20f;

    private readonly Ship _ship;
    private readonly AsteroidFactory _asteroidFactory;
    private readonly ObjectPool<Ufo> _ufoPool;
    private readonly CollisionDetector _collisionDetector;
    private readonly LaserWeapon _laserWeapon;
    private readonly RewardService _rewardService;
    private readonly GameScore _score;

    private readonly List<Asteroid> _asteroidsToHit = new List<Asteroid>();
    private readonly List<Ufo> _ufosToHit = new List<Ufo>();

    public LaserCollisionHandler(Ship ship, AsteroidFactory asteroidFactory, ObjectPool<Ufo> ufoPool,
        CollisionDetector collisionDetector, LaserWeapon laserWeapon, RewardService rewardService, GameScore score)
    {
        _ship = ship;
        _asteroidFactory = asteroidFactory;
        _ufoPool = ufoPool;
        _collisionDetector = collisionDetector;
        _rewardService = rewardService;
        _score = score;
        _laserWeapon = laserWeapon;
        _laserWeapon.OnFired += HandleLaserFired;
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

        _asteroidsToHit.Clear();
        _ufosToHit.Clear();

        foreach (var asteroid in _asteroidFactory.GetAllInUseObjects())
        {
            if (_collisionDetector.CheckLaserHit(start, end, asteroid.Physics))
            {
                _asteroidsToHit.Add(asteroid);
            }
        }

        foreach (var ufo in _ufoPool.InUseObjects)
        {
            if (_collisionDetector.CheckLaserHit(start, end, ufo.Physics))
            {
                _ufosToHit.Add(ufo);
            }
        }

        DestroyEnemies(_asteroidsToHit);
        DestroyEnemies(_ufosToHit);
    }

    private void DestroyEnemies<T>(List<T> enemiesToHit) where T : IEnemy
    {
        foreach (var enemy in enemiesToHit)
        {
            var reward = _rewardService.GetReward(enemy.Type);
            _score.AddScore(reward);
            enemy.TakeHit();
        }
    }
}