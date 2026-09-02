using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    [SerializeField] private BulletView _bulletViewPrefab;
    [SerializeField] private AsteroidView _asteroidViewPrefab;
    [SerializeField] private UfoView _ufoViewPrefab;

    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);
        Container.Bind<ShipView>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesTo<GameBootstrapper>().AsSingle();
        Container.Bind<BulletFactory>().AsSingle().WithArguments(_bulletViewPrefab,0.1f);
        Container.Bind<LaserView>().FromComponentInHierarchy().AsSingle();
        Container.Bind<AsteroidFactory>().AsSingle().WithArguments(_asteroidViewPrefab);
        Container.Bind<CollisionSystemTicker>().FromComponentInHierarchy().AsSingle();
        Container.Bind<UfoFactory>().AsSingle().WithArguments(_ufoViewPrefab, 0.4f, 1f);
        Container.Bind<ShipStatusView>().FromComponentInHierarchy().AsSingle();
    }
}
