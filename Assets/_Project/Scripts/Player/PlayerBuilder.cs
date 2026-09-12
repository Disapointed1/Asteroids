using Zenject;

public class PlayerBuilder
{
    private readonly BulletFactory _bulletFactory;
    private readonly ShipView _shipView;
    private readonly LaserView _laserView;
    private readonly GameConfigFacade _configFacade;

    public PlayerBuilder(BulletFactory bulletFactory, ShipView shipView, LaserView laserView, GameConfigFacade configFacade)
    {
        _bulletFactory = bulletFactory;
        _shipView = shipView;
        _laserView = laserView;
        _configFacade = configFacade;
    }

    public Ship Build(IInputProvider inputProvider, WorldBoundary worldBoundary, out ShipWeapon shipWeapon,
        out LaserWeapon laserWeapon, out ShipController shipController)
    {
        shipWeapon = new ShipWeapon(_bulletFactory, _configFacade.Player.FireRate);
        laserWeapon = new LaserWeapon();

        Ship ship = new Ship(_configFacade.Player.ShipRadius, _configFacade.Player.ShipMass,
            _configFacade.Player.ShipDragCoefficient, _configFacade.Player.MaxSpeed, _configFacade.Player.MaxHealth);

        shipController = new ShipController(ship, inputProvider, _configFacade.Player.RotationSpeed,
            _configFacade.Player.ThrustPower, worldBoundary, shipWeapon, _configFacade.Player.BulletSpeed, laserWeapon);

        _laserView.Initialize(ship, laserWeapon);
        _shipView.Initialize(ship, shipController);
        return ship;
    }
}