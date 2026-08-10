
using Zenject;


public class ProjectInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);
        Container.Bind<ShipView>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesTo<GameBootstrapper>().AsSingle();
    }
}
