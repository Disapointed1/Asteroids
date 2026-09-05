using Zenject;

public class AsteroidFactory
{
    private readonly DiContainer _container;
    private readonly AsteroidView _asteroidViewPrefab;
    private readonly float _largeRadius;
    private readonly float _mediumRadius;
    private readonly float _smallRadius;

    public AsteroidFactory(DiContainer container, AsteroidView asteroidViewPrefab, float largeRadius, float mediumRadius, float smallRadius)
    {
        _container = container;
        _asteroidViewPrefab = asteroidViewPrefab;
        _largeRadius = largeRadius;
        _mediumRadius = mediumRadius;
        _smallRadius = smallRadius;
    }

    public Asteroid Create(AsteroidSize size)
    {
        float radius = GetRadiusForSize(size);
        Asteroid asteroid = new Asteroid(radius, size);
        AsteroidView asteroidView = _container.InstantiatePrefabForComponent<AsteroidView>(_asteroidViewPrefab);
        asteroidView.Initialize(asteroid);
        asteroid.SetView(asteroidView.gameObject);
        return asteroid;
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