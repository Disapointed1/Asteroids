using System;

public class ShipStatusViewModel
{
   private readonly Ship _ship;

   public float Speed => _ship.Physics.Velocity.magnitude;

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
