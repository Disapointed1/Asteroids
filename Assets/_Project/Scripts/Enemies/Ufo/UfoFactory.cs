using Zenject;

public class UfoFactory
{
    private readonly DiContainer _container;
    private readonly float _mass;
    private readonly float _radius;
    private readonly UfoView _ufoViewPrefab;

    public UfoFactory(DiContainer container, UfoView ufoViewPrefab, float radius, float mass)
    {
        _container = container;
        _ufoViewPrefab = ufoViewPrefab;
        _radius = radius;
        _mass = mass;
    }

    public Ufo CreateUfo()
    {
        var ufo = new Ufo(_radius, _mass);
        var ufoView = _container.InstantiatePrefabForComponent<UfoView>(_ufoViewPrefab);
        ufoView.Initialize(ufo);
        return ufo;
    }
}