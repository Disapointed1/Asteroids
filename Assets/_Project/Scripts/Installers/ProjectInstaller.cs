using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    [SerializeField] private BulletView _bulletViewPrefab;
    [SerializeField] private AsteroidView _asteroidViewPrefab;
    [SerializeField] private UfoView _ufoViewPrefab;

    public override void InstallBindings()
    {
        var configProvider = new GameConfigProvider();
        Container.Bind<GameConfigProvider>().FromInstance(configProvider).AsSingle();

        SignalBusInstaller.Install(Container);
        Container.Bind<ShipView>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesTo<GameBootstrapper>().AsSingle();
        Container.Bind<BulletFactory>().AsSingle().WithArguments(_bulletViewPrefab, configProvider.Player.BulletRadius);
        Container.Bind<LaserView>().FromComponentInHierarchy().AsSingle();
        Container.Bind<AsteroidFactory>().AsSingle().WithArguments(
            _asteroidViewPrefab,
            configProvider.Enemy.AsteroidLargeRadius,
            configProvider.Enemy.AsteroidMediumRadius,
            configProvider.Enemy.AsteroidSmallRadius);
        Container.Bind<CollisionSystemTicker>().FromComponentInHierarchy().AsSingle();
        Container.Bind<UfoFactory>().AsSingle().WithArguments(_ufoViewPrefab, configProvider.Enemy.UfoRadius,
            configProvider.Enemy.UfoMass);
        Container.Bind<ShipStatusView>().FromComponentInHierarchy().AsSingle();
        Container.Bind<GameScoreView>().FromComponentInHierarchy().AsSingle();
        Container.Bind<GameOverView>().FromComponentInHierarchy().AsSingle();
        Container.Bind<TouchControlsView>().FromComponentInHierarchy().AsSingle();
        Container.Bind<FullScreenAdService>().FromComponentInHierarchy().AsSingle();
        Container.Bind<InputProviderFactory>().AsSingle();
        Container.Bind<PlayerBuilder>().AsSingle();
        Container.Bind<EnemySystemBuilder>().AsSingle();
        Container.Bind<GameplaySystemBuilder>().AsSingle();
        Container.Bind<CollisionDetector>().AsSingle();
        Container.Bind<CollisionResolver>().AsSingle();
        Container.DeclareSignal<GameOverSignal>();
        Container.Bind<Camera>().FromComponentInHierarchy().AsSingle();
        Container.Bind<FirebaseAnalyticsService>().AsSingle();
        Container.Bind<SceneLoader>().AsSingle();
        Container.Bind<PauseService>().AsSingle();
    }
}