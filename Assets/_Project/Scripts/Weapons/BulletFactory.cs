using Zenject;

public class BulletFactory
{
    private readonly DiContainer _container;
    private readonly BulletView _bulletViewPrefab;
    private readonly float _bulletRadius;

    public BulletFactory(DiContainer container, BulletView bulletViewPrefab, float radius)
    {
        _container = container;
        _bulletViewPrefab = bulletViewPrefab;
        _bulletRadius = radius;
    }

    public Bullet Create()
    {
        Bullet bullet = new Bullet(_bulletRadius);
        BulletView view = _container.InstantiatePrefabForComponent<BulletView>(_bulletViewPrefab);
        view.Initialize(bullet);
        return bullet;
    }
}
