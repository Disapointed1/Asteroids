using UnityEngine;

public class ShipController
{
    private readonly Ship _ship;
    private readonly ShipWeapon _shipWeapon;
    private readonly IInputProvider _input;
    private readonly float _rotationSpeed;
    private readonly float _thrustPower;
    private readonly WorldBoundary _worldBoundary;
    private readonly float _bulletSpeed;


    public ShipController(Ship ship, IInputProvider input, float rotationSpeed, float thrustPower, WorldBoundary worldBoundary, ShipWeapon shipWeapon,  float bulletSpeed)
    {
        _ship =  ship;
        _input = input;
        _rotationSpeed = rotationSpeed;
        _thrustPower = thrustPower;
        _worldBoundary = worldBoundary;
        _shipWeapon = shipWeapon;
        _bulletSpeed = bulletSpeed;
    }

    public void Tick(float deltaTime)
    {
        Vector2 movementInput =  _input.GetMovementInput();
        float rotationInput = _input.GetRotationInput();

        if (!_ship.IsInvulnerable)
            _ship.ApplyRotation(rotationInput *  deltaTime * _rotationSpeed);

        Vector2 thrustDirection = GetThrustDirection();

        if (movementInput.y > 0 && !_ship.IsInvulnerable)
            _ship.Physics.ApplyAcceleration(thrustDirection * _thrustPower, deltaTime);
        else
            _ship.Physics.ApplyDrag(deltaTime);

        _ship.Physics.ClampVelocity();
        _ship.Physics.UpdatePosition(deltaTime);
        _worldBoundary.WrapPosition(_ship.Physics);

    }

    public void HandleFireInput()
    {
        if (_input.GetFireInput() && !_ship.IsInvulnerable)
        {
            Vector2 thrustDirection =GetThrustDirection();
            ICommand command = new FireBulletCommand(_shipWeapon, _ship.Physics.Position,  thrustDirection,_bulletSpeed, _ship.Rotation);
            command.Execute();
        }
    }

    public void HandleLaserInput()
    {
        if (_input.GetLaserInput() && !_ship.IsInvulnerable)
        {
            ICommand command = new FireLaserCommand(_ship);
            command.Execute();
        }
    }

    private Vector2 GetThrustDirection()
    {
        return new  Vector2(-Mathf.Sin(_ship.Rotation * Mathf.Deg2Rad),
            Mathf.Cos(_ship.Rotation * Mathf.Deg2Rad));
    }

}
