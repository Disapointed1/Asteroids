using System;

public class ShipStatusViewModel
{
   private readonly Ship _ship;

   public float Speed => _ship.Physics.Velocity.magnitude;
   public string PositionText => $"X: {_ship.Physics.Position.x:F1}, Y: {_ship.Physics.Position.y:F1}";
   public string RotationText => $"Rotate Angle : {_ship.Rotation:F1} ";
   public string CurrentLaserCountText => $"Laser Charges : {_ship.CurrentLaserCharges} / {_ship.MaxLaserCharges}";
   public string LaserRechargeText => $"Recharge: {_ship.TimeUntilNextCharge:F1}s";

   public event Action OnStatsChanged;

   public ShipStatusViewModel(Ship ship)
   {
       _ship = ship;
   }

   public void Tick()
   {
      OnStatsChanged?.Invoke();
   }
}
