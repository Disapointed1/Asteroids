using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Zenject;
using Random = UnityEngine.Random;

public class AsteroidSpawner
{
    private readonly float _asteroidSpeed;
    private readonly WorldBoundary _boundary;

    private readonly CancellationTokenSource _cts = new();
    private readonly EnemyCounterTracker _enemyCounter;
    private readonly float _maxSpawnDelay;
    private readonly float _minSpawnDelay;
    private readonly SignalBus _signalBus;
    private AsteroidSplitter _asteroidSplitter;
    private AsteroidFactory _asteroidFactory;

    public AsteroidSpawner(AsteroidFactory asteroidFactory, WorldBoundary boundary, float asteroidSpeed,
        EnemyCounterTracker enemyCounter, SignalBus signalBus, float minSpawnDelay, float maxSpawnDelay)
    {
        _boundary = boundary;
        _asteroidSpeed = asteroidSpeed;
        _enemyCounter = enemyCounter;
        _signalBus = signalBus;
        _signalBus.Subscribe<GameOverSignal>(HandleGameOver);
        _minSpawnDelay = minSpawnDelay;
        _maxSpawnDelay = maxSpawnDelay;
        _asteroidFactory = asteroidFactory;
    }


    public void StartSpawning()
    {
        SpawnLoop(_cts.Token).Forget();
    }

    public void SetSplitter(AsteroidSplitter splitter)
    {
        _asteroidSplitter = splitter;
    }

    private async UniTask SpawnLoop(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            var spawnRate = Random.Range(_minSpawnDelay, _maxSpawnDelay);
            await UniTask.Delay(TimeSpan.FromSeconds(spawnRate), cancellationToken: token);

            if (!_enemyCounter.CanSpawn())
                continue;

            var asteroid = _asteroidFactory.Get(AsteroidSize.Large);
            asteroid.OnDestroyed += HandleAsteroidDestroyed;
            var position = _boundary.GetRandomPositionOutside();
            asteroid.Spawn(position, _asteroidSpeed);
            _enemyCounter.RegisterSpawned();
        }
    }

    private void HandleAsteroidDestroyed(Asteroid asteroid)
    {
        asteroid.OnDestroyed -= HandleAsteroidDestroyed;
        var position = asteroid.Physics.Position;
        var wasFragment = asteroid.IsFragment;
        _asteroidFactory.Return(asteroid);

        if (!wasFragment)
            _enemyCounter.RegisterDestroyed();

        _asteroidSplitter.Split(asteroid, position, _asteroidSpeed, HandleAsteroidDestroyed);
    }


    private void HandleGameOver()
    {
        _signalBus.Unsubscribe<GameOverSignal>(HandleGameOver);
        _cts.Cancel();
    }
}