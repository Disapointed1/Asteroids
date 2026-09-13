using System;
using UnityEngine;

public class AsteroidSplitter
{
    private readonly AsteroidFactory _asteroidFactory;
    private readonly ObjectPool<Asteroid> _asteroidPool;
    private readonly int _fragmentPerSplit;
    private readonly float _smallerFragmentSpeedMultiplier;

    public AsteroidSplitter(AsteroidFactory asteroidFactory, ObjectPool<Asteroid> asteroidPool, int fragmentPerSplit,
        float smallerFragmentSpeedMultiplier)
    {
        _asteroidFactory = asteroidFactory;
        _asteroidPool = asteroidPool;
        _fragmentPerSplit = fragmentPerSplit;
        _smallerFragmentSpeedMultiplier = smallerFragmentSpeedMultiplier;
    }

    public void Split(Asteroid destroyedAsteroid, Vector2 position, float baseSpeed,
        Action<Asteroid> onFragmentDestroyed)
    {
        if (destroyedAsteroid.Size == AsteroidSize.Large)
            SpawnFragments(position, AsteroidSize.Medium, baseSpeed, onFragmentDestroyed);
        else if (destroyedAsteroid.Size == AsteroidSize.Medium)
            SpawnFragments(position, AsteroidSize.Small, baseSpeed, onFragmentDestroyed);
    }

    private void SpawnFragments(Vector2 position, AsteroidSize size, float baseSpeed,
        Action<Asteroid> onFragmentDestroyed)
    {
        for (var i = 0; i < _fragmentPerSplit; i++)
        {
            var fragment = _asteroidFactory.Create(size);
            fragment.MarkAsFragment();
            _asteroidPool.Register(fragment);
            fragment.OnDestroyed += onFragmentDestroyed;
            var fragmentSpeed = baseSpeed * _smallerFragmentSpeedMultiplier;
            fragment.Spawn(position, fragmentSpeed);
        }
    }
}