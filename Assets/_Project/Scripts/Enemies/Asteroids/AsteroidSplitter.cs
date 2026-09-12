using System;
using UnityEngine;

public class AsteroidSplitter
{
    private const int FragmentPerSplit = 2;
    private const float SmallerFragmentSpeedMultiplier = 1.5f;

    private readonly AsteroidFactory _asteroidFactory;
    private readonly ObjectPool<Asteroid> _asteroidPool;

    public AsteroidSplitter(AsteroidFactory asteroidFactory, ObjectPool<Asteroid> asteroidPool)
    {
        _asteroidFactory = asteroidFactory;
        _asteroidPool = asteroidPool;
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
        for (int i = 0; i < FragmentPerSplit; i++)
        {
            Asteroid fragment = _asteroidFactory.Create(size);
            fragment.MarkAsFragment();
            _asteroidPool.Register(fragment);
            fragment.OnDestroyed += onFragmentDestroyed;
            float fragmentSpeed = baseSpeed * SmallerFragmentSpeedMultiplier;
            fragment.Spawn(position, fragmentSpeed);
        }
    }
}