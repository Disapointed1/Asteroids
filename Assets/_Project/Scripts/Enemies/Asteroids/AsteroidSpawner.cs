using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class AsteroidSpawner
{
    private const float MinSpawnDelay = 1f;
    private const float MaxSpawnDelay = 5f;

    private readonly ObjectPool<Asteroid> _asteroidPool;
    private readonly WorldBoundary _boundary;
    private readonly float _asteroidSpeed;
    private readonly EnemyCounterTracker _enemyCounter;
    private readonly SignalBus _signalBus;
    private AsteroidSplitter _asteroidSplitter;

    private CancellationTokenSource _cts = new CancellationTokenSource();

    public ObjectPool<Asteroid> AsteroidPool => _asteroidPool;

    public AsteroidSpawner(AsteroidFactory asteroidFactory, WorldBoundary boundary, float asteroidSpeed,
        EnemyCounterTracker enemyCounter, SignalBus signalBus)
    {
        _asteroidPool = new ObjectPool<Asteroid>(() => asteroidFactory.Create(AsteroidSize.Large));
        _boundary = boundary;
        _asteroidSpeed = asteroidSpeed;
        _enemyCounter = enemyCounter;
        _signalBus = signalBus;
        _signalBus.Subscribe<GameOverSignal>(HandleGameOver);
    }

    public void StartSpawning()
    {
        SpawnLoop(_cts.Token).Forget();
    }
    public void SetSplitter(AsteroidSplitter splitter)
    {
        _asteroidSplitter =  splitter;
    }

    private async UniTask SpawnLoop(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            float spawnRate = UnityEngine.Random.Range(MinSpawnDelay, MaxSpawnDelay);
            await UniTask.Delay(TimeSpan.FromSeconds(spawnRate), cancellationToken: token);

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
        bool wasFragment = asteroid.IsFragment;
        _asteroidPool.Return(asteroid);

        if (!wasFragment)
            _enemyCounter.RegisterDestroyed();

        _asteroidSplitter.Split(asteroid, position, _asteroidSpeed, HandleAsteroidDestroyed);
    }


    private void HandleGameOver()
    {
        _cts.Cancel();
    }
}