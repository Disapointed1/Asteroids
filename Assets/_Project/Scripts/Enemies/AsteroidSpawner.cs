using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class AsteroidSpawner
{
    private readonly ObjectPool<Asteroid> _asteroidPool;
    private readonly WorldBoundary _boundary;
    private readonly float _asteroidSpeed;

    public ObjectPool<Asteroid> AsteroidPool => _asteroidPool;


    public AsteroidSpawner(AsteroidFactory asteroidFactory,WorldBoundary boundary, float asteroidSpeed)
    {
        _asteroidPool = new ObjectPool<Asteroid>(()=> asteroidFactory.Create(AsteroidSize.Large));
        _boundary = boundary;
        _asteroidSpeed = asteroidSpeed;
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

            Asteroid asteroid = _asteroidPool.Get();
            Vector2 position = GetRandomSpawnPosition();
            asteroid.Spawn(position,_asteroidSpeed);
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

}
