using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class UfoSpawner
{
    private const float MinSpawnDelay = 1f;
    private const float MaxSpawnDelay = 5f;

    private readonly IShipInfo _shipInfo;
    private readonly ObjectPool<Ufo> _ufoPool;
    private readonly WorldBoundary _boundary;
    private readonly float _ufoSpeed;
    private readonly UfoFactory _ufoFactory;
    private readonly EnemyCounterTracker _counterTracker;
    private readonly SignalBus _signalBus;
    private bool _isGameOver;

    public ObjectPool<Ufo> UfoPool => _ufoPool;

    public UfoSpawner(UfoFactory ufoFactory, WorldBoundary boundary, float ufoSpeed, IShipInfo shipInfo, EnemyCounterTracker counterTracker, SignalBus signalBus)
    {
        _ufoPool = new ObjectPool<Ufo>(() => ufoFactory.CreateUfo(_shipInfo));
        _boundary = boundary;
        _ufoSpeed = ufoSpeed;
        _ufoFactory = ufoFactory;
        _shipInfo = shipInfo;
        _counterTracker = counterTracker;
        _signalBus = signalBus;
        _signalBus.Subscribe<GameOverSignal>(HandleGameOver);
    }

    private void HandleGameOver()
    {
        _isGameOver = true;
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

            float spawnRate = Random.Range(MinSpawnDelay, MaxSpawnDelay);
            await UniTask.Delay(TimeSpan.FromSeconds(spawnRate));

            if (_isGameOver)
                break;

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
}