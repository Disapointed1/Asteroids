using UnityEngine;

public class AsteroidView : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidBody;

    private Asteroid _asteroid;

    public void Initialize(Asteroid asteroid)
    {
        _asteroid = asteroid;
        ApplyScaleForSize(asteroid.Size);
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

    private void ApplyScaleForSize(AsteroidSize size)
    {
        float scale = size switch
        {
            AsteroidSize.Large => 1f,
            AsteroidSize.Medium => 0.6f,
            AsteroidSize.Small => 0.3f,
            _ => 1f
        };
        transform.localScale = Vector3.one * scale;
    }
}
