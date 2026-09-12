using Zenject;

public class EnemySystemBuilder
{
    private readonly AsteroidFactory _asteroidFactory;
    private readonly UfoFactory _ufoFactory;
    private readonly GameConfigFacade _configFacade;
    private readonly SignalBus _signalBus;

    public EnemySystemBuilder(AsteroidFactory asteroidFactory, UfoFactory ufoFactory, GameConfigFacade configFacade,
        SignalBus signalBus)
    {
        _asteroidFactory = asteroidFactory;
        _ufoFactory = ufoFactory;
        _configFacade = configFacade;
        _signalBus = signalBus;
    }

    public AsteroidSpawner BuildAsteroidSpawner(WorldBoundary worldBoundary, EnemyCounterTracker enemyCounterTracker)
    {
        AsteroidSpawner spawner = new AsteroidSpawner(_asteroidFactory, worldBoundary,
            _configFacade.Enemy.AsteroidSpeed, enemyCounterTracker, _signalBus);
        AsteroidSplitter splitter = new AsteroidSplitter(_asteroidFactory, spawner.AsteroidPool);
        spawner.SetSplitter(splitter);
        spawner.StartSpawning();
        return spawner;
    }

    public UfoSpawner BuildUfoSpawner(WorldBoundary worldBoundary, EnemyCounterTracker enemyCounterTracker)
    {
        UfoSpawner spawner = new UfoSpawner(_ufoFactory, worldBoundary, enemyCounterTracker, _signalBus);
        spawner.StartSpawning();
        return spawner;
    }
}
