using System;

public class ShipStatusViewModel
{
    private readonly LaserWeapon _laserWeapon;
    private readonly Ship _ship;

    public ShipStatusViewModel(Ship ship, LaserWeapon laserWeapon)
    {
        _ship = ship;
        _laserWeapon = laserWeapon;
    }

    public float Speed => _ship.Physics.Velocity.magnitude;
    public string PositionText => $"X: {_ship.Physics.Position.x:F1}, Y: {_ship.Physics.Position.y:F1}";
    public string RotationText => $"Rotate Angle : {_ship.Rotation:F1} ";

    public string CurrentLaserCountText =>
        $"Laser Charges : {_laserWeapon.CurrentLaserCharges} / {_laserWeapon.MaxCharges}";

    public string LaserRechargeText => $"Recharge: {_laserWeapon.TimeUntilNextCharges:F1}s";

    public event Action OnStatsChanged;

    public void Tick()
    {
        OnStatsChanged?.Invoke();
    }
}