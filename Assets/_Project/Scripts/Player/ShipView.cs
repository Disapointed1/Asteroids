using UnityEngine;

public class ShipView : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody2D;

    private Ship _ship;
    private ShipController _shipController;

    public void Initialize(Ship ship, ShipController shipController)
    {
        _ship = ship;
        _shipController = shipController;
    }

    private void FixedUpdate()
    {
        Debug.Log(_ship.Physics.Velocity.magnitude);
        _shipController.Tick(Time.fixedDeltaTime);
        _rigidbody2D.MovePosition(_ship.Physics.Position);
        transform.rotation = Quaternion.Euler(0,0,_ship.Rotation);
    }

}
