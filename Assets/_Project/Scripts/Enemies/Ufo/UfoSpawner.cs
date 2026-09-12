using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class UfoSpawner
{
    private const float MinSpawnDelay = 1f;
    private const float MaxSpawnDelay = 5f;

    private readonly ObjectPool<Ufo> _ufoPool;
    private readonly WorldBoundary _boundary;
    private readonly EnemyCounterTracker _counterTracker;
    private readonly SignalBus _signalBus;

    private CancellationTokenSource _cts =  new CancellationTokenSource();

    public ObjectPool<Ufo> UfoPool => _ufoPool;

    public UfoSpawner(UfoFactory ufoFactory, WorldBoundary boundary,
        EnemyCounterTracker counterTracker, SignalBus signalBus)
    {
        _ufoPool = new ObjectPool<Ufo>( ufoFactory.CreateUfo);
        _boundary = boundary;
        _counterTracker = counterTracker;
        _signalBus = signalBus;
        _signalBus.Subscribe<GameOverSignal>(HandleGameOver);
    }
    public void StartSpawning()
    {
        SpawnLoop(_cts.Token).Forget();
    }

    private async UniTask SpawnLoop(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {


            float spawnRate = Random.Range(MinSpawnDelay, MaxSpawnDelay);
            await UniTask.Delay(TimeSpan.FromSeconds(spawnRate), cancellationToken: token);


            if (!_counterTracker.CanSpawn())
                continue;

            Ufo ufo = _ufoPool.Get();
            ufo.OnDestroyed += HandleUfoDestroyed;
            Vector2 position = _boundary.GetRandomPositionOutside();
            ufo.SetPosition(position);
            _counterTracker.RegisterSpawned();
        }
    }

    private void HandleUfoDestroyed(Ufo ufo)
    {
        ufo.OnDestroyed -= HandleUfoDestroyed;
        _ufoPool.Return(ufo);
        _counterTracker.RegisterDestroyed();
    }

    private void HandleGameOver()
    {
        _cts.Cancel();
    }
}