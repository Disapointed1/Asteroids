using UnityEngine;

public class ShipView : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private ParticleSystem _particles;

    private Ship _ship;
    private ShipController _shipController;

    private void Update()
    {
        _shipController.HandleInput();
    }

    private void FixedUpdate()
    {
        _shipController.Tick(Time.fixedDeltaTime);
        _rigidbody2D.MovePosition(_ship.Physics.Position);
        transform.rotation = Quaternion.Euler(0, 0, _ship.Rotation);
    }

    private void OnDestroy()
    {
        _ship.OnInvulnerabilityStarted -= HandleInvulnerabilityStarted;
        _ship.OnInvulnerabilityEnded -= HandleInvulnerabilityEnded;
    }

    public void Initialize(Ship ship, ShipController shipController)
    {
        _ship = ship;
        _shipController = shipController;
        _ship.OnInvulnerabilityStarted += HandleInvulnerabilityStarted;
        _ship.OnInvulnerabilityEnded += HandleInvulnerabilityEnded;
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