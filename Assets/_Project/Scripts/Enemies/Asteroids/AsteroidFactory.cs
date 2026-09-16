using System.Collections.Generic;
using Zenject;

public class AsteroidFactory
{
    private readonly AsteroidView _asteroidViewPrefab;
    private readonly DiContainer _container;
    private readonly float _largeRadius;
    private readonly float _mediumRadius;
    private readonly float _smallRadius;

    private readonly ObjectPool<Asteroid> _largePool;
    private readonly ObjectPool<Asteroid> _mediumPool;
    private readonly ObjectPool<Asteroid> _smallPool;

    public AsteroidFactory(DiContainer container, AsteroidView asteroidViewPrefab, float largeRadius,
        float mediumRadius, float smallRadius)
    {
        _container = container;
        _asteroidViewPrefab = asteroidViewPrefab;
        _largeRadius = largeRadius;
        _mediumRadius = mediumRadius;
        _smallRadius = smallRadius;

        _largePool = new ObjectPool<Asteroid>(() => CreateNew(AsteroidSize.Large));
        _mediumPool = new ObjectPool<Asteroid>(() => CreateNew(AsteroidSize.Medium));
        _smallPool = new ObjectPool<Asteroid>(() => CreateNew(AsteroidSize.Small));
    }

    public Asteroid Get(AsteroidSize size)
    {
        return GetPool(size).Get();
    }

    public void Return(Asteroid asteroid)
    {
        GetPool(asteroid.Size).Return(asteroid);
    }

    public void Register(Asteroid asteroid)
    {
        GetPool(asteroid.Size).Register(asteroid);
    }

    public IEnumerable<Asteroid> GetAllInUseObjects()
    {
        foreach (var asteroid in _largePool.InUseObjects) yield return asteroid;
        foreach (var asteroid in _mediumPool.InUseObjects) yield return asteroid;
        foreach (var asteroid in _smallPool.InUseObjects) yield return asteroid;
    }

    public Asteroid CreateNew(AsteroidSize size)
    {
        var radius = GetRadiusForSize(size);
        var asteroid = new Asteroid(radius, size);
        var asteroidView = _container.InstantiatePrefabForComponent<AsteroidView>(_asteroidViewPrefab);
        asteroidView.Initialize(asteroid);
        return asteroid;
    }

    private ObjectPool<Asteroid> GetPool(AsteroidSize size)
    {
        return size switch
        {
            AsteroidSize.Large => _largePool,
            AsteroidSize.Medium => _mediumPool,
            AsteroidSize.Small => _smallPool,
            _ => _largePool
        };
    }

    private float GetRadiusForSize(AsteroidSize size)
    {
        return size switch
        {
            AsteroidSize.Large => _largeRadius,
            AsteroidSize.Medium => _mediumRadius,
            AsteroidSize.Small => _smallRadius,
            _ => _largeRadius
        };
    }
}