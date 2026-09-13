using UnityEngine;

public class ShipController
{
    private readonly float _bulletSpeed;
    private readonly IInputProvider _input;
    private readonly LaserWeapon _laserWeapon;
    private readonly float _rotationSpeed;
    private readonly Ship _ship;
    private readonly ShipWeapon _shipWeapon;
    private readonly float _thrustPower;
    private readonly WorldBoundary _worldBoundary;


    public ShipController(Ship ship, IInputProvider input, float rotationSpeed, float thrustPower,
        WorldBoundary worldBoundary, ShipWeapon shipWeapon, float bulletSpeed, LaserWeapon laserWeapon)
    {
        _ship = ship;
        _input = input;
        _rotationSpeed = rotationSpeed;
        _thrustPower = thrustPower;
        _worldBoundary = worldBoundary;
        _shipWeapon = shipWeapon;
        _bulletSpeed = bulletSpeed;
        _laserWeapon = laserWeapon;
    }

    public void Tick(float deltaTime)
    {
        var movementInput = _input.GetMovementInput();
        var rotationInput = _input.GetRotationInput();

        if (!_ship.IsInvulnerable)
            _ship.ApplyRotation(rotationInput * deltaTime * _rotationSpeed);

        var thrustDirection = GetThrustDirection();

        if (movementInput.y > 0 && !_ship.IsInvulnerable)
            _ship.Physics.ApplyAcceleration(thrustDirection * _thrustPower, deltaTime);
        else
            _ship.Physics.ApplyDrag(deltaTime);

        _ship.Physics.ClampVelocity();
        _ship.Physics.UpdatePosition(deltaTime);
        _worldBoundary.WrapPosition(_ship.Physics);
    }

    private void HandleFireInput()
    {
        if (_input.GetFireInput() && !_ship.IsInvulnerable)
        {
            var thrustDirection = GetThrustDirection();
            ICommand command = new FireBulletCommand(_shipWeapon, _ship.Physics.Position, thrustDirection, _bulletSpeed,
                _ship.Rotation);
            command.Execute();
        }
    }

    private void HandleLaserInput()
    {
        if (_input.GetLaserInput() && !_ship.IsInvulnerable)
        {
            ICommand command = new FireLaserCommand(_laserWeapon);
            command.Execute();
        }
    }

    public void HandleInput()
    {
        HandleFireInput();
        HandleLaserInput();
    }

    private Vector2 GetThrustDirection()
    {
        return DirectionMath.FromAngle(_ship.Rotation);
    }
}