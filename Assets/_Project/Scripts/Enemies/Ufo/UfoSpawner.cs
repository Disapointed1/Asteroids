using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Zenject;
using Random = UnityEngine.Random;

public class UfoSpawner
{
    private const float MinSpawnDelay = 1f;
    private const float MaxSpawnDelay = 5f;
    private readonly WorldBoundary _boundary;
    private readonly EnemyCounterTracker _counterTracker;

    private readonly CancellationTokenSource _cts = new();
    private readonly SignalBus _signalBus;

    public UfoSpawner(UfoFactory ufoFactory, WorldBoundary boundary,
        EnemyCounterTracker counterTracker, SignalBus signalBus)
    {
        UfoPool = new ObjectPool<Ufo>(ufoFactory.CreateUfo);
        _boundary = boundary;
        _counterTracker = counterTracker;
        _signalBus = signalBus;
        _signalBus.Subscribe<GameOverSignal>(HandleGameOver);
    }

    public ObjectPool<Ufo> UfoPool { get; }

    public void StartSpawning()
    {
        SpawnLoop(_cts.Token).Forget();
    }

    private async UniTask SpawnLoop(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            var spawnRate = Random.Range(MinSpawnDelay, MaxSpawnDelay);
            await UniTask.Delay(TimeSpan.FromSeconds(spawnRate), cancellationToken: token);


            if (!_counterTracker.CanSpawn())
                continue;

            var ufo = UfoPool.Get();
            ufo.OnDestroyed += HandleUfoDestroyed;
            var position = _boundary.GetRandomPositionOutside();
            ufo.SetPosition(position);
            _counterTracker.RegisterSpawned();
        }
    }

    private void HandleUfoDestroyed(Ufo ufo)
    {
        ufo.OnDestroyed -= HandleUfoDestroyed;
        UfoPool.Return(ufo);
        _counterTracker.RegisterDestroyed();
    }

    private void HandleGameOver()
    {
        _cts.Cancel();
    }
}