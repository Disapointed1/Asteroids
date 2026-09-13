using UnityEngine;

public class FireBulletCommand : ICommand
{
    private readonly Vector2 _direction;
    private readonly Vector2 _position;
    private readonly float _rotation;
    private readonly float _speed;
    private readonly ShipWeapon _weapon;

    public FireBulletCommand(ShipWeapon weapon, Vector2 position, Vector2 direction, float speed, float rotation)
    {
        _weapon = weapon;
        _position = position;
        _direction = direction;
        _speed = speed;
        _rotation = rotation;
    }

    public void Execute()
    {
        _weapon.Fire(_position, _direction, _speed, _rotation);
    }
}