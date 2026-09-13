using Zenject;

public class BulletFactory
{
    private readonly float _bulletRadius;
    private readonly BulletView _bulletViewPrefab;
    private readonly DiContainer _container;

    public BulletFactory(DiContainer container, BulletView bulletViewPrefab, float radius)
    {
        _container = container;
        _bulletViewPrefab = bulletViewPrefab;
        _bulletRadius = radius;
    }

    public Bullet Create()
    {
        var bullet = new Bullet(_bulletRadius);
        var view = _container.InstantiatePrefabForComponent<BulletView>(_bulletViewPrefab);
        view.Initialize(bullet);
        return bullet;
    }
}