using UnityEngine;

public class AsteroidView : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidBody;

    private Asteroid _asteroid;

    private void FixedUpdate()
    {
        _rigidBody.MovePosition(_asteroid.Physics.Position);
    }

    public void Initialize(Asteroid asteroid)
    {
        _asteroid = asteroid;
        _asteroid.OnSpawnedEvent += HandleSpawned;
        _asteroid.OnReturnedEvent += HandleReturned;
    }

    private void HandleSpawned()
    {
        gameObject.SetActive(true);
        transform.localScale = Vector3.one * _asteroid.Physics.Radius;
        SyncPosition();
    }

    private void HandleReturned()
    {
        gameObject.SetActive(false);
    }

    private void SyncPosition()
    {
        transform.position = _asteroid.Physics.Position;
    }
}