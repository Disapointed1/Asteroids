using System.Collections.Generic;
using UnityEngine;

public class BulletCollisionHandler : ICollisionHandler
{
    private readonly ObjectPool<Bullet> _bulletPool;
    private readonly AsteroidFactory _asteroidFactory;
    private readonly ObjectPool<Ufo> _ufoPool;
    private readonly CollisionDetector _collisionDetector;
    private readonly RewardService _rewardService;
    private readonly GameScore _score;

    private readonly List<Bullet> _bulletsToReturn = new List<Bullet>();
    private readonly List<Asteroid> _asteroidsToHit = new List<Asteroid>();
    private readonly List<Ufo> _ufosToHit = new List<Ufo>();

    public BulletCollisionHandler(ObjectPool<Bullet> bulletPool, AsteroidFactory asteroidFactory,
        ObjectPool<Ufo> ufoPool, CollisionDetector collisionDetector, RewardService rewardService, GameScore score)
    {
        _bulletPool = bulletPool;
        _asteroidFactory = asteroidFactory;
        _ufoPool = ufoPool;
        _collisionDetector = collisionDetector;
        _rewardService = rewardService;
        _score = score;
    }

    public void CheckCollisions()
    {
        _bulletsToReturn.Clear();
        _asteroidsToHit.Clear();
        _ufosToHit.Clear();

        foreach (var bullet in _bulletPool.InUseObjects)
        {
            foreach (var asteroid in _asteroidFactory.GetAllInUseObjects())
            {
                if (_collisionDetector.CheckCollision(bullet.Physics, asteroid.Physics))
                {
                    _asteroidsToHit.Add(asteroid);
                    _bulletsToReturn.Add(bullet);
                }
            }

            foreach (var ufo in _ufoPool.InUseObjects)
            {
                if (_collisionDetector.CheckCollision(bullet.Physics, ufo.Physics))
                {
                    _ufosToHit.Add(ufo);
                    _bulletsToReturn.Add(bullet);
                }
            }
        }

        foreach (var bullet in _bulletsToReturn)
        {
            _bulletPool.Return(bullet);
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