using UnityEngine;

public class TouchInputProvider : IInputProvider
{
   private readonly Joystick _joystick;
   private readonly TouchButton _fireButton;
   private readonly TouchButton _laserButton;

   public TouchInputProvider(Joystick joystick, TouchButton fireButton, TouchButton laserButton)
   {
      _joystick = joystick;
      _fireButton = fireButton;
      _laserButton = laserButton;
   }

   public Vector2 GetMovementInput()
   {
      return _joystick.InputDirection;
   }

   public float GetRotationInput()
   {
      return _joystick.InputDirection.x;
   }

   public bool GetFireInput()
   {
      return _fireButton.IsPressed;
   }

   public bool GetLaserInput()
   {
      return _laserButton.IsPressed;
   }
}
