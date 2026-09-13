public class PlayerBuilder
{
    private readonly BulletFactory _bulletFactory;
    private readonly GameConfigProvider _configProvider;
    private readonly LaserView _laserView;
    private readonly ShipView _shipView;

    public PlayerBuilder(BulletFactory bulletFactory, ShipView shipView, LaserView laserView,
        GameConfigProvider configProvider)
    {
        _bulletFactory = bulletFactory;
        _shipView = shipView;
        _laserView = laserView;
        _configProvider = configProvider;
    }

    public Ship Build(IInputProvider inputProvider, WorldBoundary worldBoundary, out ShipWeapon shipWeapon,
        out LaserWeapon laserWeapon, out ShipController shipController)
    {
        shipWeapon = new ShipWeapon(_bulletFactory, _configProvider.Player.FireRate,
            _configProvider.Player.BulletLifeTime);
        laserWeapon = new LaserWeapon(_configProvider.Player.MaxLaserCharges, _configProvider.Player.LaserRechargeTime);

        var ship = new Ship(_configProvider.Player.ShipRadius, _configProvider.Player.ShipMass,
            _configProvider.Player.ShipDragCoefficient, _configProvider.Player.MaxSpeed,
            _configProvider.Player.MaxHealth,
            _configProvider.Player.InvulnerabilityDuration);

        shipController = new ShipController(ship, inputProvider, _configProvider.Player.RotationSpeed,
            _configProvider.Player.ThrustPower, worldBoundary, shipWeapon, _configProvider.Player.BulletSpeed,
            laserWeapon);

        _laserView.Initialize(ship, laserWeapon);
        _shipView.Initialize(ship, shipController);
        return ship;
    }
}