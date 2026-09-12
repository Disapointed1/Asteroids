using UnityEngine;

public class UfoView : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidBody;

    private Ufo _ufo;
    public void Initialize(Ufo ufo)
    {
        _ufo = ufo;
        _ufo.OnSpawnedEvent += HandleSpawned;
        _ufo.OnReturnedEvent += HandleReturned;
    }

    private void HandleSpawned()
    {
        gameObject.SetActive(true);
        SyncPosition();
    }

    private void HandleReturned()
    {
        gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        _rigidBody.MovePosition(_ufo.Physics.Position);
    }

    public void SyncPosition()
    {
        transform.position = _ufo.Physics.Position;
    }
}