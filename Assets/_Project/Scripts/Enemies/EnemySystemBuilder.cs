using Zenject;

public class EnemySystemBuilder
{
    private readonly AsteroidFactory _asteroidFactory;
    private readonly GameConfigProvider _configProvider;
    private readonly SignalBus _signalBus;
    private readonly UfoFactory _ufoFactory;

    public EnemySystemBuilder(AsteroidFactory asteroidFactory, UfoFactory ufoFactory, GameConfigProvider configProvider,
        SignalBus signalBus)
    {
        _asteroidFactory = asteroidFactory;
        _ufoFactory = ufoFactory;
        _configProvider = configProvider;
        _signalBus = signalBus;
    }

    public AsteroidSpawner BuildAsteroidSpawner(WorldBoundary worldBoundary, EnemyCounterTracker enemyCounterTracker)
    {
        var spawner = new AsteroidSpawner(_asteroidFactory, worldBoundary,
            _configProvider.Enemy.AsteroidSpeed, enemyCounterTracker, _signalBus, _configProvider.Enemy.MinSpawnDelay,
            _configProvider.Enemy.MaxSpawnDelay);


        var splitter = new AsteroidSplitter(_asteroidFactory, spawner.AsteroidPool,
            _configProvider.Enemy.FragmentsPerSplit, _configProvider.Enemy.SmallerFragmentSpeedMultiplier);
        spawner.SetSplitter(splitter);
        spawner.StartSpawning();
        return spawner;
    }

    public UfoSpawner BuildUfoSpawner(WorldBoundary worldBoundary, EnemyCounterTracker enemyCounterTracker)
    {
        var spawner = new UfoSpawner(_ufoFactory, worldBoundary, enemyCounterTracker, _signalBus);
        spawner.StartSpawning();
        return spawner;
    }
}