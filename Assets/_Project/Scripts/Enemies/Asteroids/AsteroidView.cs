using UnityEngine;

public class AsteroidView : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidBody;

    private Asteroid _asteroid;

    public void Initialize(Asteroid asteroid)
    {
        _asteroid = asteroid;
        transform.localScale = Vector3.one * asteroid.Physics.Radius;
    }

    public void FixedUpdate()
    {
        _asteroid.Physics.UpdatePosition(Time.fixedDeltaTime);
        _rigidBody.MovePosition(_asteroid.Physics.Position);
    }

    public void SyncPosition()
    {
        transform.position = _asteroid.Physics.Position;
    }
}