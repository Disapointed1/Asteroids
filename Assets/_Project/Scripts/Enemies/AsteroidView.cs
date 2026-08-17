using UnityEngine;

public class AsteroidView : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidBody;

    private Asteroid _asteroid;

    public void Initialize(Asteroid asteroid)
    {
        _asteroid = asteroid;
    }

    public void FixedUpdate()
    {
        _asteroid.Physics.UpdatePosition(Time.fixedDeltaTime);
        _rigidBody.MovePosition(_asteroid.Physics.Position);
    }
}
