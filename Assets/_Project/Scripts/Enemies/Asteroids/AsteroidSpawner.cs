using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class AsteroidSpawner
{
    private const float MinSpawnDelay = 1f;
    private const float MaxSpawnDelay = 5f;
    private const int FragmentsPerSplit = 2;

    private readonly ObjectPool<Asteroid> _asteroidPool;
    private readonly WorldBoundary _boundary;
    private readonly float _asteroidSpeed;
    private readonly AsteroidFactory _asteroidFactory;
    private readonly EnemyCounterTracker _enemyCounter;
    private readonly SignalBus _signalBus;

    private bool _isGameOver;

    private float _smallerFragmentSpeed = 1.5f;

    public ObjectPool<Asteroid> AsteroidPool => _asteroidPool;

    public AsteroidSpawner(AsteroidFactory asteroidFactory, WorldBoundary boundary, float asteroidSpeed, EnemyCounterTracker enemyCounter, SignalBus signalBus)
    {
        _asteroidPool = new ObjectPool<Asteroid>(() => asteroidFactory.Create(AsteroidSize.Large));
        _boundary = boundary;
        _asteroidSpeed = asteroidSpeed;
        _asteroidFactory = asteroidFactory;
        _enemyCounter = enemyCounter;
        _signalBus = signalBus;
        _signalBus.Subscribe<GameOverSignal>(HandleGameOver);
    }

    public void StartSpawning()
    {
        SpawnLoop().Forget();
    }

    private async UniTaskVoid SpawnLoop()
    {
        while (true)
        {
            if (_isGameOver)
                break;

            float spawnRate = UnityEngine.Random.Range(MinSpawnDelay, MaxSpawnDelay);
            await UniTask.Delay(TimeSpan.FromSeconds(spawnRate));

            if (_isGameOver)
                break;

            if (!_enemyCounter.CanSpawn())
                continue;

            Asteroid asteroid = _asteroidPool.Get();
            asteroid.OnDestroyed += HandleAsteroidDestroyed;
            Vector2 position = _boundary.GetRandomPositionOutside();
            asteroid.Spawn(position, _asteroidSpeed);
            _enemyCounter.RegisterSpawned();
        }
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
            SpawnFragments(position, AsteroidSize.Medium, FragmentsPerSplit);
        else if (size == AsteroidSize.Medium)
            SpawnFragments(position, AsteroidSize.Small, FragmentsPerSplit);
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
            fragment.Spawn(position, fragmentSpeed);
        }
    }

    private void HandleGameOver()
    {
        _isGameOver = true;
    }
}