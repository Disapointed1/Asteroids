using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    [SerializeField] private BulletView _bulletViewPrefab;
    [SerializeField] private AsteroidView _asteroidViewPrefab;
    [SerializeField] private UfoView _ufoViewPrefab;
    [SerializeField] private TouchButton _fireButton;
    [SerializeField] private TouchButton _laserButton;

    public override void InstallBindings()
    {
        GameConfigFacade configFacade = new GameConfigFacade();
        Container.Bind<GameConfigFacade>().FromInstance(configFacade).AsSingle();

        SignalBusInstaller.Install(Container);
        Container.Bind<ShipView>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesTo<GameBootstrapper>().AsSingle();
        Container.Bind<BulletFactory>().AsSingle().WithArguments(_bulletViewPrefab, configFacade.Player.BulletRadius);
        Container.Bind<LaserView>().FromComponentInHierarchy().AsSingle();
        Container.Bind<AsteroidFactory>().AsSingle().WithArguments(
            _asteroidViewPrefab,
            configFacade.Enemy.AsteroidLargeRadius,
            configFacade.Enemy.AsteroidMediumRadius,
            configFacade.Enemy.AsteroidSmallRadius);
        Container.Bind<CollisionSystemTicker>().FromComponentInHierarchy().AsSingle();
        Container.Bind<UfoFactory>().AsSingle().WithArguments(_ufoViewPrefab, configFacade.Enemy.UfoRadius, configFacade.Enemy.UfoMass);
        Container.Bind<ShipStatusView>().FromComponentInHierarchy().AsSingle();
        Container.Bind<GameScoreView>().FromComponentInHierarchy().AsSingle();
        Container.Bind<GameOverView>().FromComponentInHierarchy().AsSingle();
        Container.Bind<TouchButton>().WithId("Fire").FromInstance(_fireButton).AsCached();
        Container.Bind<TouchButton>().WithId("Laser").FromInstance(_laserButton).AsCached();
        Container.Bind<Joystick>().FromComponentInHierarchy().AsSingle();
        Container.Bind<FullScreenAdService>().FromComponentInHierarchy().AsSingle();
        Container.DeclareSignal<GameOverSignal>();
    }
}