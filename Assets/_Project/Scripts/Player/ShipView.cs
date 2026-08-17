using UnityEngine;

public class ShipView : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private ParticleSystem _particles;

    private Ship _ship;
    private ShipController _shipController;
    private CollisionSystem _collisionSystem;

    public void Initialize(Ship ship, ShipController shipController, CollisionSystem collisionSystem)
    {
        _ship = ship;
        _shipController = shipController;
        _collisionSystem = collisionSystem;
        _ship.OnInvulnerabilityStarted += HandleInvulnerabilityStarted;
        _ship.OnInvulnerabilityEnded += HandleInvulnerabilityEnded;
    }

    private void FixedUpdate()
    {
        _shipController.Tick(Time.fixedDeltaTime);
        _rigidbody2D.MovePosition(_ship.Physics.Position);
        transform.rotation = Quaternion.Euler(0, 0, _ship.Rotation);
    }

    private void Update()
    {
        _shipController.HandleFireInput();
        _shipController.HandleLaserInput();
    }

    private void HandleInvulnerabilityStarted()
    {
        _particles.Play();
    }

    private void HandleInvulnerabilityEnded()
    {
        _particles.Stop();
    }

}
