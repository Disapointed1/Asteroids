using Zenject;

public class AsteroidFactory
{
    private readonly DiContainer _container;
    private readonly AsteroidView _asteroidViewPrefab;


    public AsteroidFactory(DiContainer container,  AsteroidView asteroidViewPrefab)
    {
        _container = container;
        _asteroidViewPrefab = asteroidViewPrefab;
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
            AsteroidSize.Large => 1f,
            AsteroidSize.Medium => 0.6f,
            AsteroidSize.Small => 0.3f,
            _ => 0.5f
        };
    }
}
