using Zenject;

public class UfoFactory
{
    private readonly DiContainer _container;
    private readonly UfoView _ufoViewPrefab;
    private readonly float _radius;
    private readonly float _mass;

    public UfoFactory(DiContainer container, UfoView ufoViewPrefab,  float radius, float mass)
    {
        _container = container;
        _ufoViewPrefab = ufoViewPrefab;
        _radius = radius;
        _mass = mass;

    }

    public Ufo CreateUfo(IShipInfo shipInfo)
    {
        Ufo ufo = new Ufo(_radius,  _mass);

        UfoView ufoView =  _container.InstantiatePrefabForComponent<UfoView>(_ufoViewPrefab);
        ufoView.Initialize(ufo, shipInfo);
        ufo.SetView(ufoView.gameObject);
        return ufo;
    }


}
