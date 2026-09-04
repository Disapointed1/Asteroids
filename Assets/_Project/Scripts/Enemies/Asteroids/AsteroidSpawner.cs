using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class AsteroidSpawner
{
    private readonly ObjectPool<Asteroid> _asteroidPool;
    private readonly WorldBoundary _boundary;
    private readonly float _asteroidSpeed;
    private readonly AsteroidFactory _asteroidFactory;
    private readonly EnemyCounterTracker _enemyCounter;

    private float _smallerFragmentSpeed = 1.5f;

    public ObjectPool<Asteroid> AsteroidPool => _asteroidPool;


    public AsteroidSpawner(AsteroidFactory asteroidFactory,WorldBoundary boundary, float asteroidSpeed, EnemyCounterTracker enemyCounter)
    {
        _asteroidPool = new ObjectPool<Asteroid>(()=> asteroidFactory.Create(AsteroidSize.Large));
        _boundary = boundary;
        _asteroidSpeed = asteroidSpeed;
        _asteroidFactory = asteroidFactory;
        _enemyCounter = enemyCounter;
    }

    public void StartSpawning()
    {
        SpawnLoop().Forget();
    }

    private async UniTaskVoid SpawnLoop()
    {
        while (true)
        {
            float spawnRate = UnityEngine.Random.Range(1f, 5f);
            await UniTask.Delay(TimeSpan.FromSeconds(spawnRate));

            if(!_enemyCounter.CanSpawn())
                continue;

            Asteroid asteroid = _asteroidPool.Get();
            asteroid.OnDestroyed += HandleAsteroidDestroyed;
            Vector2 position = GetRandomSpawnPosition();
            asteroid.Spawn(position,_asteroidSpeed);
            _enemyCounter.RegisterSpawned();
        }

    }

    private Vector2 GetRandomSpawnPosition()
    {
        int side = UnityEngine.Random.Range(0, 4);
        float x, y;

        switch (side)
        {
            case 0:
                x  = UnityEngine.Random.Range(-_boundary.HalfWidth, _boundary.HalfWidth);
                y = _boundary.HalfHeight + 1f;
                break;
            case 1:
                x = UnityEngine.Random.Range(-_boundary.HalfWidth, _boundary.HalfWidth);
                y = -_boundary.HalfHeight - 1f;
                break;
            case 2:
                x = -_boundary.HalfWidth - 1f;
                y = UnityEngine.Random.Range(-_boundary.HalfHeight, _boundary.HalfHeight);
                break;
            default:
                x = _boundary.HalfWidth + 1f;
                y = UnityEngine.Random.Range(-_boundary.HalfHeight, _boundary.HalfHeight);
                break;
        }
        return new Vector2(x, y);
    }

    private void HandleAsteroidDestroyed(Asteroid asteroid)
    {
        asteroid.OnDestroyed -= HandleAsteroidDestroyed;
        Vector2 position = asteroid.Physics.Position;
        AsteroidSize size = asteroid.Size;
        bool wasFragment = asteroid.IsFragment;
        _asteroidPool.Return(asteroid);

        if (!wasFragment)
            _enemyCounter.RegisterDestroyed();

        if (size == AsteroidSize.Large)
        {
            SpawnFragments(position, AsteroidSize.Medium, 2);
        }
        else if (size == AsteroidSize.Medium)
        {
            SpawnFragments(position, AsteroidSize.Small, 2);
        }
    }

    private void SpawnFragments(Vector2 position, AsteroidSize size, int count)
    {
        for (int i = 0; i < count; i++)
        {
            Asteroid fragment = _asteroidFactory.Create(size);
            fragment.MarkAsFragment();
            _asteroidPool.Register(fragment);
            fragment.OnDestroyed += HandleAsteroidDestroyed;
            float fragmentSpeed = _asteroidSpeed * _smallerFragmentSpeed;
            fragment.Spawn(position,  fragmentSpeed);
        }
    }

}
