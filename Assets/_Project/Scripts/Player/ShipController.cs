using UnityEngine;

public class ShipController
{
    private readonly Ship _ship;
    private readonly IInputProvider _input;
    private readonly float _rotationSpeed;
    private readonly float _thrustPower;
    private readonly WorldBoundary _worldBoundary;

    public ShipController(Ship ship, IInputProvider input, float rotationSpeed, float thrustPower, WorldBoundary worldBoundary)
    {
        _ship =  ship;
        _input = input;
        _rotationSpeed = rotationSpeed;
        _thrustPower = thrustPower;
        _worldBoundary = worldBoundary;

    }

    public void Tick(float deltaTime)
    {
        Vector2 movementInput =  _input.GetMovementInput();
        float rotationInput = _input.GetRotationInput();

        _ship.ApplyRotation(rotationInput *  deltaTime * _rotationSpeed);

        Vector2 thrustDirection = new Vector2(
            -Mathf.Sin(_ship.Rotation * Mathf.Deg2Rad),
            Mathf.Cos(_ship.Rotation * Mathf.Deg2Rad));

        if (movementInput.y > 0)
            _ship.Physics.ApplyAcceleration(thrustDirection * _thrustPower, deltaTime);
        else
            _ship.Physics.ApplyDrag(deltaTime);
        _ship.Physics.UpdatePosition(deltaTime);
        _worldBoundary.WrapPosition(_ship.Physics);

    }

}
